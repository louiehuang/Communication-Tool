using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testServer
{
    class UserLogin
    {
        private string online;
        private string sql;
        mydb db;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num">用户QQ号码</param>
        /// <param name="psw">用户QQ密码</param>
        public UserLogin(string account, string pwd)
        {
            db = new mydb();
            sql = "select count(*) from Users where U_account='" + account
                + "' and U_pwd='" + pwd + "'";
            online = "update Users set U_status=1 where U_account='" + account + "'";
        }
        /// <summary>
        /// 检查用户帐户和密码是否一致
        /// </summary>
        /// <returns>一致返回true，否则返回false</returns>
        public bool Check()
        {
            bool issuccess = false;
            int i = db.QueryLogin(sql);
            if (i != -1)
            {
                issuccess = true;
                db.QueryNoReturn(online);
            }
            return issuccess;
        }
    }
}
