using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.SkinClass;
using CCWin.SkinControl;

/*
 * 启动程序时先打开此界面，点击登陆后IsLogin置为true，以启动FormMain
 * 在Program.cs中设置启动顺序
 */


namespace testClient
{
    public partial class FormLogin : CCSkinMain
    {
        public bool IsLogin = false;

        public FormLogin()
        {
            //InitializeComponent(0); //初始化背景渐变色
            InitializeComponent();
        }

        /*
        private void InitializeComponent(int i)
        {
            // 
            // GradientForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(176, 134);
            this.Color1 = System.Drawing.Color.LightSteelBlue;
            this.Name = "GradientForm";
            this.Text = "GradientForm";
        }
        */

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void tb_pwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btn_login_Click(sender, e);//触发button事件
                //btn_login.PerformClick();
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string account = cb_account.Text.Trim();
            string pwd = rtb_pwd.Text.Trim();

            /*需补充格式检查*/

            myInfo.Login = "[login][" + account + "," + pwd + "]";
            IsLogin = true;
            this.Close();
        }


        private void rtb_pwd_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtb_pwd_Click(object sender, EventArgs e)
        {
            this.rtb_pwd.Text = string.Empty;
            this.rtb_pwd.ForeColor = Color.Black;
        }

        private void cb_account_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Brush myBrush = Brushes.Black;
            Font ft = new Font("微软雅黑", 10f);
            e.Graphics.DrawString(cb_account.Items[e.Index].ToString(), ft, myBrush, e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        private void cb_account_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = 24;
        }

        private void cb_account_Click(object sender, EventArgs e)
        {
            this.cb_account.Text = string.Empty;
            this.cb_account.ForeColor = Color.Black;
        }

        /// <summary>
        /// 回车登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtb_pwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btn_login_Click(sender, e);//触发button事件
                //btn_login.PerformClick();
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            FormRegister formRegister = new FormRegister();
            formRegister.Show();


        }

        private void skinButton2_Click(object sender, EventArgs e)
        {

        }



    }
}
