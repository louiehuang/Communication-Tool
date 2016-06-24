using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace testClient
{
    /*myInfo用于记录客户端各种操作所得到的数据，由timer1不断将此处数据发往服务器*/
    class myInfo
    {

        public static string Login = null; //记录用户的登录信息，myInfo.Login = "[login][account,pwd]", 如[login][10151100,1111]
        public static string MsgReadyToBeSentToServer = null; //各窗体准备好发给服务器的信息

        public static Hashtable ChatForm = new Hashtable(); //用户打开的多个聊天界面, FormChat构造函数中加入
        public static Hashtable GroupChatForm = new Hashtable(); //用户打开的多个群组聊天界面, GroupChat构造函数中加入

    }
}
