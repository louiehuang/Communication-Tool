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
using System.Net;


namespace testClient
{
    public partial class FormMain : CCSkinMain
    {
        TcpClient myclient;
        NetworkStream networkStream;
        byte[] buffer; //缓存接收到和要发送的信息
        int size = 8192;

        private SplitMsg MsgHandler;//处理收到的字符
        public delegate void updataMain(string msg);
        public Hashtable UserMsg = new Hashtable();

        static int ballonMsgNum = 0; 
        private delegate void Showicon(int i);
        private bool ShowIcon;
        /// <summary>
        /// 1、2切换，实现托盘气球闪烁
        /// </summary>
        private int IconMode;

        public delegate void PersonalMsgEventHandler(string fromuser, string mymsg);//委托私聊消息发送操作
        public delegate void LoginEventHandler(string msg);//委托登陆操作
        public delegate void GroupMsgEventHandler(string groupnum, string msg);//委托群组消息发送操作

        static AutoResetEvent myResetEvent = new AutoResetEvent(false);

        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动FormMain时与服务器连接进行登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            MsgHandler = new SplitMsg();

            /*与服务器连接*/
            IPAddress Ip = IPAddress.Parse("127.0.0.1");
            int Port = 2333;
            IPEndPoint EndPoint = new IPEndPoint(Ip, Port);
            myclient = new TcpClient();
            myclient.Connect(EndPoint);

            /*获得网络流*/
            networkStream = myclient.GetStream();
            buffer = new byte[size];

            // FormLogin点击登录后myInfo.Login = "[login][account,pwd]", 如[login][10151100,pass]
            string loginRequest = myInfo.Login;

            /*发送信息(登录信息)*/
            SendMsg(loginRequest);

            /* 异步读取服务器反馈信息*/
            lock (networkStream)
            {
                AsyncCallback asy = new AsyncCallback(ReceiveMsg);
                networkStream.BeginRead(buffer, 0, size, asy, null);
            }

        }


        /// <summary>
        /// 每隔50ms向服务器发送客户端产生的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (myInfo.MsgReadyToBeSentToServer != string.Empty)//各窗体中有消息需要发送给服务器
                {
                    string temp = myInfo.MsgReadyToBeSentToServer;//赋给临时变量
                    myInfo.MsgReadyToBeSentToServer = string.Empty;//清空，方便再次有消息传过来

                    SendMsg(temp);//发送消息
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                timer1.Stop();
            }
        }

        /// <summary>
        /// 发送消息，字符串转字节写入网络流
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            msg += "[e]"; //末尾设置分隔符

            byte[] bytes = Encoding.Unicode.GetBytes(msg);
            /*写入网络流，并刷新*/
            networkStream.Write(bytes, 0, bytes.Length);
            networkStream.Flush();
        }

        /// <summary>
        /// 接收服务器的各种请求并作出相应处理
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
                if (readbytes <= 0) //若出现传输字节为0时不做处理
                {
                    //
                }
                else //正常接收信息
                {
                    //将字节数组buffer解码为字符串，并清空buffer以便接收下一条由服务器传来的信息
                    string Readmsg = Encoding.Unicode.GetString(buffer, 0, readbytes);
                    Array.Clear(buffer, 0, buffer.Length);//清空缓存

                    //MessageBox.Show("client: \n" + Readmsg);

                    /*处理所得的Readmsg*/
                    //因离线消息有两个前缀([off],[talk])，所以先判断是否是离线消息 ([off]开头，包含私聊离线消息和群组离线消息)
                    //若是，则将其按群组和私聊分类然后按类别重组
                    if (Readmsg.StartsWith("[off]"))
                    {
                        /*离线消息归类*/
                        /*
                         * [off][talk][101511]Shane 22:59:03:\nhello[e][talk][100001]Alice 22:59:30: Hi[e][talk][101511]Shane 22:59:42: FromShane[e]
                         * 分成
                         * ClassfiedMsgList[0] = "[talk][101511]Shane 22:59:03:\nhello\nShane 22:59:42:\nFromShane\n"
                         * ClassfiedMsgList[1] = "[talk][100001]Alice 22:59:30:\nHi\n"
                         */
                        string[] LeftMsg = new string[100];

                        //归类
                        Regex reg_account = new Regex(@"\d{6}"); //最先找到的6位数即为消息发送者的账号
                        Match m = reg_account.Match(Readmsg);


                        List<string> RawMsgList = new List<string>();
                        string rawmsg = Readmsg.Substring(5); //去掉[off]

                        //MessageBox.Show(rawmsg);
                        int splitindex = rawmsg.IndexOf("[e]");
                        while (splitindex != -1)
                        {
                            RawMsgList.Add(rawmsg.Substring(0, splitindex));
                            //MessageBox.Show(rawmsg.Substring(0,splitindex));
                            rawmsg = rawmsg.Substring(splitindex + 3);
                            splitindex = rawmsg.IndexOf("[e]");
                        }

                        /*
                         RawMsgList[0] = "[talk][101511]Shane 22:59:03:\nhello" 
                         RawMsgList[1] = "[talk][100001]Alice 22:59:30:\nHi" 
                         RawMsgList[2] = "[talk][101511]Shane 22:59:42:\nFromShane"
                         */

                        //按类别重组
                        int index = 0;
                        List<string> ClassfiedMsgList = new List<string>();
                        foreach (string singlemsg in RawMsgList)
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

                                    ClassfiedMsgList[pos] += onemsg;

                                    //MessageBox.Show("found: " + ClassfiedMsg[pos]);

                                }
                            }
                            if (flag == 0)//没有找到
                            {
                                //singlemsg = "[talk][101511]Shane 22:59:03:\nhello" 
                                UserMsg.Add(account, index);
                                string onemsg = singlemsg + "\n";
                                ClassfiedMsgList.Insert(index, onemsg);

                                //MessageBox.Show("First time: " + onemsg);
                                index++;
                            }
                        }

                        foreach (string leftmsg in ClassfiedMsgList)
                        {
                            string fromuser = leftmsg.Substring(7, 6); //发送者id
                            
                            MessageBox.Show(fromuser); //去掉怎么头像显示不正确了，时间问题？
                            
                            string mymsg = leftmsg.Substring(14); //msg
                            //mymsg = "Shane 22:59:03:\nhello\nShane 22:59:42:\nFromShane\n"
                            this.ProcessPersonalMsg(fromuser, mymsg);
                        }


                    } 
                    else //不是离线消息
                    {
                        //按[e]切分消息
                        MsgHandler.MSG = Readmsg;
                        string[] splittedMsg = MsgHandler.process();

                        //依次处理分隔后的每一条的消息
                        if (splittedMsg.Length != 0)
                        {
                            foreach (string msg in splittedMsg)
                            {
                                //MessageBox.Show(msg);
                                if (msg.StartsWith("[login_successful]"))//登陆成功
                                {
                                    /*[login_successful]
                                      * <Info>
                                      *          <myInfo>
                                      *                  <qqnum101511 name="Shane" img="19" sign="西城主唱"></qqnum101511>
                                      *          </myInfo>
                                      *          <friends>
                                      *                  <frienditem account="253841" name="Bryan" img="1"></frienditem>
                                      *                  <frienditem account="100001" name="Alice" img="2"></frienditem>
                                      *          </friend>
                                      *          <groups>
                                      *                  <groupitem num="000001" name="DataBase" header="1"></groupitem>
                                      *                  <groupitem num="000002" name="C#" header="2"></groupitem>
                                      *          </groups>
                                      * </Info>
                                   */

                                    string temp = msg.Substring(18); //去掉前缀

                                    /*更新窗体，载入好友和群组列表**/
                                    LoginEventHandler handlelogin = new LoginEventHandler(LoadMyInfo);
                                    handlelogin.BeginInvoke(temp, null, null);

                                }
                                else if (msg.StartsWith("login_failed")) //登陆失败
                                {
                                    
                                }
                                else if (msg.StartsWith("[talk]")) //私聊会话请求
                                {
                                    //101511发给253841信息:hello，则msg = "[talk][101511]Shane 18:33:59:\nhello"
                                    //此处客户端为253841所打开的客户端
                                    string fromuser = msg.Substring(7, 6); //发送者id
                                    string mymsg = msg.Substring(14); //msg

                                    PersonalMsgEventHandler personalchatHandler = new PersonalMsgEventHandler(ProcessPersonalMsg);
                                    personalchatHandler.BeginInvoke(fromuser, mymsg, null, null);

                                }
                                else if (msg.StartsWith("[group]")) //群组聊天请求
                                {
                                    //MessageBox.Show("1 --- " + msg);
                                    //101511向群组000001发送信息:hello，则msg = "[group][000001]Shane 18:34:09:\hello"
                                    string groupnum = msg.Substring(8, 6);
                                    string groupmsg = msg.Substring(15); //Shane 18:34:09:\hello

                                    GroupMsgEventHandler groupchatHandler = new GroupMsgEventHandler(ProcessGroupMsg);
                                    groupchatHandler.BeginInvoke(groupnum, groupmsg, null, null);

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


        int ballontime = 0; //记录气球消息的次数，第一次的时候除消息外需要加前缀[msg]，姓名和头像索引，之后只加消息本身
        string ballonMsg = string.Empty; //气球消息

        /// <summary>
        /// 处理收到的私聊信息
        /// </summary>
        /// <param name="nums"></param>
        /// <param name="mymsg"></param>
        public void ProcessPersonalMsg(string fromuser, string mymsg)
        {
            // Shane在18:33:59给Bryan发"hello",则Shane向服务器发送msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
            // Bryan收到的msg = "[talk][101511]Shane 18:33:59:\nhello", fromuser="101511", mymsg="Shane 18:33:59:\nhello"
            //MessageBox.Show("fromuser="+fromuser+"msg"+mymsg);

            string fromuser_name = string.Empty; //发送者fromuser的昵称
            int fromuser_header = 0; //发送者fromuser的头像索引
            bool formChatIsOpen = false;//判断和fromuser的聊天窗口是否已打开，若是则更新窗体，否则显示到托盘
            byte[] msgbyte = Encoding.Unicode.GetBytes(mymsg);//转成byte

            //遍历clb_friend，获取fromuser的昵称和头像索引
            for (int i = 0; i < 1; i++) //找clb_friend的第一个组(我的好友)
            {
                for (int j = 0; j < clb_friend.Items[i].SubItems.Count; j++)
                {
                    if (clb_friend.Items[i].SubItems[j].Tag.ToString() == fromuser) //通过发送者账号获得发送者昵称
                    {
                        fromuser_name = clb_friend.Items[i].SubItems[j].NicName;
                        fromuser_header = (int)clb_friend.Items[i].SubItems[j].ID;
                    }
                }
            }

            //查找聊天窗口是否有打开
            foreach (DictionaryEntry chats in myInfo.ChatForm)
            {
                //窗口已打开，则更新窗口
                if (chats.Key.ToString() == fromuser)
                {
                    formChatIsOpen = true;
                    //更新聊天窗口中消息信息
                    ((FormChat)chats.Value).BeginInvoke(new UpdateFormChat(((FormChat)chats.Value).updateFormChat), mymsg);
                }
            }

            //窗口未打开，则显示到托盘
            if (!formChatIsOpen)
            {
                //离线消息: mymsg = "Shane 12:44:50:\nhello\nShane 12:45:08:\nHi[e][talk][100001]\nShane\n12:46:00:\nFromShaneAgain\n"
                //balloon = "[msg]101511,Shane,5,Shane 12:44:50:\nhello\nShane 12:45:08:\nHi[e][talk][100001]\nShane\n12:46:00:\nFromShaneAgain\n"
                
                string balloon = string.Empty; //每次的气球消息

                if (ballontime == 0) //第一次时加前缀，之后只处理消息本身
                {
                    balloon = "[msg]" + fromuser + "," + fromuser_name + "," + fromuser_header.ToString() + "," + mymsg;
                    ballontime = 1; //在显示了聊天窗体后再置0
                }
                else
                {
                    balloon = mymsg;
                    ballontime = 1;
                }

                this.BeginInvoke(new Showicon(startBallonTimer), new object[] { 6 }); //委托，托盘图标闪烁


                updataMain handler = new updataMain(ProcessBallonMsg);
                handler.BeginInvoke(balloon, null, null);
                ballonMsgNum++;

            }
            
        }

        /// <summary>
        /// 处理收到的群组消息
        /// </summary>
        /// <param name="groupnum"></param>
        /// <param name="groupmsg"></param>
        public void ProcessGroupMsg(string groupnum, string groupmsg)
        {
            // Shane在18:33:59给群组1发"hello",则Shane向服务器发送msg = "[talk][101511,000001]Shane 18:33:59:\nhello",
            // Bryan收到的msg = "[group][000001]Shane 18:33:59:\nhello", groupid="000001", groupmsg="Shane 18:33:59:\nhello"

            string group_name = string.Empty;
            int group_header = 0;
            bool groupChatIsOpen = false;//表示是否有该窗体
            byte[] msgbyte = Encoding.Unicode.GetBytes(groupmsg);//转成byte

            //遍历clb_friend，获取groupnum的名称和头像索引
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j <  clb_group.Items[i].SubItems.Count; j++)
                {
                    if (clb_group.Items[i].SubItems[j].Tag.ToString() == groupnum) 
                    {
                        group_name = clb_group.Items[i].SubItems[j].NicName;
                        group_header = (int)clb_group.Items[i].SubItems[j].ID;
                    }
                }
            }

            //查找聊天窗口是否有打开
            foreach (DictionaryEntry groupchats in myInfo.GroupChatForm)
            {
                if (groupchats.Key.ToString() == groupnum)//有该窗体存在
                {
                    //MessageBox.Show("processGroupMsg");
                    groupChatIsOpen = true;
                    //更新聊天窗口中消息信息
                    ((GroupChat)groupchats.Value).BeginInvoke(new UpdateGroupChat(((GroupChat)groupchats.Value).updateGroupChat), groupmsg);
                }
            }

            //窗口未打开，将消息显示到托盘气球
            if (!groupChatIsOpen)
            {
                //balloonMsg =  "[msg]000001,Programming,2,Shane 12:44:50:\nhello\nAlice 12:45:23:\nHi\n"
                //string balloonMsg = "[msg]" + groupnum + "," + group_name + "," + group_header.ToString() + "," + groupmsg;

                string balloon = string.Empty;

                if (ballontime == 0) //第一次时加前缀，之后只处理消息本身
                {
                    balloon = "[msg]" + groupnum + "," + group_name + "," + group_header.ToString() + "," + groupmsg;
                    ballontime = 1;
                }
                else
                {
                    balloon = groupmsg;
                    ballontime = 1;
                }

                this.BeginInvoke(new Showicon(startBallonTimer), new object[] { 6 }); //委托，托盘图标闪烁

                updataMain handler = new updataMain(ProcessBallonMsg);
                handler.BeginInvoke(balloon, null, null);
                ballonMsgNum++;
            }
        
        }


        /// <summary>
        /// 处理托盘气球消息
        /// </summary>
        /// <param name="msg"></param>
        public void ProcessBallonMsg(string msg)
        {
            ballonMsg += msg + '\n';
            myResetEvent.WaitOne(); //P, 阻塞当前线程,直到收到信号(点击托盘图标时发送解锁信号)

            //MessageBox.Show("ballon:\n " + ballonMsg);

            this.BeginInvoke(new updataMain(ShowFormChat), new object[] { ballonMsg });

            ballonMsgNum--; 
        }


        /// <summary>
        /// 点击气球图标，解锁线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if (ballonMsgNum > 0)
            {
                myResetEvent.Set(); // V，解锁当前线程
                if (ballonMsgNum == 1)
                {
                    timer2.Stop();
                }
            }
        }


        /// <summary>
        /// 点击气球后显示聊天界面
        /// </summary>
        /// <param name="msg"></param>
        public void ShowFormChat(string msg)
        {
            //msg = "[msg]101511,Shane,5,Shane 18:33:59:\nhello,hi"
            //msg = [msg]101511,Shane,5,Shane 12:44:50:\nhello\nShane 12:45:08:\nHi[e][talk][100001]\nShane\n12:46:00:\nFromShaneAgain\n

            string[] splitInfo = msg.Substring(5).Split(','); //去掉[msg]后按逗号分隔
            byte[] mymsg = Encoding.Unicode.GetBytes(splitInfo[3]);

            if (int.Parse(splitInfo[0]) >= 100000) //私聊消息
            {
                FormChat mychat = new FormChat(this.pictureBox1.Tag.ToString(), splitInfo[0], label1.Text, splitInfo[1], int.Parse(splitInfo[2]));
                mychat.Show();
                mychat.BeginInvoke(new UpdateFormChat(mychat.updateFormChat), splitInfo[3]);
            }
            else //群组消息
            {
                //[msg]000001,Programming,2,Shane 12:44:50:\nhello\nAlice 12:45:23:\nHi\n
                //获取群组的所有用户
                GroupChat groupchat = new GroupChat(this.pictureBox1.Tag.ToString(), splitInfo[0], label1.Text, splitInfo[1], int.Parse(splitInfo[2]));
                groupchat.Show();
                groupchat.BeginInvoke(new UpdateGroupChat(groupchat.updateGroupChat), splitInfo[3]);
                
            }

            //显示聊天窗体后清空气球消息和重置第一次气球消息标志
            ballonMsg = string.Empty; 
            ballontime = 0;

        }


        /*startBallonTimer, timer2_Tick, ShowIconMode三个方法实现在托盘的图标闪烁*/
        /// <summary>开始timer2，闪烁气球图标</summary>
        /// <param name="i"></param>
        public void startBallonTimer(int i)
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
            // 通过ShowIcon的切换来实现图标(两个，一大一小)闪烁效果
            if (ShowIcon)
            {
                ShowIcon = false;
                this.SwitchIconImage(1);
            }
            else
            {
                ShowIcon = true;
                this.SwitchIconImage(2);
            }

        }

        /// <summary>
        /// 切换气球图像实现闪烁
        /// </summary>
        /// <param name="ShowMode"></param>
        private void SwitchIconImage(int ShowMode)
        {
            switch (ShowMode)
            {
                case 1://消息闪动图像1
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\Message_up.ico");
                    break;
                case 2://消息闪动图像2
                    this.notifyIcon1.Icon = Icon.ExtractAssociatedIcon(Application.StartupPath + "\\myico\\Message_down.ico");
                    break;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Activate();
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
                string myAccount = pictureBox1.Tag.ToString();

                //获取选中好友的名称
                string name = item.NicName;

                //获取选中好友的头像索引
                int img = (int)item.ID; //用ID属性表示头像索引

                //实例化一个聊天界面
                FormChat chat = new FormChat(myAccount, num, label1.Text, name, img);
                chat.Show();
            }
        }

        /// <summary>
        /// 双击群组项时弹出群组聊天窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="es"></param>
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

            bool isOpen = false;
            // 若已经打开了与被选中对象的聊天窗口，则给焦点激活
            foreach (DictionaryEntry mygroupchat in myInfo.GroupChatForm)
            {
                if (mygroupchat.Key.ToString() == num)
                {
                    isOpen = true;
                    ((FormChat)mygroupchat.Value).Activate();
                }
            }

            // 若没有打开过与该对象的聊天窗口，则构造窗口并显示
            if (!isOpen)
            {
                //获取自己的账号id
                string myAccount = pictureBox1.Tag.ToString();

                //获取选中群组的名称
                string name = item.NicName;

                //获取选中群组的头像索引
                int img = (int)item.ID; //用ID属性表示头像索引

                //实例化一个聊天界面
                GroupChat groupchat = new GroupChat(myAccount, num, label1.Text, name, img);
                groupchat.Show();
            }
        }


        /// <summary>
        /// 由xml形式的字符串加载自己的信息和好友资料
        /// </summary>
        /// <param name="msg"></param>
        public void LoadMyInfo(string msg)
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
             * <Info>
             *          <myInfo>
             *                  <me account="101511" name="Shane" header="19" signature="西城主唱"></me>
             *          </myInfo>
             *          <friends>
             *                  <frienditem account="253841" name="Bryan" header="1"></frienditem>
             *                  <frienditem account="100001" name="Alice" header="2"></frienditem>
             *          </friend>
             *          <groups>
             *                  <groupitem num="000001" name="DataBase" header="1"></groupitem>
             *                  <groupitem num="000002" name="C#" header="2"></groupitem>
             *          </groups>
             * </Info>
             */

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg);

            //遍历根节点<Info></Info>的所有子节点
            XmlNodeList xmlnodeList = doc.SelectSingleNode("Info").ChildNodes;
            foreach (XmlNode SuperirNode in xmlnodeList)
            {
                if (SuperirNode.Name == "myInfo") //个人信息
                {
                    foreach (XmlNode child in SuperirNode.ChildNodes)
                    {
                        string myAccount = child.Attributes["account"].Value;
                        label1.Text = child.Attributes["name"].Value; //获取自己的昵称
                        int imageindex = int.Parse(child.Attributes["header"].Value); //获取自己的头像索引
                        label2.Text = child.Attributes["signature"].Value; //获取自己的签名

                        /*置pictureBox的Image自己的头像，Tag为自己的账号:10151100*/
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\image\\" + imageindex.ToString() + ".jpg");
                        pictureBox1.Tag = (object)myAccount; //101511

                    }

                }
               else if (SuperirNode.Name == "friends") //好友信息
                {

                    clb_friend.Items[0].SubItems.Clear();

                    foreach (XmlNode child in SuperirNode.ChildNodes)
                    {
                        //<frienditem account="253841" name="Bryan" img="1"></frienditem> 
                        //<frienditem account="100001" name="Alice" img="2"></frienditem>
                        
                        string account = child.Attributes["account"].Value; //获取好友账号
                        string name = child.Attributes["name"].Value; //获取好友昵称
                        int imageindex = int.Parse(child.Attributes["header"].Value); //获取好友头像编号索引

                        //用name创建好友对象subitems，并将账号和头像加入创建的对象
                        ChatListSubItem subitems = new ChatListSubItem(name);
                        subitems.Tag = (object)account; //账号,253841
                        subitems.HeadImage = imageList1.Images[imageindex]; //HeadImage直接根据imageList1加载头像

                        subitems.ID = (uint)imageindex; //ID记录头像索引,用于clb_friend_DoubleClickSubItem中构建聊天窗口
                       
                        clb_friend.Items[0].SubItems.Add(subitems);

                    }

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
