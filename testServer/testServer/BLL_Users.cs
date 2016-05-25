using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testServer
{
    //业务逻辑层
    class BLL_Users
    {
        DAL_Users DAL_users = new DAL_Users();
        public Model_Users BLL_Users_Basic_Info(string account)
        {
            return DAL_users.DAL_Users_Basic_Info(account);
        }
    }
}
