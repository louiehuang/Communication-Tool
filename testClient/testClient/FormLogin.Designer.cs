namespace testClient
{
    partial class FormLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_login = new CCWin.SkinControl.SkinButton();
            this.skinPanel1 = new CCWin.SkinControl.SkinPanel();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.rtb_pwd = new System.Windows.Forms.RichTextBox();
            this.cb_account = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_login.BackColor = System.Drawing.Color.Transparent;
            this.btn_login.BackRectangle = new System.Drawing.Rectangle(50, 23, 50, 23);
            this.btn_login.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(21)))), ((int)(((byte)(26)))));
            this.btn_login.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_login.DownBack = global::testClient.Properties.Resources.btn_Down;
            this.btn_login.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btn_login.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_login.ForeColor = System.Drawing.Color.White;
            this.btn_login.Location = new System.Drawing.Point(133, 282);
            this.btn_login.Margin = new System.Windows.Forms.Padding(0);
            this.btn_login.MouseBack = global::testClient.Properties.Resources.btn_norml;
            this.btn_login.Name = "btn_login";
            this.btn_login.NormlBack = global::testClient.Properties.Resources.btn_Mouse;
            this.btn_login.Size = new System.Drawing.Size(194, 30);
            this.btn_login.TabIndex = 6;
            this.btn_login.Text = "登  录";
            this.btn_login.UseVisualStyleBackColor = false;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // skinPanel1
            // 
            this.skinPanel1.BackColor = System.Drawing.Color.Transparent;
            this.skinPanel1.BackgroundImage = global::testClient.Properties.Resources.r4;
            this.skinPanel1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel1.DownBack = null;
            this.skinPanel1.Location = new System.Drawing.Point(39, 196);
            this.skinPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.skinPanel1.MouseBack = null;
            this.skinPanel1.Name = "skinPanel1";
            this.skinPanel1.NormlBack = null;
            this.skinPanel1.Size = new System.Drawing.Size(80, 80);
            this.skinPanel1.TabIndex = 9;
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.Create = true;
            this.skinButton1.DownBack = global::testClient.Properties.Resources.register;
            this.skinButton1.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton1.Location = new System.Drawing.Point(336, 199);
            this.skinButton1.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton1.MouseBack = global::testClient.Properties.Resources.register;
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = global::testClient.Properties.Resources.register1;
            this.skinButton1.Size = new System.Drawing.Size(65, 25);
            this.skinButton1.TabIndex = 10;
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinButton2
            // 
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.Create = true;
            this.skinButton2.DownBack = global::testClient.Properties.Resources.findpwd;
            this.skinButton2.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinButton2.Location = new System.Drawing.Point(336, 230);
            this.skinButton2.Margin = new System.Windows.Forms.Padding(0);
            this.skinButton2.MouseBack = global::testClient.Properties.Resources.findpwd;
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = global::testClient.Properties.Resources.findpwd1;
            this.skinButton2.Size = new System.Drawing.Size(65, 25);
            this.skinButton2.TabIndex = 11;
            this.skinButton2.UseVisualStyleBackColor = false;
            this.skinButton2.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // rtb_pwd
            // 
            this.rtb_pwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtb_pwd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtb_pwd.ForeColor = System.Drawing.Color.DimGray;
            this.rtb_pwd.Location = new System.Drawing.Point(133, 227);
            this.rtb_pwd.Margin = new System.Windows.Forms.Padding(0);
            this.rtb_pwd.MaxLength = 32767;
            this.rtb_pwd.Name = "rtb_pwd";
            this.rtb_pwd.Size = new System.Drawing.Size(194, 30);
            this.rtb_pwd.TabIndex = 12;
            this.rtb_pwd.Text = " 密码";
            this.rtb_pwd.Click += new System.EventHandler(this.rtb_pwd_Click);
            this.rtb_pwd.TextChanged += new System.EventHandler(this.rtb_pwd_TextChanged);
            this.rtb_pwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtb_pwd_KeyDown);
            // 
            // cb_account
            // 
            this.cb_account.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cb_account.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_account.ForeColor = System.Drawing.Color.DimGray;
            this.cb_account.FormattingEnabled = true;
            this.cb_account.ItemHeight = 24;
            this.cb_account.Items.AddRange(new object[] {
            "101511",
            "253841",
            "100000"});
            this.cb_account.Location = new System.Drawing.Point(133, 196);
            this.cb_account.Name = "cb_account";
            this.cb_account.Size = new System.Drawing.Size(194, 30);
            this.cb_account.TabIndex = 13;
            this.cb_account.Text = " 账号";
            this.cb_account.UseWaitCursor = true;
            this.cb_account.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cb_account_DrawItem);
            this.cb_account.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.cb_account_MeasureItem);
            this.cb_account.Click += new System.EventHandler(this.cb_account_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::testClient.Properties.Resources.afternoon;
            this.ClientSize = new System.Drawing.Size(432, 342);
            this.CloseBoxSize = new System.Drawing.Size(30, 30);
            this.CloseDownBack = global::testClient.Properties.Resources.sysbtn_close_down;
            this.CloseMouseBack = global::testClient.Properties.Resources.sysbtn_close_hover;
            this.CloseNormlBack = global::testClient.Properties.Resources.sysbtn_close_normal;
            this.Controls.Add(this.cb_account);
            this.Controls.Add(this.rtb_pwd);
            this.Controls.Add(this.skinButton2);
            this.Controls.Add(this.skinButton1);
            this.Controls.Add(this.skinPanel1);
            this.Controls.Add(this.btn_login);
            this.EffectCaption = CCWin.TitleType.None;
            this.MaxDownBack = global::testClient.Properties.Resources.sysbtn_max_down;
            this.MaximizeBox = false;
            this.MaxMouseBack = global::testClient.Properties.Resources.sysbtn_max_hover;
            this.MaxNormlBack = global::testClient.Properties.Resources.sysbtn_max_normal;
            this.MaxSize = new System.Drawing.Size(30, 30);
            this.MiniDownBack = global::testClient.Properties.Resources.sysbtn_min_down;
            this.MiniMouseBack = global::testClient.Properties.Resources.sysbtn_min_hover;
            this.MiniNormlBack = global::testClient.Properties.Resources.sysbtn_min_normal;
            this.MiniSize = new System.Drawing.Size(30, 30);
            this.Name = "FormLogin";
            this.ShowBorder = false;
            this.ShowDrawIcon = false;
            this.Text = "Chat";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormLogin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinButton btn_login;
        private CCWin.SkinControl.SkinPanel skinPanel1;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinButton skinButton2;
        private System.Windows.Forms.RichTextBox rtb_pwd;
        private System.Windows.Forms.ComboBox cb_account;
    }
}