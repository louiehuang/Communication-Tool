using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testServer
{
    class UserLogin
    {
        private string sql_status;
        private string sql_login;
        mydb db;

        /// <summary>
        /// 构造函数，传入用户账号和密码，构建sql语句
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        public UserLogin(string account, string pwd)
        {
            db = new mydb();

            //更新用户在线状态语句，置为在线
            sql_status = "update Users set U_status=1 where U_account='" + account + "'";

            //查询用户账号密码返回行数的语句
            sql_login = "select count(*) from Users where U_account='" + account
                + "' and U_pwd='" + pwd + "'";
        }

        /// <summary>
        /// 从数据库查询Users表，判断用户的登录请求是否被允许
        /// </summary>
        /// <returns></returns>
        public bool Permit()
        {
            bool permit = false;

            //查询，返回受影响的行数，若登陆符合要求，则是一行受影响
            int iine = db.QueryLogin(sql_login);

            //一行受影响表示登陆成功，将用户在线状态置1
            if (iine == 1)
            {
                permit = true;
                db.QueryNoReturn(sql_status);
            }

            return permit;
        }
    }
}
