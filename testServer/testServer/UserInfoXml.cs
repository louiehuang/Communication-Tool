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
        private string account; //当前用户账号
        BLL_Users BLL_user = new BLL_Users();
        BLL_Groups BLL_group = new BLL_Groups();

        public UserInfoXml(string account)
        {
            this.account = account;
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
                Model_Users Model_user = BLL_user.BLL_Users_GetBasicInfo(account);
                myInfo += string.Format("<me account=\"{0}\" name=\"{1}\" header=\"{2}\" signature=\"{3}\"></me>",
                                Model_user.U_account, Model_user.U_name, Model_user.U_header, Model_user.U_signature);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

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
            DataSet DS = BLL_user.BLL_Users_GetUserFriends(account); //获取用户好友结果的数据集
            DataTable tbl = DS.Tables[0];
            foreach (DataRow row in tbl.Rows) //查询到的第一个表Tables[0]
            {
                friends += String.Format("<frienditem account=\"{0}\" name=\"{1}\" header=\"{2}\"></frienditem>\n",
                    row["U_account"].ToString(), row["U_name"].ToString(), row["U_header"].ToString());
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

            DataSet DS = BLL_user.BLL_Users_GetUserGroups(account); //获取用户群组结果的数据集

            foreach (DataRow row in DS.Tables[0].Rows) //查询出来的第一个表Tables[0]
            {
                groups += String.Format("<groupitem num=\"{0}\" name=\"{1}\" header=\"{2}\"></groupitem>\n",
                    row["Group_num"].ToString(), row["Group_name"].ToString(), row["Group_header"].ToString());
            }
            
            groups += "</groups>";

            return groups;
        }


    }
}
