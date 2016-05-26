using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace testServer
{
    class DAL_Groups
    {
        DBConn conn = new DBConn();

        /// <summary>
        /// 获取群组的用户
        /// </summary>
        /// <param name="groupnum"></param>
        /// <returns></returns>
        public DataSet DAL_Groups_GetUserInGroup(string groupnum)
        {
            string sql = "select userid from Belongs WHERE groupnum = '" + groupnum + "'";
            return conn.DBQuery(sql);

        }

        /// <summary>
        /// 消息存入数据库的GroupMsg表
        /// </summary>
        public void DAL_Groups_SaveGroupMsg(string fromuser, string groupnum, string time, string realmsg)
        {
            string sql = string.Format("INSERT INTO GroupMsg (GM_groupid, GM_fromuser,GM_time,GM_content) values ('{0}','{1}','{2}','{3}')",
                                groupnum, fromuser, time, realmsg);

            int line = conn.DBcmd(sql);
            //MessageBox.Show("line" + line.ToString());

        }


    }
}
