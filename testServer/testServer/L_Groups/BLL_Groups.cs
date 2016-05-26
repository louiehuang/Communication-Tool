using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace testServer
{
    class BLL_Groups
    {
        DAL_Groups DAL_groups = new DAL_Groups();


        /// <summary>
        /// 获取群组的用户
        /// </summary>
        /// <param name="groupnum"></param>
        /// <returns></returns>
        public string[] BLL_Groups_GetUserInGroup(string groupnum)
        {
            DataSet DS = DAL_groups.DAL_Groups_GetUserInGroup(groupnum);

            string[] result = new string[200]; //假设群组上限200人

            int i = 0;
            foreach (DataRow row in DS.Tables[0].Rows) //查询出来的第一个表Tables[0]
            {
                result[i] = row["userid"].ToString();
                i++;
            }

            return result;
        }



        /// <summary>
        /// 消息存入数据库的GrouplMsg表
        /// </summary>
        public void BLL_Groups_SaveGroupMsg(string fromuser, string groupnum, string time, string realmsg)
        {
            DAL_groups.DAL_Groups_SaveGroupMsg(fromuser, groupnum, time, realmsg);
        }


    }
}
