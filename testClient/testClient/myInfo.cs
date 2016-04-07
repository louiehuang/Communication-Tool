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
        public static string Login = null; //是否已登录
        public static string ChildFromMsg = null; //子窗体信息
        public static byte[] Talk;
        public static bool picture = false; //聊天发送框是否有图
        public static Hashtable ChatForm = new Hashtable(); //用户打开的多个聊天界面, FormChar构造函数中加入
        public static Hashtable UserDate = new Hashtable();
        //public static List<FindUser> FindForm = new List<FindUser>();
    }
}
