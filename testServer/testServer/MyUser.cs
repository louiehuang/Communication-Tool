using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace testServer
{
    class MyUser
    {
        public delegate void handlefortalk(string num1, string num2, string msg);//处理A发送B msg
        public delegate void handle(string num1, string num2);//委托，用于异步调用处理操作db和转发
        public static Hashtable AllOnlineUser = new Hashtable();//存储在线用户的号码和对应的实例
        private string MyFriendList = "";//我的好友
        private string MyAllOnlineFriends = "";//当前用户的在线好友名单，之间用逗号分开
        private string MyQQnum = "";

        private ManageUser ManageNum;
        private BuildXml build;//构建好友、陌生人、黑名单XML格式字符串
        TcpClient user;
        NetworkStream networkStream;
        private HandleMsg MsgHandler;

        byte[] buffer;
        public const int size = 8192;

        public MyUser(TcpClient client)
        {
            user = client;
            buffer = new byte[size];
            MsgHandler = new HandleMsg();
            networkStream = user.GetStream();

            lock (networkStream)
            {
                AsyncCallback asyncCallback = new AsyncCallback(ReceiveMsg);
                networkStream.BeginRead(buffer, 0, size, asyncCallback, null);
            }
        }

        public void ReceiveMsg(IAsyncResult result)//接收消息，并作出反应
        {
            int readsize;
            try
            {
                lock (networkStream)
                {
                    readsize = networkStream.EndRead(result);
                }

                if (readsize < 1)
                {
                    MessageBox.Show("ERROR: 收到的<1");
                }
                else
                {
                    string Remsg = Encoding.Unicode.GetString(buffer, 0, readsize);
                    Array.Clear(buffer, 0, buffer.Length);//清空缓存
                    MsgHandler.MSG = Remsg; //class HandleMsg
                    string[] AllMsg = MsgHandler.process();


                    if (AllMsg.Length != 0)
                    {
                        foreach (string msg in AllMsg)
                        {
                            //MessageBox.Show(msg);
                            if (msg.StartsWith("[login]"))//登陆
                            {
                                //eg:[login][101511,1111]
                                //获取QQ号码和密码
                                // NumPsw[] = {101511,1111}
                                string[] NumPsw = msg.Substring(8,   msg.LastIndexOf(']') - 8).Split(',');
                                handle handlelogin = new handle(StartLogin);
                                handlelogin.BeginInvoke(NumPsw[0], NumPsw[1], null, null);
                            }
                            else if (msg.StartsWith("[talk]"))//聊天
                            {
                                // Shane在18:33:59给Bryan发消息"hello"
                                // 则Shane向服务器发送 msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
                                // 服务器向Bryan发送 msg = "[talk][101511]Shane 18:33:59:\nhello"，并写入数据库
                                // 若Bryan不在线，则再写入离线消息[talk][101511]Shane 18:33:59:\nhello[e]
                                // Nums[0] = "101511", Nums[1] = "253841", msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
                                string[] Nums = msg.Substring(7, msg.IndexOf(']', 7) - 7).Split(',');
                                handlefortalk handletalk = new handlefortalk(StartTalk);
                                handletalk.BeginInvoke(Nums[0], Nums[1], msg, null, null);
                            }
                        }
                    }

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
                string tempall = this.GetAllnumsOnline();//获得在线的好友陌生人和黑名单的xml格式
                string[] temppele = this.build.GetAllNums(tempall).Split(',');//从XML格式中提取所有QQ号码，用“，”分开
                this.SendMsgToAllPeople("[down][" + this.MyQQnum + "][e]", temppele);//告诉有关的所有人QQnum下线了
                ManageUser down = new ManageUser();
                down.DownLine(MyQQnum);
                AllOnlineUser.Remove(this.MyQQnum);//从在线名单中删除这个对象实例
            }
        }

        public void StartTalk(string num1, string num2, string msg)//num1发送给num2 msg
        {
            /* 发送形式[talk][num1]msg
             * num1 = 101511, num2 = 253841
             * msg = "[talk][101511,253841]Shane 18:33:59:\nhello",
             * time = "18:33:59",
             * realmsg =  "hello",
             */
            //发给好友num2的消息格式
            //tofriendmsg = "[talk][101511]Shane 18:33:59:\nhello"

            /*SendMsgToSomebody方法：若num2在线则将消息发过去，不在线则存入Users表中num2的offlinemsg*/
            string tofriendmsg = "[talk][" + num1 + "]" + msg.Substring(7 + num1.Length + 1 + num2.Length + 1);
            this.SendMsgToSomebody(tofriendmsg + "[e]", num2);

            /*
            string str = "Shanesf 11:05:39:\n hello";
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}");
            Match m = reg.Match(str);
            while (m.Success)
            {
                MessageBox.Show(m.Result("$0"));
                m = m.NextMatch();
            }
             */

            /*正则表达式获取字符串内时间*/
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}");
            Match m = reg.Match(msg);
            string time = m.Result("$0");

            /*获取msg内的实际消息hello*/
            int index = msg.IndexOf("\n"); //第一个\n后实际消息开始
            string realmsg = msg.Substring(index); //realmsg = "hello";

            /*存入数据库*/
            SavePersonalMsg(num1, num2, time, realmsg);
        }


        /// <summary>
        /// 消息存入数据库的PersonalMsg表
        /// </summary>
        public void SavePersonalMsg(string num1, string num2, string time, string realmsg)
        {
            mydb db = new mydb();
            //bool result = false;
            /*
            string sql = "insert into PersonalMsg (PM_fromuser,PM_touser,PM_time,PM_content) values('" +
                num1 + "','" + num2 + "','" + time + "','" + realmsg + "')";
             */
            string sql = string.Format("INSERT INTO PersonalMsg (PM_fromuser,PM_touser,PM_time,PM_content) values ('{0}','{1}','{2}','{3}')",
                                num1, num2, time, realmsg);

            int i = db.QueryReturn(sql); //i=1则插入正常

        }




        public void StartLogin(string num, string psw)//登陆判断
        {
            UserLogin login = new UserLogin(num, psw);

            if (login.Check())//登陆成功
            {
                AllOnlineUser.Add(num, this); //当前用户加入在线用户表
                MyQQnum = num; //此线程用户的账号为登陆请求传来的num
                ManageNum = new ManageUser(num);

                build = new BuildXml(num);

                //发送好友名单
                string tempall = GetAllnumsOnline();
                string tempmsg = "[welcome]" + tempall;

                this.SendMsgToSomebody(tempmsg + "[e]", num); //发送反馈登陆成功和好友信息给此线程的登录用户

                /*
                //还要向他的在线好友发送他已经上线了！
                //eg[online][101511][e] 表示101511(Shane)上线了
                string broadcast = "[online][" + num + "][e]";

                MyAllOnlineFriends = build.GetAllNums(tempall);//获得当前用户的在线好友名单，用逗号分开的
                string[] TotellPeople = MyAllOnlineFriends.Split(',');

                SendMsgToAllPeople(broadcast, TotellPeople);
                */




                // 获取当前用户的离线消息并发回
                string LeftMsg = ManageNum.GetLeftMsg();//获得用户未的离线消息
                //LeftMsg = "[talk][101511]Shane 18:33:59:\nhello[e][off][talk][101511]Shane 19:33:59:\nHi[e]"

                if (LeftMsg != string.Empty)//如果未收到的消息不为空
                {
                    //string SendLeftMsg = "[talk][" + MyQQnum + "]" + LeftMsg;
                    this.SendMsgToSomebody(LeftMsg, num);//发送未收到的信息
                    //MessageBox.Show(LeftMsg);
                    ManageNum.ClearLeftMsg();//清空未收到的信息
                }
                //MessageBox.Show("Login Succeed");



            }
            else//登陆不成功，反馈登陆失败信息
            {
                //MessageBox.Show("Login Failed");
                AllOnlineUser.Add("guest", this);
                string tempmsg = "[loginfailed][e]";
                this.SendMsgToSomebody(tempmsg, "guest");
                AllOnlineUser.Remove("guest");
                return;
            }
        }

        public string GetAllnumsOnline()//把好友组合成一个xml格式
        {
            this.MyFriendList = build.BuildFriend();
            string result = "<all>" + this.ManageNum.GetSimpleDate() + this.MyFriendList + "</all>";

            //MessageBox.Show(result);

            /*
             * 例如账号101511请求登录，其由一个好友253841，则返回信息如下
             * <all><MySimple><qqnum101511 name="Shane" img="19" sign="签名测试"></qqnum101511></MySimple>
             * <friends><qqnum253841 name="Bryan" img="1"  ison="1"></qqnum253841></friend></all>
             */
            return result;
        }

        public string GetLeftMsgInXml()
        {

            string result = "<leftmsg>" + this.ManageNum.GetLeftMsg() + "/leftmsg>";
            return result;
        
        }

        public void SendMsgToSomebody(string msg, string num) //把信息发送给num
        {
            //(1)发给自己(num=01511) msg = [welcome]<all><MySimple><qqnum101511 name="Shane" img="19" sign="签名测试"></qqnum101511></MySimple>
            //<friends><qqnum253841 name="Bryan" img="1"  ison="1"></qqnum253841></friend></all>

            //(2)发给好友(num=253841), msg = "[talk][101511]Shane 18:33:59:\nhell"
            bool issend = false;
            foreach (DictionaryEntry QQnum in AllOnlineUser)
            {
                //MessageBox.Show(QQnum.Key.ToString());
                if (QQnum.Key.ToString() == num) //对方在线
                {
                    issend = true;
                    ((MyUser)QQnum.Value).SendMsg(msg);  //直接SendMsg(msg)????????
                }
            }
            if (issend == false)//如果num不在线,把消息写入Users表，以便num再次上线时发给他
            {
                //msg = tofriendmsg = "[talk][101511]Shane 18:33:59:\nhello[e]"
                string leftmsg = msg;

                //string leftmsg = msg.Substring(14, msg.Length - 14 - 3) + "\n"; //Shane 18:33:59:\nhello\n
                //msg += "[e]";//因为可能有多条留言，所以用[e]分开，前面调用已经加过[e]了
                ManageUser templeft = new ManageUser();
                templeft.LeftMsg(leftmsg, num);

                //string send = msg.Substring(0, msg.Length - 3);
                //templeft.SaveToOfflineMsg(send, num);
            }
        }

        public void SendMsgToSomebodyNoSave(byte[] msg, string num)//直接发送，如果对方不在线不保存数据库
        {
            foreach (DictionaryEntry QQnum in AllOnlineUser)
            {
                if (QQnum.Key.ToString() == num)
                {
                    ((MyUser)QQnum.Value).SendMsg(msg);
                }
            }
        }


        public void SendMsg(string msg)//发送消息，字符串转字节写入网络流
        {
            byte[] temp = Encoding.Unicode.GetBytes(msg);
            networkStream.Write(temp, 0, temp.Length);
            networkStream.Flush();
        }

        public void SendMsg(byte[] msg)//发送消息，字节数组直接写入网络流
        {
            networkStream.Write(msg, 0, msg.Length);
            networkStream.Flush();
        }

        public void SendMsgToAllPeople(string msg, string[] nums)//只负责发送消息通知某人上线或离线
        {
            foreach (string people in nums)
            {
                foreach (DictionaryEntry QQnum in AllOnlineUser)
                {
                    if ((QQnum.Key).ToString() == people)
                    {
                        ((MyUser)QQnum.Value).SendMsg(msg);
                    }
                }
            }
        }



    }
}
