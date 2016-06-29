using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace testServer
{
    //业务逻辑层
    class BLL_Users
    {
        DAL_Users DAL_users = new DAL_Users();

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Model_Users BLL_Users_GetBasicInfo(string account)
        {
            return DAL_users.DAL_Users_GetBasicInfo(account);
        }

        /// <summary>
        /// 获取用户好友
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataSet BLL_Users_GetUserFriends(string account)
        {
            return DAL_users.DAL_Users_GetUserFriends(account);
        }

        /// <summary>
        /// 获取用户群组
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public DataSet BLL_Users_GetUserGroups(string account)
        {
            return DAL_users.DAL_Users_GetUserGroups(account);
        }



        /// <summary>
        /// 判断用户登录请求是否被允许
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool BLL_Users_LoginPermission(string account, string pwd)
        {
            return DAL_users.DAL_Users_LoginPermission(account, pwd);
        }

        /// <summary>
        /// 消息存入数据库的PersonalMsg表
        /// </summary>
        public void BLL_Users_SavePersonalMsg(string fromuser, string touser, string time, string realmsg)
        {
            DAL_users.DAL_Users_SavePersonalMsg(fromuser, touser, time, realmsg);
        }


        /// <summary>
        /// 将用户离线消息存入数据库User表中的offlinemsg字段
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="somebody">somebody</param>
        public void BLL_Users_saveMsgToOfflinemsg(string msg, string account)
        {
            //msg = "[talk][101511]Shane 18:33:59:\nhell[e]"
            //somebody = "253841"
            //先获取253841的离线消息，再将本次消息msg加在末尾，重新存入
            string formerOfflinemsg = BLL_Users_GetOfflineMsg(account);//获取用户之前的离线消息;
            string currentOfflinemsg = string.Empty;

            if (formerOfflinemsg != string.Empty)
                currentOfflinemsg = formerOfflinemsg + msg; //将当前细线消息加在其后，存入
            else
                currentOfflinemsg = msg;

            DAL_users.DAL_Users_saveMsgToOfflinemsg(currentOfflinemsg, account);

        }

        /// <summary>
        /// 获取用户先前的离线消息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string BLL_Users_GetOfflineMsg(string account)
        {
            return DAL_users.DAL_Users_GetOfflineMsg(account);
        }

        /// <summary>
        /// 清空用户离线消息
        /// </summary>
        /// <param name="account"></param>
        public void BLL_Users_ClearOfflineMsg(string account)
        {
            DAL_users.DAL_Users_ClearOfflineMsg(account);
        }


        public bool BLL_Users_Register(string account, string name, string pwd, int header, string gender, string sign, int age, int constellation, int blood)
        {
            return DAL_users.DAL_Users_Register(account, name, pwd, header, gender, sign, age, constellation, blood);
        }



    }
}
