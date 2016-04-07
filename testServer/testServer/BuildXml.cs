using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace testServer
{
    class BuildXml
    {
        mydb db;
        private string QQnum;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num">用户QQ号码</param>
        public BuildXml(string num)
        {
            this.QQnum = num;
            db = new mydb();
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public string BuildFriend()
        {
            string friends = "<friends>";
            DataSet ds = new DataSet();
            try
            {
                string sql = @"select U_account as QQ号码,U_name as 昵称,U_header as 头像,U_status as 是否在线 from Users where U_account=
any(select Friend_friendid from Friends where Friend_hostid='" + QQnum + "')";
                ds = db.Query(sql);
            }
            catch (Exception e)
            {
                MessageBox.Show("Query Friends Error:\n" + e.Message);
            }

            foreach (DataRow row in ds.Tables[0].Rows) //查询出来的第一个表Tables[0]
            {
                friends += String.Format("<qqnum{0} name=\"{1}\" img=\"{2}\" ison=\"{3}\"></qqnum{4}>\n",
                    row["QQ号码"].ToString(), row["昵称"].ToString(), row["头像"].ToString(),
                    row["是否在线"].ToString(), row["QQ号码"].ToString());
            }
            friends += "</friends>";

            return friends;
        }

        public string BuildLeftMsg()
        {
            string leftmsg = "<leftmsg>";
            DataSet ds = new DataSet();
            try
            {
                string sql = @"select userid as QQ号码, content as 离线消息 from OfflineMsg where id='" + QQnum + "')";
                ds = db.Query(sql);
            }
            catch (Exception e)
            {
                MessageBox.Show("Query Friends Error:\n" + e.Message);
            }

            foreach (DataRow row in ds.Tables[0].Rows) //查询出来的第一个表Tables[0]
            {
                leftmsg += String.Format("<num account=\"{0}\" msg=\"{1}\"></num>\n",
                    row["QQ号码"].ToString(),row["离线消息"].ToString());
            }
            leftmsg += "</leftmsg>";

            return leftmsg;

        }


        public string GetAllNums(string allnumxml)
        {
            /*
             * 例如账号101511请求登录，其由一个好友253841，则返回信息如下allnumxml
             * <all><MySimple><qqnum101511 name="Shane" img=19 sign="签名测试"></qqnum101511></MySimple>
             * <friends><qqnum253841 name="Bryan" img=1  ison=1></qqnum253841></friend></all>
             */
            string result = "";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(allnumxml);
            foreach (XmlNode father in doc.DocumentElement.ChildNodes)
            {
                if (father.Name != "MySimple") //father.Name = "friends"
                {
                    foreach (XmlNode child in father.ChildNodes)
                    {
                        if (Convert.ToInt32(child.Attributes["ison"].Value) == 1) //好友在线
                        {
                            //截取qqnum以后的，按,分隔
                            result += child.Name.Substring(5) + ",";
                        }
                    }
                }
            }
            return result;

        }
    }
}
