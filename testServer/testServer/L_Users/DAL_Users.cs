using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace testServer
{
    //数据访问层
    class DAL_Users
    {
        DBConn conn = new DBConn();

        /// <summary>
        /// 获取用户的基本信息(账号，昵称，头像，签名)
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Model_Users DAL_Users_GetBasicInfo(string account)
        {
            Model_Users Model_users = new Model_Users();

            string sql = "select U_account,U_name, U_header, U_signature from Users where U_account='" + account + "'";
            DataSet DS = conn.DBQuery(sql);
            DataTable tbl = DS.Tables[0];

            if (tbl.Rows.Count > 0)
            {
                DataRow row = tbl.Rows[0];
                Model_users.U_account = row["U_account"].ToString();
                Model_users.U_name = row["U_name"].ToString();
                Model_users.U_header = row["U_header"].ToString();
                Model_users.U_signature = row["U_signature"].ToString();
            }

            return Model_users;
        }

        /// <summary>
        /// 获取用户的好友
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataSet DAL_Users_GetUserFriends(string account)
        {
            string sql = @"select U_account, U_name, U_header, U_status from Users where U_account=" +
                                        "any(select Friend_friendid from Friends where Friend_hostid='" + account + "')";

            return conn.DBQuery(sql);
        }


        /// <summary>
        /// 获取用户的群组
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataSet DAL_Users_GetUserGroups(string account)
        {
            string sql = @"select Group_num, Group_name, Group_header from Groups where Group_num=" +
                                    "any(select groupnum from Belongs where userid='" + account + "')";

            return conn.DBQuery(sql);
        }

        /// <summary>
        /// 判断用户登录请求是否被允许
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool DAL_Users_LoginPermission(string account, string pwd)
        {
            bool flag = false; //判断用户登录是否被允许

            ////更新用户在线状态语句，置为在线
            //string sql_status = "update Users set U_status=1 where U_account='" + account + "'";

            //查询用户账号密码返回行数的语句
            string sql_login = "select count(*) from Users where U_account='" + account
                + "' and U_pwd='" + pwd + "'";

            //登录请求查询，检索是否有符合的账号密码
            int line = conn.DB_LoginQuery(sql_login);

            if (line == 1)
            {
                flag = true;
                //conn.DBNonReturn(sql_status);
            }
            return flag;
        }

        /// <summary>
        /// 消息存入数据库的PersonalMsg表
        /// </summary>
        public void DAL_Users_SavePersonalMsg(string fromuser, string touser, string time, string realmsg)
        {
            string sql = string.Format("INSERT INTO PersonalMsg (PM_fromuser,PM_touser,PM_time,PM_content) values ('{0}','{1}','{2}','{3}')",
                    fromuser, touser, time, realmsg);

            int line = conn.DBcmd(sql);
            //MessageBox.Show("line" + line.ToString());

        }


        /// <summary>
        /// 将用户离线消息存入数据库User表中的offlinemsg字段
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="somebody">somebody</param>
        public void DAL_Users_saveMsgToOfflinemsg(string msg, string account)
        {
            string sql = "update Users set U_offlinemsg='" + msg + "' where U_account='" + account + "'";

            int line = conn.DBcmd(sql);

        }


        /// <summary>
        /// 获得用户未收到的信息
        /// </summary>
        /// <returns></returns>
        public string DAL_Users_GetOfflineMsg(string account)
        {
            string sql = "select U_offlinemsg from Users where U_account='" + account + "'";
            DataSet ds = new DataSet();

            ds = conn.DBQuery(sql);

            string offlinemsg = ds.Tables[0].Rows[0][0].ToString().Trim();
            return offlinemsg;
        }

        public void DAL_Users_ClearOfflineMsg(string account)
        {
            string sql = "update Users set U_offlinemsg='[off]' where U_account='" + account + "'";
            //string clearofflinemsg = "delete from OfflineMsg where Userid='" + account + "'";
            conn.DBcmd(sql);

        }

        public bool DAL_Users_Register(string account, string name, string pwd, int header, string gender, string sign, int age, int constellation, int blood)
        {

            string sql = string.Format("INSERT INTO Users (U_account, U_name, U_pwd, U_header, U_gender, U_signature, U_age, U_constellation, U_bloodtype) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    account, name, pwd, header, gender, sign, age, constellation, blood);

            int line = conn.DBcmd(sql);

            if (line == 1)
                return true;
            else
                return false;

        }


    }
}
