using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace testServer
{
    class HandleUserEvent
    {
        mydb db;
        private string myAccount;
        /// <summary>
        /// 构造函数,num作为参数
        /// </summary>
        /// <param name="num"></param>
        public HandleUserEvent(string num)
        {
            myAccount = num;
            db = new mydb();
        }

        public HandleUserEvent()
        {
            db = new mydb();
        }

        ///// <summary>
        ///// 从表中添加号码为num的好友
        ///// </summary>
        ///// <param name="num"></param>
        ///// <returns></returns>
        //public bool AddFriendFromTable(string num)
        //{
        //    bool result = false;
        //    string sql = "insert into tb_friend (friend_somebody,friend_friends) values('" + myAccount + "','" + num + "')";
        //    int i = db.QueryReturn(sql);
        //    if (i == 1)
        //        result = true;
        //    return result;
        //}


        /// <summary>
        /// 获取某一群组中的用户账号
        /// </summary>
        /// <param name="groupnum"></param>
        /// <returns></returns>
        public string[] GetUserInGroup(string groupnum)
        {
            string[] result = new string[200]; //假设群组上限200人
            string sql = "select userid from Belongs WHERE groupnum = '" + groupnum + "'";
            DataSet ds = new DataSet();
            ds = db.Query(sql);

            int i = 0;
            foreach (DataRow row in ds.Tables[0].Rows) //查询出来的第一个表Tables[0]
            {
                result[i] = row["userid"].ToString();
                i++;
            }

            return result;
        }


        /// <summary>
        /// 将用户离线消息存入数据库User表中的offlinemsg字段
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="somebody">somebody</param>
        public void saveMsgToOfflinemsg(string msg, string somebody)
        {
            //msg = "[talk][101511]Shane 18:33:59:\nhell[e]"
            //somebody = "253841"
            //先获取253841的离线消息，再将本次消息msg加在末尾，重新存入
            HandleUserEvent temp = new HandleUserEvent(somebody);
            string myleft = temp.GetLeftMsg(); //temp.myAccount = 253841
            if (myleft != string.Empty)
                myleft += msg;
            else
                myleft = msg;
            string sql = "update Users set U_offlinemsg='" + myleft + "' where U_account='" + somebody + "'";
            db.QueryNoReturn(sql);
        }

        /// <summary>
        /// 获得用户未收到的信息
        /// </summary>
        /// <returns></returns>
        public string GetLeftMsg()
        {
            string getmsg = "select U_offlinemsg from Users where U_account='" + myAccount + "'";
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
            string clearmsg = "update Users set U_offlinemsg='[off]' where U_account='" + myAccount + "'";
            string clearofflinemsg = "delete from OfflineMsg where Userid='" + myAccount + "'";
            db.QueryNoReturn(clearmsg);
            db.QueryNoReturn(clearofflinemsg);
        }




        public void DownLine(string myAccount) //myAccount下线
        {
            string downline = "update Users set U_status=0 where U_account='" + myAccount + "'";
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




    }
}
