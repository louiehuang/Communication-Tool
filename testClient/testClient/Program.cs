using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace testClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*先显示登录界面*/
            FormLogin mylogin = new FormLogin();
            mylogin.ShowDialog();

            /*点击登录后启动FormMain，由FormMain的timer1负责与服务器的连接和信息交换*/
            if (mylogin.IsLogin == true)
            {
                Application.Run(new FormMain());
            }
        }
    }
}
