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
    class UserInfoXml
    {
        mydb db;
        private string account; //当前用户账号
        BLL_Users BLL_user = new BLL_Users();

        public UserInfoXml(string account)
        {
            this.account = account;
            db = new mydb();
        }

        /// <summary>
        /// 获取用户基本信息(账号,昵称,头像,签名)
        /// </summary>
        /// <returns>返回用户基本信息</returns>
        public string GetMyBasicInfo()
        {
            string myInfo = "<myInfo>";
            try
            {
                Model_Users Model_user = BLL_user.BLL_Users_Basic_Info(account);
                myInfo += string.Format("<me account=\"{0}\" name=\"{1}\" header=\"{2}\" signature=\"{3}\"></me>",
                                Model_user.U_account, Model_user.U_name, Model_user.U_header, Model_user.U_signature);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            //MessageBox.Show(myInfo);

            //DataSet ds = new DataSet();
            //try
            //{
            //    string sql = "select U_account,U_name, U_header, U_signature from Users where U_account='" + account + "'";
            //    ds = db.Query(sql);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Query MyInfo Error:\n" + e.Message);
            //}

            //DataRow row = ds.Tables[0].Rows[0];
            //myInfo += string.Format("<me account=\"{0}\" name=\"{1}\" header=\"{2}\" signature=\"{3}\"></me>",
            //                                   row["U_account"], row["U_name"].ToString(), row["U_header"].ToString(), row["U_signature"].ToString());


            myInfo += "</myInfo>";



            return myInfo;
        }


        /// <summary>
        /// 获取当前账号的好友信息，返回xml形式的字符串
        /// </summary>
        /// <returns></returns>
        public string GetUserFriendsXml()
        {
            string friends = "<friends>";
            DataSet ds = new DataSet();
            try
            {
                string sql = @"select U_account, U_name, U_header, U_status from Users where U_account=
any(select Friend_friendid from Friends where Friend_hostid='" + account + "')";
                ds = db.Query(sql);
            }
            catch (Exception e)
            {
                MessageBox.Show("Query Friends Error:\n" + e.Message);
            }

            foreach (DataRow row in ds.Tables[0].Rows) //查询到的第一个表Tables[0]
            {
                friends += String.Format("<frienditem account=\"{0}\" name=\"{1}\" header=\"{2}\"></frienditem>\n",
                    row["U_account"].ToString(), row["U_name"].ToString(), row["U_header"].ToString() );
            }

            friends += "</friends>";

            return friends;
        }

        /// <summary>
        ///  获取当前账号的群组信息，返回xml形式的字符串
        /// </summary>
        /// <returns></returns>
        public string GetUserGroupsXml()
        {
            string groups = "<groups>";
            DataSet ds = new DataSet();
            try
            {
                string sql = @"select Group_num, Group_name, Group_header from Groups where Group_num=
any(select groupnum from Belongs where userid='" + account + "')";
                ds = db.Query(sql);
            }
            catch (Exception e)
            {
                MessageBox.Show("Query Groups Error:\n" + e.Message);
            }

            foreach (DataRow row in ds.Tables[0].Rows) //查询出来的第一个表Tables[0]
            {
                groups += String.Format("<groupitem num=\"{0}\" name=\"{1}\" header=\"{2}\"></groupitem>\n",
                    row["Group_num"].ToString(), row["Group_name"].ToString(), row["Group_header"].ToString());
            }
            groups += "</groups>";

            return groups;
        }



        ///*下面这个并没有用上*/
        //public string GetLeftMsgInXml(HandleUserEvent handleUserEvent)
        //{
        //    string result = "<leftmsg>" + handleUserEvent.GetLeftMsg() + "/leftmsg>";
        //    return result;
        
        //}

    }
}
