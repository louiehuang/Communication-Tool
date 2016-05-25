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

        public Model_Users DAL_Users_Basic_Info(string account)
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
    }
}
