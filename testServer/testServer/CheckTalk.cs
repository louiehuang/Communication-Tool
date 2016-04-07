using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testServer
{
    public enum CheckType
    {
        Friend,
        Stranger,
        No
    };

    class CheckTalk
    {
        private string sqlstranger;
        private string sqlfriend;
        private string QQnum1;
        private string QQnum2;
        mydb db;
        /// <summary>
        /// 构造函数： 初始化A(num1)用户和B(num2)用户
        /// </summary>
        /// <param name="num1">A用户</param>
        /// <param name="num2">B用户</param>
        public CheckTalk(string num1, string num2)//num1发消息给num2,判断num2的 黑名单 或 好友名单中 是否有num1
        {
            this.QQnum1 = num1;
            this.QQnum2 = num2;
            db = new mydb();
            sqlfriend = "select count(*) from tb_friend where friend_somebody='" + num2
    + "' and friend_friends='" + num1 + "'";
            sqlstranger = "select count(*) from tb_strangers where strangers_somebody='" + num2
     + "' and strangers_stranger='" + num1 + "'";

        }
        /// <summary>
        /// 开始检查:返回true则A是在B的黑名单、陌生人、好友中,false则相反，type表明是要检查在好友中还是检查在黑名单中
        /// </summary>
        /// <param name="type"> </param>
        /// <returns></returns>
        public bool Check(CheckType type)
        {

            bool ishave = false;
            int i = -1;
            switch (type)
            {
                case CheckType.Friend: i = db.QueryLogin(sqlfriend); break;
                case CheckType.Stranger: i = db.QueryLogin(sqlstranger); break;
            }
            if (i != -1)
                ishave = true;
            return ishave;
        }

        /// <summary>
        /// num1和num2的关系，返回CheckType类型
        /// </summary>
        /// <returns></returns>
        public CheckType Relation()
        {
            CheckType result = CheckType.No;
            if (this.Check(CheckType.Friend))
            {
                result = CheckType.Friend;
            }
            else if (this.Check(CheckType.Stranger))
            {
                result = CheckType.Stranger;
            }
            return result;

        }
    }
}
