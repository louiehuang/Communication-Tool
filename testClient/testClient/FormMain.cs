using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Collections;
using System.Xml;
using Aptech.UI;
using CCWin;
using CCWin.SkinClass;
using CCWin.SkinControl;
using System.Threading;
using System.Text.RegularExpressions;

/*
 * private void timer1_Tick(object sender, EventArgs e) 启动FormMain时与服务器连接，之后每隔100ms向服务器发送客户端产生的信息
 * public void SendMsg(string msg)
 * public void SendMsg(byte[] msg) 
 * public void ReceiveMsg(IAsyncResult result)
 * public void ProcessTalk(string nums, string mymsg)
 * private void sideBar1_ItemDoubleClick(Aptech.UI.SbItemEventArgs e)
 * public void myupdate(string msg)
 */


namespace testClient
{
    public partial class FormMain : CCSkinMain
    {
        public static string Pmynum;

        TcpClient myclient;
        NetworkStream networkStream;
        byte[] buffer; //缓存接收到和要发送的信息
        int size = 8192;

        private HandleMsg MsgHandle;//处理收到的字符
        public delegate void updataMain(string msg);
        public Hashtable UserMsg = new Hashtable();

        static int msgnums = 0;
        private delegate void Showicon(int i);
        private bool ShowIcon;
        /// <summary>
        /// 1代表登陆成功状态，2代表有系统消息来,3、4代码正在登陆中,6代表有人Q你，5代表登陆失败
        /// </summary>
        private int IconMode;


        static AutoResetEvent myResetEvent = new AutoResetEvent(false);
        static string share;


        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载窗体，设置sideBar的分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            MsgHandle = new HandleMsg();
        }

        private void sideBar1_MouseDown(object sender, MouseEventArgs e)
        {

        }


        /// <summary>
        /// 启动FormMain时与服务器连接进行登录，之后每隔100ms向服务器发送客户端产生的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (myInfo.Login != null)//登录操作只执行一次
                {
                    // FormLogin点击登录后myInfo.Login = "[login][account,pwd]", 如[login][10151100,pass]
                    string temp = myInfo.Login;
                    myInfo.Login = null; //登录只执行一次，但timer不断在运行，所以执行过一次后置空

                    /*与服务器连接*/
                    myclient = new TcpClient();
                    myclient.Connect("127.0.0.1", 500);
                    //myclient.Connect("10.146.93.17",500);

                    /*获得网络流*/
                    networkStream = myclient.GetStream();
                    buffer = new byte[size];

                    /*发送信息(登录信息)*/
                    SendMsg(temp);

                    /* 异步读取服务器反馈信息*/
                    lock (networkStream)
                    {
                        AsyncCallback asy = new AsyncCallback(ReceiveMsg);
                        networkStream.BeginRead(buffer, 0, size, asy, null);
                    }
                }

                if (myInfo.ChildFromMsg != string.Empty)//表示有消息从子窗体中传过来
                {
                    string temp = myInfo.ChildFromMsg;//赋给临时变量
                    myInfo.ChildFromMsg = string.Empty;//清空，方便再次有消息传过来

                    SendMsg(temp);//发送消息
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("timer1控件: " + ex.Message);
                timer1.Stop();
            }
        }

        /// <summary>
        /// 发送字符串型消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            msg += "[e]"; //末尾设置分隔符
            byte[] tempmsg = Encoding.Unicode.GetBytes(msg);
            /*写入网络流，并刷新*/
            networkStream.Write(tempmsg, 0, tempmsg.Length);
            networkStream.Flush();
        }

        /// <summary>
        /// 发送字节型消息(文本中含有图片时调用)
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(byte[] msg)//特殊
        {
            networkStream.Write(msg, 0, msg.Length);
            networkStream.Flush();
        }

        /// <summary>
        /// 接收服务器发回消息
        /// </summary>
        /// <param name="result"></param>
        public void ReceiveMsg(IAsyncResult result)
        {
            try
            {
                int readbytes;
                /*结束异步的BeginRead，获取所读字节数*/
                lock (networkStream)
                {
                    readbytes = networkStream.EndRead(result);
                }
                if (readbytes < 1) //
                {
                    MessageBox.Show("读取信息时异常");
                }
                else //正常接收信息
                {
                    /*将所读字节数组buffer解码为string，赋给Readmsg*/
                    string Readmsg = Encoding.Unicode.GetString(buffer, 0, readbytes);
                    Array.Clear(buffer, 0, buffer.Length);//清空缓存

                    /*处理所得的Readmsg*/

                    if (Readmsg.StartsWith("[off]"))
                    {
                        /*离线消息归类*/
                        /*
                         * [off][talk][101511]Shane 22:59:03:\nhello[e][talk][100001]Alice 22:59:30: Hi[e][talk][101511]Shane 22:59:42: FromShane[e]
                         * 分成
                         * ClassfiedMsg[0] = "[talk][101511]Shane 22:59:03:\nhello\nShane 22:59:42:\nFromShane\n"
                         * ClassfiedMsg[1] = "[talk][100001]Alice 22:59:30:\nHi\n"
                         */
                        string[] LeftMsg = new string[100];

                        //归类
                        Regex reg_account = new Regex(@"\d{6}");
                        Match m = reg_account.Match(Readmsg);


                        List<string> RawMsg = new List<string>();
                        string rawmsg = Readmsg.Substring(5);
                        //MessageBox.Show(rawmsg);
                        int splitindex = rawmsg.IndexOf("[e]");
                        while (splitindex != -1)
                        {
                            RawMsg.Add(rawmsg.Substring(0, splitindex));
                            //MessageBox.Show(rawmsg.Substring(0,splitindex));
                            rawmsg = rawmsg.Substring(splitindex + 3);
                            splitindex = rawmsg.IndexOf("[e]");
                        }

                        /*
                         RawMsg[0] = "[talk][101511]Shane 22:59:03:\nhello" 
                         RawMsg[1] = "[talk][100001]Alice 22:59:30:\nHi" 
                         RawMsg[2] = "[talk][101511]Shane 22:59:42:\nFromShane"
                         */

                        int index = 0;
                        List<string> ClassfiedMsg = new List<string>();
                        foreach (string singlemsg in RawMsg)
                        {
                            int flag = 0;
                            string account = singlemsg.Substring(7, 6);
                            foreach (DictionaryEntry msg in UserMsg)
                            {
                                if (msg.Key.ToString() == account) //找到
                                {
                                    flag = 1;
                                    int pos = int.Parse(msg.Value.ToString());

                                    string onemsg = singlemsg.Substring(14) + "\n"; //Shane 22:59:03:\nhello\n

                                    ClassfiedMsg[pos] += onemsg;

                                    //MessageBox.Show("found: " + ClassfiedMsg[pos]);

                                }
                            }
                            if (flag == 0)//没有找到
                            {
                                //singlemsg = "[talk][101511]Shane 22:59:03:\nhello" 
                                UserMsg.Add(account, index);
                                string onemsg = singlemsg + "\n";
                                ClassfiedMsg.Insert(index, onemsg);

                                //MessageBox.Show("First time: " + onemsg);
                                index++;
                            }
                        }

                        foreach (string leftmsg in ClassfiedMsg)
                        {
                            string nums = leftmsg.Substring(7, 6); //发送者id
                            string mymsg = leftmsg.Substring(14); //msg
                            //mymsg = "Shane 22:59:03:\nhello\nShane 22:59:42:\nFromShane\n"
                            this.ProcessTalk(nums, mymsg);
                        }


                    } //if离线消息
                    else
                    {
                        MsgHandle.MSG = Readmsg;
                        string[] AllMsg = MsgHandle.process();
                        if (AllMsg.Length != 0)
                        {
                            foreach (string msg in AllMsg)
                            {
                                //MessageBox.Show(msg);
                                if (msg.StartsWith("[welcome]"))//登陆成功
                                {
                                    //[welcome]<all><MySimple><qqnum10151100 name="Shane" img="1" sign="no"><qqnum10151100></MySimple>
                                    //<friends><qqnum25384137 name="Bryan" img="2" ison="yes"></qqnum25384137></friend></all>
                                    string temp = msg.Substring(9);
                                    /*更新窗体，载入好友列表**/
                                    this.Invoke(new updataMain(myupdate), new object[] { temp });
                                }
                                else if (msg.StartsWith("[talk]")) //会话请求
                                {
                                    //101511发给253841信息:hello，则msg = "[talk][101511]Shane 18:33:59:\nhello"
                                    //此处客户端为253841所打开的客户端
                                    string nums = msg.Substring(7, 6); //发送者id
                                    string mymsg = msg.Substring(14); //msg

                                    this.ProcessTalk(nums, mymsg);
                                }
                                else if (msg.StartsWith("[group]"))
                                {
                                    //MessageBox.Show("1 --- " + msg);
                                    //101511向群组000001发送信息:hello，则msg = "[group][000001]Shane 18:34:09:\hello"
                                    string groupid = msg.Substring(8, 6);
                                    string groupmsg = msg.Substring(15); //Shane 18:34:09:\hello

                                    this.ProcessGroupMsg(groupid, groupmsg);
                                }
                            }
                        }
                    } //else
                } //else
                lock (networkStream)
                {
                    AsyncCallback asy = new AsyncCallback(ReceiveMsg);
                    networkStream.BeginRead(buffer, 0, size, asy, null);
                }
            }
            catch (Exception ex)
            {
                //出现异常用户断开
                MessageBox.Show("breakdown: " + ex.Message);
            }
        }

        /// <summary>
        /// 处理收到聊天的信息
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="mymsg"></param>
        public void ProcessTalk(string nums, string mymsg)
        {
            // Shane在18:33:59给Bryan发"hello",则Shane向服务器发送msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
            // Bryan收到的msg = "[talk][101511]Shane 18:33:59:\nhello", nums="101511", mymsg="Shane 18:33:59:\nhello"
            //MessageBox.Show("nums="+nums+"msg"+mymsg);
            string name = "";
            int img = 0;
            bool chatformisopen = false;//表示是否有该窗体
            byte[] msgbyte = Encoding.Unicode.GetBytes(mymsg);//转成byte

            //要从sidebar中搜索，获取nums的头像，昵称，如果没有就直接用num开启聊天窗口
            //如果窗口没打开，显示在托盘
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < clb_friend.Items[i].SubItems.Count; j++)
                {
                    if (clb_friend.Items[i].SubItems[j].Tag.ToString() == nums) //通过发送者账号获得发送者昵称
                    {
                        name = clb_friend.Items[i].SubItems[j].NicName;
                        img = (int)clb_friend.Items[i].SubItems[j].ID;
                        //MessageBox.Show(name);
                    }
                }
            }

            //MessageBox.Show("process");
            //查找聊天窗口是否有打开
            foreach (DictionaryEntry chats in myInfo.ChatForm)
            {
                if (chats.Key.ToString() == nums)//有该窗体存在
                {
                    //MessageBox.Show("process1");
                    chatformisopen = true;
                    //更新聊天窗口中消息信息
                    ((FormChat)chats.Value).BeginInvoke(new UpdateChat(((FormChat)chats.Value).updatemsg), new object[] { msgbyte });
                }
            }

            //窗口未打开
            if (!chatformisopen)//显示到托盘
            {
                //把消息打包
                //allmsgs = "[msg]101511,Shane,5,Shane 18:33:59:\nhello"

                //离线: mymsg = Shane 12:44:50:\nhello\nShane 12:45:08:\nHi[e][talk][100001]\nShane\n12:46:00:\nFromShaneAgain\n
                //[msg]101511],5,Shane 12:44:50:\nhello\nShane 12:45:08:\nHi[e][talk][100001]\nShane\n12:46:00:\nFromShaneAgain\n
                string allmsgs = "[msg]" + nums + "," + name + "," + img.ToString() + "," + mymsg;
                this.BeginInvoke(new Showicon(myshow), new object[] { 6 }); //委托，托盘图标闪烁

                updataMain handle = new updataMain(OnebyOne);
                handle.BeginInvoke(allmsgs, null, null);
                msgnums++;
            }
            
        }

        public void ProcessGroupMsg(string groupid, string groupmsg)
        {
            // Shane在18:33:59给群组1发"hello",则Shane向服务器发送msg = "[talk][101511,000001]Shane 18:33:59:\nhello",
            // Bryan收到的msg = "[group][000001]Shane 18:33:59:\nhello", groupid="000001", groupmsg="Shane 18:33:59:\nhello"

            string name = "";
            int img = 0;
            bool groupchatformisopen = false;//表示是否有该窗体
            byte[] msgbyte = Encoding.Unicode.GetBytes(groupmsg);//转成byte

            //要从clb_group中搜索，获取groupid的header，name
            //如果窗口没打开，显示在托盘
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j <  clb_group.Items[i].SubItems.Count; j++)
                {
                    if (clb_group.Items[i].SubItems[j].Tag.ToString() == groupid) //通过发送者账号获得发送者昵称
                    {
                        name = clb_group.Items[i].SubItems[j].NicName;
                        img = (int)clb_group.Items[i].SubItems[j].ID;
                        //MessageBox.Show(name);
                    }
                }
            }

            //MessageBox.Show("process");
            //查找聊天窗口是否有打开
            foreach (DictionaryEntry groupchats in myInfo.GroupChatForm)
            {
                if (groupchats.Key.ToString() == groupid)//有该窗体存在
                {
                    MessageBox.Show("processGroupMsg");
                    groupchatformisopen = true;
                    //更新聊天窗口中消息信息
                    ((GroupChat)groupchats.Value).BeginInvoke(new UpdateChat(((GroupChat)groupchats.Value).updatemsg), new object[] { msgbyte });
                }
            }

            //窗口未打开
            if (!groupchatformisopen)//显示到托盘
            {
                //把消息打包
                //allmsgs = "[msg]101511,Shane,5,Shane 18:33:59:\nhello"

                //msg = [msg]101511,Shane,5,Shane 12:44:50:\nhello\nShane 12:45:08:\nHi[e][talk][100001]\nShane\n12:46:00:\nFromShaneAgain\n
                
                //群组: [msg]000001,Programming,2,Shane 12:44:50:\nhello\nAlice 12:45:23:\nHi\n
                string allmsgs = "[msg]" + groupid + "," + name + "," + img.ToString() + "," + groupmsg;
                this.BeginInvoke(new Showicon(myshow), new object[] { 6 }); //委托，托盘图标闪烁

                updataMain handle = new updataMain(OnebyOne);
                handle.BeginInvoke(allmsgs, null, null);
                msgnums++;
            }
        
        }


        public void OnebyOne(string msg)
        {
            myResetEvent.WaitOne(); //阻塞当前线程,直到收到信号(点击托盘图标时发送解锁信号)

            share = msg;
            if (share.StartsWith("[msg]"))//有人发来消息
            {
                //[msg]num,name,img,msg
                this.BeginInvoke(new updataMain(ShowFormChat), new object[] { msg });
            }
            msgnums--;
        }

        public void ShowFormChat(string msg)
        {
            //msg = "[msg]101511,Shane,5,Shane 18:33:59:\nhello,hi"
            //离线:msg = [msg]101511,Shane,5,Shane 12:44:50:\nhello\nShane 12:45:08:\nHi[e][talk][100001]\nShane\n12:46:00:\nFromShaneAgain\n
            
            /*这里消息截图需要改，消息中含逗号时会被误分*/
            string[] alltemp = share.Substring(5).Split(',');
            byte[] mymsg = Encoding.Unicode.GetBytes(alltemp[3]);

            if (int.Parse(alltemp[0]) >= 100000) //用户消息
            {
                FormChat mychat = new FormChat(this.pictureBox1.Tag.ToString(), alltemp[0], label1.Text, alltemp[1], int.Parse(alltemp[2]));
                mychat.Show();
                mychat.BeginInvoke(new UpdateChat(mychat.updatemsg), new object[] { mymsg });
            }
            else //群组消息
            {
                //[msg]000001,Programming,2,Shane 12:44:50:\nhello\nAlice 12:45:23:\nHi\n
                //获取群组的所有用户
                GroupChat groupchat = new GroupChat(this.pictureBox1.Tag.ToString(), alltemp[0], label1.Text, alltemp[1], int.Parse(alltemp[2]));


            }

        }


        /*myshow, timer2_Tick, ShowIconMode三个方法实现在托盘的图标闪烁*/
        /// <summary>显示相应的托盘图标</summary>
        /// <param name="i"></param>
        public void myshow(int i)
        {
            if (this.timer2.Enabled)
            {
                IconMode = i;
            }
            else
            {
                IconMode = i;
                timer2.Start();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (IconMode == 6)//有人Q你
            {
                // 通过ShowIcon的切换来实现图标(两个，一大一小)闪烁效果
                if (ShowIcon)
                {
                    ShowIcon = false;
                    this.ShowIconModel(6);
                }
                else
                {
                    ShowIcon = true;
                    this.ShowIconModel(7);
                }
            }
        }

        private void ShowIconModel(int ShowMode)//显示图标
        {
            switch (ShowMode)
            {
                case 1://登陆后显示正常图标
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\App.ico");
                    break;
                case 2://有系统消息来
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\MsgManagerButton.ico");
                    break;
                case 3://登陆前显示向左图标
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\2.ico");
                    break;
                case 4://登陆前显示向右图标
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\1.ico");
                    break;
                case 5://显示离线图片
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\left.ico");
                    break;
                case 6://显示有人Q你
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\f1.ico");
                    break;
                case 7://显示有人Q你
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\f2.ico");
                    break;
                case 8://有系统消息来
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\system.ico");
                    break;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Activate();
        }

        //点击托盘的消息图标
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (msgnums > 0)
            {
                myResetEvent.Set();
                if (msgnums == 1)
                {
                    timer2.Stop();
                    ShowIconModel(1);
                }
            }
        }



        /// <summary>
        /// 双击好友项时弹出聊天窗口
        /// </summary>
        /// <param name="e"></param>
        private void clb_friend_DoubleClickSubItem(object sender, ChatListEventArgs e, MouseEventArgs es)
        {
            // Tag 为对方账号
            //string num = sideBar1.SeletedItem.Tag.ToString();
            //MessageBox.Show(num);
            if (es.Button == MouseButtons.Right) 
            {
                return;
            }
            ChatListSubItem item = e.SelectSubItem;
            item.IsTwinkle = false;

            string num = item.Tag.ToString();

            bool isshow = false;
            // 若已经打开了与被选中对象的聊天窗口，则给焦点激活
            foreach (DictionaryEntry mychat in myInfo.ChatForm)
            {
                if (mychat.Key.ToString() == num)
                {
                    isshow = true;
                    ((FormChat)mychat.Value).Activate();
                }
            }

            // 若没有打开过与该对象的聊天窗口，则构造窗口并显示
            if (!isshow)
            {
                //获取自己的账号id
                string mynum = pictureBox1.Tag.ToString();

                //获取选中好友的名称
                string name = item.NicName;

                //获取选中好友的头像索引
                int img = (int)item.ID; //用ID属性表示头像索引

                //实例化一个聊天界面
                FormChat chat = new FormChat(mynum, num, label1.Text, name, img);
                chat.Show();
            }
        }


        private void clb_group_DoubleClickSubItem(object sender, ChatListEventArgs e, MouseEventArgs es)
        {
            // Tag 为群组号
            if (es.Button == MouseButtons.Right)
            {
                return;
            }
            ChatListSubItem item = e.SelectSubItem;
            item.IsTwinkle = false;

            string num = item.Tag.ToString(); //000001

            bool isshow = false;
            // 若已经打开了与被选中对象的聊天窗口，则给焦点激活
            foreach (DictionaryEntry mygroupchat in myInfo.GroupChatForm)
            {
                if (mygroupchat.Key.ToString() == num)
                {
                    isshow = true;
                    ((FormChat)mygroupchat.Value).Activate();
                }
            }

            // 若没有打开过与该对象的聊天窗口，则构造窗口并显示
            if (!isshow)
            {
                //获取自己的账号id
                string mynum = pictureBox1.Tag.ToString();

                //获取选中群组的名称
                string name = item.NicName;

                //获取选中群组的头像索引
                int img = (int)item.ID; //用ID属性表示头像索引

                //实例化一个聊天界面
                GroupChat groupchat = new GroupChat(mynum, num, label1.Text, name, img);
                groupchat.Show();
            }
        }


        /// <summary>
        /// 由xml形式的字符串加载自己的信息和好友资料
        /// </summary>
        /// <param name="msg"></param>
        public void myupdate(string msg)
        {
            /* Xml基础操作:
             *  XmlDocument xmlDoc = new XmlDocument();   //创建XmlDocument对象 
             *  xmlDoc.Load(filename);           //url载入xml文件名 
             *  xmlDoc.LoadXml(xmldata);    //如果是xml字符串，则用此形式
             *  XmlNodeList xn0 = xmlDoc.SelectSingleNode("Document").ChildNodes;   //读取根节点的所有子节点，放到xn0中
             *  //查找二级节点的内容或属性
             * foreach (XmlNode node in xn0)
             * {
             *      if(node.Name == 匹配的二级节点名)
             *      {
             *          string innertext = node.InnerText.Trim(); //匹配二级节点的内容
             *          string attr = node.Attributes[0].ToString(); //属性
             *      }
             * }
             */

            /*
             * <all>
             *          <MySimple>
             *                  <qqnum101511 name="Shane" img="19" sign="签名测试"></qqnum101511>
             *          </MySimple>
             *          <friends>
             *                  <qqnum253841 name="Bryan" img="1" ison=0></qqnum253841>
             *                  <qqnum100002 name="测试用户" img="4" ison="yes"></qqnum100002>
             *          </friend>
             *          <groups>
             *                  <groupitem num="000001" name="DataBase" header="1"></groupitem>
             *                  <groupitem num="000002" name="C#" header="2"></groupitem>
             *          </groups>
             * </all>
             */

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg);

            //遍历根节点<all></all>的所有子节点
            XmlNodeList xmlnodeList = doc.SelectSingleNode("all").ChildNodes;
            foreach (XmlNode SuperirNode in xmlnodeList)
            {
                /*该账号自己的资料(昵称，账号，签名，头像)*/
                if (SuperirNode.Name == "MySimple")
                {
                    foreach (XmlNode child in SuperirNode.ChildNodes)
                    {
                        label1.Text = child.Attributes["name"].Value; //获取自己的昵称
                        int imageindex = int.Parse(child.Attributes["img"].Value); //获取自己的头像索引
                        label2.Text = child.Attributes["sign"].Value; //获取自己的签名

                        /*置pictureBox的Image自己的头像，Tag为自己的账号:10151100*/
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\" + imageindex.ToString() + ".jpg");
                        pictureBox1.Tag = (object)child.Name.Substring(5); //child.Name = "qqnum10151100"
                        //MessageBox.Show(pictureBox1.Tag.ToString());
                        FormMain.Pmynum = pictureBox1.Tag.ToString();
                    }
                    //MessageBox.Show("本人信息载入完成");
                }
                /*该账号的好友信息*/
               else if (SuperirNode.Name == "friends")
                {
                    /*
                     //实例化SbItem对象，需要一个字符串和一个整形值座位参数
                    SbItem item = new SbItem((string)reader["NickName"], (int)reader["FaceId"]);
                    //把查询出来的好友帐号赋值给item的Tag标签
                    item.Tag = (int)reader["FriendId"];
                    //把item对象添加到好友组中
                    sbFriends.Groups[0].Items.Add(item);
                     */

                    //sideBar1.Groups[0].Items.Clear();

                    /*替换sidebar1*/
                    clb_friend.Items[0].SubItems.Clear();

                    foreach (XmlNode child in SuperirNode.ChildNodes)
                    {
                        //<qqnum253841 name="Bryan" img="2" ison="1"></qqnum253841>
                        //<qqnum333841 name="admin" img="3" ison="1"></qqnum333841>
                        int imageindex = int.Parse(child.Attributes["img"].Value); //头像编号索引
                          
                        string name = child.Attributes["name"].Value;

                        ChatListSubItem subitems = new ChatListSubItem(name);
                        subitems.ID = (uint)imageindex; //ID记录头像索引,用于clb_friend_DoubleClickSubItem中构建聊天窗口
                        subitems.HeadImage = imageList1.Images[imageindex]; //HeadImage直接根据imageList1加载头像
                        subitems.Tag = (object)child.Name.Substring(5); //账号,253841
                        clb_friend.Items[0].SubItems.Add(subitems);

                    }
                    //MessageBox.Show("朋友信息载入完成");
                }
                else if (SuperirNode.Name == "groups")
                {
                    clb_group.Items[0].SubItems.Clear();

                    foreach (XmlNode child in SuperirNode.ChildNodes)
                    {
                        //<groupitem num="000001" name="DataBase" header="1"></groupitem>
                       // <groupitem num="000002" name="C#" header="2"></groupitem>
                        string num = child.Attributes["num"].Value;
                        int imageindex = int.Parse(child.Attributes["header"].Value); //头像编号索引

                        string name = child.Attributes["name"].Value;

                        ChatListSubItem subitems = new ChatListSubItem(name);
                        subitems.ID = (uint)imageindex; //ID记录头像索引,用于clb_friend_DoubleClickSubItem中构建聊天窗口
                        subitems.HeadImage = imageList1.Images[imageindex]; //HeadImage直接根据imageList1加载头像
                        subitems.Tag = (object)num; //群号,000001

                        clb_group.Items[0].SubItems.Add(subitems);

                    }
                    
                }

            }
        }





        /* 找不了英文？？？
        /// <summary>
        /// 查找好友
        /// </summary>
        private int searchIndex = 0;
        private ChatListSubItem[] searchItems;
        private string searchText = null;
        private void btn_search_Click(object sender, EventArgs e)
        {
            int searchtype = 0; //0表示找好友，1表示找群组
            string findText = tb_search.Text;
            if (findText != searchText)//搜索内容变化
            {
                searchIndex = 0;
                searchItems = clb_friend.GetSubItemsByText(findText);
                if (searchItems == null) //好友表未找到，尝试找群组
                {
                    searchItems = clb_group.GetSubItemsByText(findText);
                    searchtype = 1;
                }
            }

            if (searchItems != null)
            {
                if (searchIndex < searchItems.Length)
                {
                    switch (searchtype)
                    {
                        case 0: clb_friend.SelectSubItem = searchItems[searchIndex]; break;
                        case 1: clb_group.SelectSubItem = searchItems[searchIndex]; break;
                    }
                    searchIndex++;
                    if (searchIndex == searchItems.Length)
                    {
                        searchIndex = 0;
                    }
                }
            }
            else//没有查找到
            {
                clb_friend.SelectSubItem = null;
                clb_group.SelectSubItem = null;
            }
        }
        */

    }
}
