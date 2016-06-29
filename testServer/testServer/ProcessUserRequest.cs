using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace testServer
{
    class ProcessUserRequest
    {
        private string MyAccount = ""; //当前用户的账号 101511
        private string MyFriendList = "";//我的好友，xml形式的字符串，方便客户端解析
        private string MyGroupList = "";//我的群组，xml形式的字符串
        private UserInfoXml build;//构建好友、陌生人、黑名单XML格式字符串

        BLL_Users BLL_user = new BLL_Users();
        BLL_Groups BLL_group = new BLL_Groups();

        public delegate void PersonalMsgEventHandler(string fromuser, string touser, string msg);//委托私聊消息发送操作
        public delegate void LoginEventHandler(string num1, string num2);//委托登陆操作
        public delegate void GroupMsgEventHandler(string fromuser, string groupnum, string msg);//委托群组消息发送操作
        public delegate void RegisterEventHandler(string msg); //委托注册操作

        public static Hashtable OnlineUserTable = new Hashtable();//在线用户表,记录在线用户的账号(键)及其对象(值)

        //private string MyAllOnlineFriends = "";//当前用户的在线好友名单，之间用逗号分开


        TcpClient user;
        NetworkStream networkStream;

        private SplitMsg MsgHandler; //处理接收到的消息，按[e]分隔成多条

        byte[] buffer; //缓冲字节数组，接收和发送信息的缓冲区
        public const int size = 8192;

        //构造函数
        public ProcessUserRequest(TcpClient client)
        {
            user = client;
            buffer = new byte[size];
            MsgHandler = new SplitMsg();

            //获取网络流
            networkStream = user.GetStream();

            //回调ReceiveMsg
            lock (networkStream)
            {
                AsyncCallback asyncCallback = new AsyncCallback(ReceiveMsg);
                networkStream.BeginRead(buffer, 0, size, asyncCallback, null);
            }
        }

        /// <summary>
        /// 接收客户端的各种请求并作出相应处理
        /// </summary>
        /// <param name="result"></param>
        public void ReceiveMsg(IAsyncResult result)
        {
            int msgLength = 0;
            try
            {
                lock (networkStream)
                {
                    msgLength = networkStream.EndRead(result);
                }

                if (msgLength <= 0) //若出现传输字节为0时不做处理
                {
                    //MessageBox.Show("msgLength is less than 0");
                }
                else
                {
                    //将字节数组buffer解码为字符串，并清空buffer以便接收下一条由客户端传来的信息
                    string rawMsg = Encoding.Unicode.GetString(buffer, 0, msgLength);
                    Array.Clear(buffer, 0, buffer.Length);

                    //处理接收到的消息，按[e]分隔符分成多条
                    MsgHandler.MSG = rawMsg;
                    string[] splittedMsg = MsgHandler.process();

                    //依次处理分隔后的每一条的消息
                    if (splittedMsg.Length != 0)
                    {
                        foreach (string msg in splittedMsg)
                        {
                            //MessageBox.Show("Server: " + msg);

                            if (msg.StartsWith("[login]"))//登陆请求
                            {
                                //eg:[login][101511,1111]
                                //获取请求登陆用户的账号和密码
                                string account = msg.Substring(8, 6); //101511
                                string pwd = msg.Substring(15, msg.Length - 16); //1111

                                LoginEventHandler handlelogin = new LoginEventHandler(StartLogin);
                                handlelogin.BeginInvoke(account, pwd, null, null);

                            }
                            else if (msg.StartsWith("[talk]"))//私聊请求
                            {
                                // Shane在18:33:59给Bryan发消息"hello"
                                // 则服务器收到 msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
                                // 服务器向Bryan发送 msg = "[talk][101511]Shane 18:33:59:\nhello"，并写入数据库的PersonalMsg表
                                // 若Bryan不在线，则再写入Users表中Bryan的离线消息offlinemsg, [talk][101511]Shane 18:33:59:\nhello[e]

                                // fromuser = "101511", touser = "253841", msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
                                string fromuser = msg.Substring(7, 6); //101511
                                string touser = msg.Substring(14, 6); //253841

                                PersonalMsgEventHandler personalchatHandler = new PersonalMsgEventHandler(StartPersonalChat);
                                personalchatHandler.BeginInvoke(fromuser, touser, msg, null, null);

                            }
                            else if (msg.StartsWith("[group]")) //群聊请求
                            {
                                //Shane向群组000001发送消息"hello"
                                //则服务器收到msg = "[group][101511,000001]Shane 18:29:39:\nhello",
                                //查询群组000001中的所有用户，向这些用户发送sendmsg = "[group][000001]Shane 18:29:39:\nhello"

                                //获取群号
                                string fromuser = msg.Substring(8, 6); //101511
                                string groupid = msg.Substring(15, 6);//000001
                                string sendmsg = msg.Substring(0, 8) + msg.Substring(15);
                                //MessageBox.Show(sendmsg);

                                GroupMsgEventHandler groupchatHandler = new GroupMsgEventHandler(StartGroupChat);
                                groupchatHandler.BeginInvoke(fromuser, groupid, sendmsg, null, null);

                            }
                            else if (msg.StartsWith("[register]")) //注册账号请求
                            {
                                /*
                                     [register]
                                     <Register>
                                         <Info name="Shane" pwd="1111" gender="男" sign="西城主唱" age="21" constellation="射手座" blood="A型"></Info>
                                     </Register> 
                                 */

                                string temp = msg.Substring(10); //去掉前缀
                                //MessageBox.Show(temp);

                                /*更新窗体，载入好友和群组列表**/
                                RegisterEventHandler handleRegister = new RegisterEventHandler(LoadMyInfo);
                                handleRegister.BeginInvoke(temp, null, null);



                            }
                        }//foreach
                    }//if

                }
                lock (networkStream)
                {
                    AsyncCallback asy = new AsyncCallback(ReceiveMsg);
                    networkStream.BeginRead(buffer, 0, size, asy, null);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("异常——" + ex.Message);

                OnlineUserTable.Remove(this.MyAccount);//在线用户表中删除当前用户
            }
        }

        public void StartGroupChat(string fromuser, string groupnum, string msg)
        {
            //msg = "[group][000001]Shane 18:29:39:\nhello"
            //通过数据库Belongs表查询groupid的所有用户
            string[] groupuser = BLL_group.BLL_Groups_GetUserInGroup(groupnum);

            //对groupuser中的每个用户进行判断，若其在线，则将消息发送
            //若不在线，则将消息存入其在Users表中的offlinemsg字段
            //群组离线消息暂未实现
            foreach (string eachuser in groupuser) //101511 253841 100001
            {
                //GetUserInGroup返回了有200个单元的string数组groupuser，多余的单元为空值
                //因此读到空时说明已经groupuser中的所有用户发完群消息，即应退出循环
                if (eachuser == string.Empty || eachuser == null)
                    break;
                //MessageBox.Show("people in group: " + people); //101511 253841 100001
                foreach (DictionaryEntry onlineuser in OnlineUserTable) //101511,253841
                {
                    //MessageBox.Show("QQnum: " + QQnum.Key.ToString());
                    if ((onlineuser.Key).ToString() == eachuser && (onlineuser.Key).ToString() != MyAccount) //给除了自己的发
                    {
                        //MessageBox.Show("msg: " + msg);
                        //加分隔符[e]发送
                        ((ProcessUserRequest)onlineuser.Value).SendMsg(msg + "[e]");
                    }
                }
            }

            /*正则表达式获取字符串内时间，第一个匹配到的时间是客户端程序设置的发送时间*/
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}");
            Match m = reg.Match(msg);
            string time = m.Result("$0");

            /*获取msg内的实际消息hello*/
            int index = msg.IndexOf("\n"); //第一个\n后实际消息开始
            string realmsg = msg.Substring(index); //realmsg = "hello";

            /*存入数据库GroupMsg表*/
            //101511, 000001, 18:33:59, hello
            //SaveGrouplMsg(fromuser, groupnum, time, realmsg);
            //MessageBox.Show(fromuser + "," + groupnum + ", " + time + ", " + realmsg);
            BLL_group.BLL_Groups_SaveGroupMsg(fromuser, groupnum, time, realmsg);

        }



        /// <summary>
        /// 处理[talk]私聊请求
        /// </summary>
        /// <param name="fromuser"></param>
        /// <param name="touser"></param>
        /// <param name="msg"></param>
        public void StartPersonalChat(string fromuser, string touser, string msg)//fromuser发送给touser msg
        {
            /* 发送形式[talk][fromuser]msg
             * fromuser = 101511, touser = 253841
             * msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
             * time = "18:33:59",
             * realmsg =  "hello",
             */
            //发给好友num2的消息格式
            //tofriendmsg = "[talk][101511]Shane 18:33:59:\nhello"

            /*SendMsgToCertainUser方法：若touser在线则将消息发过去，不在线则存入Users表中touser的offlinemsg*/
            string tofriendmsg = "[talk][" + fromuser + "]" + msg.Substring(9 + fromuser.Length + touser.Length);
            //加分隔符[e]发送
            this.SendMsgToCertainUser(tofriendmsg + "[e]", touser);

            /*
            string str = "Shane 11:05:39:\n hello";
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}");
            Match m = reg.Match(str);
            while (m.Success)
            {
                MessageBox.Show(m.Result("$0"));
                m = m.NextMatch();
            }
             */

            /*正则表达式获取字符串内时间，第一个匹配到的时间是客户端程序设置的发送时间*/
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}");
            Match m = reg.Match(msg);
            string time = m.Result("$0");

            /*获取msg内的实际消息hello*/
            int index = msg.IndexOf("\n"); //第一个\n后实际消息开始
            string realmsg = msg.Substring(index); //realmsg = "hello";

            /*存入数据库PersonalMsg表*/
            //101511, 253841, 18:33:59, hello
            //SavePersonalMsg(fromuser, touser, time, realmsg);
            BLL_user.BLL_Users_SavePersonalMsg(fromuser, touser, time, realmsg);

        }


        /// <summary>
        /// 处理[login]登陆请求
        /// </summary>
        /// <param name="num"></param>
        /// <param name="psw"></param>
        public void StartLogin(string account, string pwd)//登陆判断
        {
            //查询用户登录请求是否被允许，若允许则继续处理
            if (BLL_user.BLL_Users_LoginPermission(account, pwd) == true)
            {
                OnlineUserTable.Add(account, this); //当前用户加入在线用户表

                MyAccount = account; //此线程用户的账号为登陆请求传来的account

                //创建要传回客户端的xml字符串(包括个人信息、好友信息和群组信息)
                build = new UserInfoXml(account);

                /*获取当前用户的所有信息(个人信息、好友信息和群组信息)并发回*/
                string Info = GetAllInfoOfCurrentUser();

                this.SendMsgToCertainUser("[login_successful]" + Info + "[e]", account); //发送反馈登陆成功和好友信息给此线程的登录用户

                /*
                 * 当前线程休眠一段时间！！！
                 * 否则可能用户登录的反馈信息[welcome](xml)和离线消息LeftMsg被同时发送到客户端
                 * 即客户端接收到的信息时两个字符串的和，则在解析xml时因LeftMsg格式不同而会出错
                 */
                Thread.Sleep(80);
                /*或者可以在客户端解析进行处理，发送的[login_successful]...[off][talk]...
                   按[e]分隔的第一句是xml的[login_successful]，后面是离线消息，两种类型分开处理
                */


                /* 获取当前用户的离线消息并发回 */
                string OfflineMsg = BLL_user.BLL_Users_GetOfflineMsg(account); //获得用户未的离线消息

                //OfflineMsg = "[off][talk][101511]Shane 18:33:59:\nhello[e][talk][101511]Shane 19:33:59:\nHi[e]"

                //MessageBox.Show("server: \n" + OfflineMsg);
                if (OfflineMsg.Length > 5)//如果未收到的消息不为空，因为开头是[off]，所以长度要大于5才是消息不空
                {
                    this.SendMsgToCertainUser(OfflineMsg, account);//发送未收到的信息
                    //MessageBox.Show(OfflineMsg);
                    BLL_user.BLL_Users_ClearOfflineMsg(account);

                }
                //MessageBox.Show("Login Succeed");

            }
            else//登陆不成功，反馈登陆失败信息
            {
                //MessageBox.Show("Login Failed");
                //OnlineUserTable.Add("guest", this);
                this.SendMsgToCertainUser("[login_failed][e]", account);
                //OnlineUserTable.Remove("guest");
                return;
            }
        }

        /// <summary>
        /// 获取当前用户的所有信息(个人信息、好友信息和群组信息)
        /// </summary>
        /// <returns></returns>
        public string GetAllInfoOfCurrentUser()
        {
            string MyInfo = build.GetMyBasicInfo(); //获取当前用户的个人基本信息，xml形式字符串
            MyFriendList = build.GetUserFriendsXml(); //获取当前用户的好友表，xml形式字符串
            MyGroupList = build.GetUserGroupsXml();  //获取当前用户的群组表，xml形式字符串
            string result = "<Info>" + MyInfo + MyFriendList + MyGroupList + "</Info>";

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

            return result;

        }

        /// <summary>
        /// 发送消息，字符串转字节写入网络流
        /// </summary>
        /// <param name="msg"></param>
        public void SendMsg(string msg)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(msg);
            networkStream.Write(bytes, 0, bytes.Length);
            networkStream.Flush();
        }

        public void SendMsgToCertainUser(string msg, string account) //把信息发送给num
        {
            //(1)发给自己(num=01511) msg = [welcome]<all><MySimple><qqnum101511 name="Shane" img="19" sign="签名测试"></qqnum101511></MySimple>
            //<friends><qqnum253841 name="Bryan" img="1"  ison="1"></qqnum253841></friend></all>

            //(2)发给好友(num=253841), msg = "[talk][101511]Shane 18:33:59:\nhell"
            bool flag = false; //判断是否发送成功(对方在线则发送成功)

            foreach (DictionaryEntry user in OnlineUserTable)
            {
                if (user.Key.ToString() == account) //对方在线
                {
                    flag = true;
                    //创建一个新的用户请求处理类发送消息
                    ((ProcessUserRequest)user.Value).SendMsg(msg);  //直接SendMsg(msg)出错????????
                }
            }

            if (!flag)//account不在线,则把消息写入Users表offlinemsg
            {
                //msg = tofriendmsg = "[talk][101511]Shane 18:33:59:\nhello[e]"
                string offlineMsg = msg;

                //string leftmsg = msg.Substring(14, msg.Length - 14 - 3) + "\n"; //Shane 18:33:59:\nhello\n
                //msg += "[e]";//因为可能有多条留言，所以用[e]分开，前面调用已经加过[e]了

                BLL_user.BLL_Users_saveMsgToOfflinemsg(offlineMsg, account);

            }
        }

        /// <summary>
        /// 给所有用户发送消息(群组消息和通知当前用户好友该用户上线或离线)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="nums"></param>
        public void SendMsgToAllUsers(string msg, string[] nums)
        {
            foreach (string people in nums)
            {
                foreach (DictionaryEntry user in OnlineUserTable)
                {
                    if ((user.Key).ToString() == people)
                    {
                        ((ProcessUserRequest)user.Value).SendMsg(msg);
                    }
                }
            }
        }

        public void LoadMyInfo(string msg)
        {
            /*
                <Register>
                    <Info name="Shane" pwd="1111" header=\"1\" gender="男" sign="西城主唱" age="21" constellation="12" blood="1"></Info>
                </Register> 
            */

            //MessageBox.Show("Load: " + msg);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg);
            string account = string.Empty;
            string name = string.Empty;
            string pwd = string.Empty;
            int header = 1;
            string gender = string.Empty;
            string sign = string.Empty;
            int age = 0;
            int constellation = 0;
            int blood = 0;

            Random random = new Random();
            account = random.Next(100000,1000000).ToString(); //随机生成6位数的账号

            //遍历根节点<Info></Info>的所有子节点
            XmlNodeList xmlnodeList = doc.SelectSingleNode("Register").ChildNodes;
            foreach (XmlNode child in xmlnodeList)
            {
                if (child.Name == "Info") //个人信息
                {
                        name = child.Attributes["name"].Value;
                        pwd = child.Attributes["pwd"].Value;
                        header = Convert.ToInt32(child.Attributes["header"].Value);
                        gender = child.Attributes["gender"].Value;
                        sign = child.Attributes["sign"].Value;
                        age = Convert.ToInt32(child.Attributes["age"].Value);
                        constellation =  Convert.ToInt32(child.Attributes["constellation"].Value);
                        blood =  Convert.ToInt32(child.Attributes["blood"].Value);
                }

                MessageBox.Show(name + " " + pwd + " " + header + " " + gender + " " + sign + " " + age + " " + constellation + " " + blood);

            }

            bool flag = BLL_user.BLL_Users_Register(account, name, pwd, header, gender, sign, age, constellation, blood);

            if (flag == true)
            {
                MessageBox.Show("account: " + account);
                //发送回账号
            }
            else
            { 
                //发送回注册失败
            }

        }



    }
}
