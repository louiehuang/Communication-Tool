using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace testServer
{
    class ManageUser
    {
        mydb db;
        private string QQnum;
        /// <summary>
        /// 构造函数,num作为参数
        /// </summary>
        /// <param name="num"></param>
        public ManageUser(string num)
        {
            QQnum = num;
            db = new mydb();
        }
        public ManageUser()
        {
            db = new mydb();
        }
        /// <summary>
        /// 从表中添加号码为num的好友
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool AddFriendFromTable(string num)
        {
            bool result = false;
            string sql = "insert into tb_friend (friend_somebody,friend_friends) values('" + QQnum + "','" + num + "')";
            int i = db.QueryReturn(sql);
            if (i == 1)
                result = true;
            return result;
        }

        /// <summary>
        /// 返回简单信息  QQ帐户 昵称 头像
        /// </summary>
        /// <returns></returns>
        public string GetSimpleDate()
        {
            string sql = "select U_account as QQ帐号,U_name as 昵称,U_header as 头像,U_signature as 个性签名 from Users where U_account='" + QQnum + "'";
            DataSet ds = new DataSet();
            ds = db.Query(sql);
            DataRow row = ds.Tables[0].Rows[0];
            string result = string.Format("<MySimple><qqnum{0} name=\"{1}\" img=\"{2}\" sign=\"{3}\"></qqnum{4}></MySimple>", row["QQ帐号"].ToString(), row["昵称"].ToString(), row["头像"].ToString(), row["个性签名"].ToString(), row["QQ帐号"].ToString());
            return result;
        }


        /// <summary>
        /// 如果somebody不在线，则把msg保留到数据库
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="somebody">somebody</param>
        public void LeftMsg(string msg, string somebody)
        {
            //msg = "[talk][101511]Shane 18:33:59:\nhell[e]"
            //somebody = "253841"
            //先获取253841的离线消息，再将本次消息msg加在末尾，重新存入
            ManageUser temp = new ManageUser(somebody);
            string myleft = temp.GetLeftMsg(); //temp.QQnum = 253841
            if (myleft != string.Empty)
                myleft += msg;
            else
                myleft = msg;
            string sql = "update Users set U_offlinemsg='" + myleft + "' where U_account='" + somebody + "'";
            db.QueryNoReturn(sql);
        }


        public void SaveToOfflineMsg(string msg, string tonum)
        {
            //msg = "[talk][101511]Shane 18:33:59:\nhell[e]"
            /*正则表达式获取字符串内时间*/
            Regex reg = new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}");
            Match m = reg.Match(msg);
            string time = m.Result("$0");

            /*获取msg内的实际消息hello*/
            int index = msg.IndexOf("\n"); //第一个\n后实际消息开始
            //string realmsg = msg.Substring(index); //realmsg = "hello";
            string content = msg.Substring(14);

            string sql = "insert into OfflineMsg (userid,content) values('" + tonum + "','" + content + "')";


        }


        /// <summary>
        /// 获得用户未收到的信息
        /// </summary>
        /// <returns></returns>
        public string GetLeftMsg()
        {
            string getmsg = "select U_offlinemsg from Users where U_account='" + QQnum + "'";
            DataSet ds = new DataSet();
            ds = db.Query(getmsg);
            string leftmsg = ds.Tables[0].Rows[0][0].ToString().Trim();
            return leftmsg;
        }

        /// <summary>
        /// 清空之前为收到现在已经收到的消息
        /// </summary>
        public void ClearLeftMsg()
        {
            string clearmsg = "update Users set U_offlinemsg='[off]' where U_account='" + QQnum + "'";
            string clearofflinemsg = "delete from OfflineMsg where Userid='" + QQnum + "'";
            db.QueryNoReturn(clearmsg);
            db.QueryNoReturn(clearofflinemsg);
        }

        public void DownLine(string QQnum) //QQnum下线
        {
            string downline = "update Users set U_status=0 where U_account='" + QQnum + "'";
            db.QueryNoReturn(downline);
        }

        /// <summary>
        /// 检查用户帐户和密码是否一致,用户修改密码
        /// </summary>
        /// <returns>一致返回true，否则返回false</returns>
        public bool Check(string num, string psw)
        {
            string sql = "select count(*) from tb_Users where Users_num='" + num
                 + "' and Users_psw='" + psw + "'";
            bool issuccess = false;
            int i = db.QueryLogin(sql);
            if (i != -1)
                issuccess = true;
            return issuccess;
        }

        /// <summary>
        /// 获取用户昵称
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            string sql = "select Users_name from tb_Users where Users_num='" + QQnum + "'";
            DataSet ds = new DataSet();
            ds = db.Query(sql);
            string name = ds.Tables[0].Rows[0][0].ToString().Trim();
            return name;
        }

    }
}
