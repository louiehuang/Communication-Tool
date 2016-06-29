namespace testClient
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            CCWin.SkinControl.ChatListItem chatListItem1 = new CCWin.SkinControl.ChatListItem();
            CCWin.SkinControl.ChatListItem chatListItem2 = new CCWin.SkinControl.ChatListItem();
            CCWin.SkinControl.ChatListItem chatListItem3 = new CCWin.SkinControl.ChatListItem();
            CCWin.SkinControl.ChatListSubItem chatListSubItem1 = new CCWin.SkinControl.ChatListSubItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            CCWin.SkinControl.Animation animation1 = new CCWin.SkinControl.Animation();
            this.clb_friend = new CCWin.SkinControl.ChatListBox();
            this.clb_group = new CCWin.SkinControl.ChatListBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.TabControl1 = new CCWin.SkinControl.SkinTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.skinPanel1 = new CCWin.SkinControl.SkinPanel();
            this.btn_search = new CCWin.SkinControl.SkinButton();
            this.tb_search = new CCWin.SkinControl.SkinAlphaWaterTextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.skinPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // clb_friend
            // 
            this.clb_friend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(252)))));
            this.clb_friend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clb_friend.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clb_friend.ForeColor = System.Drawing.Color.Black;
            this.clb_friend.FriendsMobile = true;
            chatListItem1.Bounds = new System.Drawing.Rectangle(0, 1, 280, 25);
            chatListItem1.IsTwinkleHide = false;
            chatListItem1.OwnerChatListBox = this.clb_friend;
            chatListItem1.Text = "我的好友";
            chatListItem1.TwinkleSubItemNumber = 0;
            chatListItem2.Bounds = new System.Drawing.Rectangle(0, 27, 280, 25);
            chatListItem2.IsTwinkleHide = false;
            chatListItem2.OwnerChatListBox = this.clb_friend;
            chatListItem2.Text = "其他用户";
            chatListItem2.TwinkleSubItemNumber = 0;
            this.clb_friend.Items.AddRange(new CCWin.SkinControl.ChatListItem[] {
            chatListItem1,
            chatListItem2});
            this.clb_friend.ListSubItemMenu = null;
            this.clb_friend.Location = new System.Drawing.Point(0, 0);
            this.clb_friend.Name = "clb_friend";
            this.clb_friend.SelectSubItem = null;
            this.clb_friend.Size = new System.Drawing.Size(280, 376);
            this.clb_friend.SubItemMenu = null;
            this.clb_friend.TabIndex = 0;
            this.clb_friend.Text = "clb_friend";
            this.clb_friend.DoubleClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.clb_friend_DoubleClickSubItem);
            // 
            // clb_group
            // 
            this.clb_group.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(252)))));
            this.clb_group.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clb_group.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clb_group.ForeColor = System.Drawing.Color.Black;
            this.clb_group.FriendsMobile = true;
            chatListItem3.Bounds = new System.Drawing.Rectangle(0, 1, 274, 25);
            chatListItem3.IsTwinkleHide = false;
            chatListItem3.OwnerChatListBox = this.clb_group;
            chatListSubItem1.Bounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            chatListSubItem1.DisplayName = "displayName";
            chatListSubItem1.HeadImage = null;
            chatListSubItem1.HeadRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            chatListSubItem1.ID = ((uint)(0u));
            chatListSubItem1.IpAddress = null;
            chatListSubItem1.IsTwinkle = false;
            chatListSubItem1.IsTwinkleHide = false;
            chatListSubItem1.IsVip = false;
            chatListSubItem1.NicName = "nicName";
            chatListSubItem1.OwnerListItem = chatListItem3;
            chatListSubItem1.PersonalMsg = "Personal Message ...";
            chatListSubItem1.PlatformTypes = CCWin.SkinControl.PlatformType.PC;
            chatListSubItem1.QQShow = null;
            chatListSubItem1.Status = CCWin.SkinControl.ChatListSubItem.UserStatus.Online;
            chatListSubItem1.Tag = null;
            chatListSubItem1.TcpPort = 0;
            chatListSubItem1.UpdPort = 0;
            chatListItem3.SubItems.AddRange(new CCWin.SkinControl.ChatListSubItem[] {
            chatListSubItem1});
            chatListItem3.Text = "我的群组";
            chatListItem3.TwinkleSubItemNumber = 0;
            this.clb_group.Items.AddRange(new CCWin.SkinControl.ChatListItem[] {
            chatListItem3});
            this.clb_group.ListSubItemMenu = null;
            this.clb_group.Location = new System.Drawing.Point(3, 3);
            this.clb_group.Name = "clb_group";
            this.clb_group.SelectSubItem = null;
            this.clb_group.Size = new System.Drawing.Size(274, 370);
            this.clb_group.SubItemMenu = null;
            this.clb_group.TabIndex = 0;
            this.clb_group.Text = "chatListBox1";
            this.clb_group.DoubleClickSubItem += new CCWin.SkinControl.ChatListBox.ChatListEventHandler(this.clb_group_DoubleClickSubItem);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "0.jpg");
            this.imageList1.Images.SetKeyName(1, "1.jpg");
            this.imageList1.Images.SetKeyName(2, "2.jpg");
            this.imageList1.Images.SetKeyName(3, "3.jpg");
            this.imageList1.Images.SetKeyName(4, "4.jpg");
            this.imageList1.Images.SetKeyName(5, "5.jpg");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Location = new System.Drawing.Point(7, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(80, 80);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(96, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "昵称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(98, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "签名";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "chat.png");
            this.imageList2.Images.SetKeyName(1, "group.png");
            // 
            // TabControl1
            // 
            animation1.AnimateOnlyDifferences = false;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 2F;
            animation1.TransparencyCoeff = 0F;
            this.TabControl1.Animation = animation1;
            this.TabControl1.AnimatorType = CCWin.SkinControl.AnimationType.HorizSlide;
            this.TabControl1.CloseRect = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.TabControl1.Controls.Add(this.tabPage1);
            this.TabControl1.Controls.Add(this.tabPage2);
            this.TabControl1.HeadBack = null;
            this.TabControl1.ImageList = this.imageList2;
            this.TabControl1.ImgTxtOffset = new System.Drawing.Point(0, 0);
            this.TabControl1.ItemSize = new System.Drawing.Size(90, 36);
            this.TabControl1.Location = new System.Drawing.Point(0, 156);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.PageArrowDown = ((System.Drawing.Image)(resources.GetObject("TabControl1.PageArrowDown")));
            this.TabControl1.PageArrowHover = ((System.Drawing.Image)(resources.GetObject("TabControl1.PageArrowHover")));
            this.TabControl1.PageCloseHover = ((System.Drawing.Image)(resources.GetObject("TabControl1.PageCloseHover")));
            this.TabControl1.PageCloseNormal = ((System.Drawing.Image)(resources.GetObject("TabControl1.PageCloseNormal")));
            this.TabControl1.PageDown = ((System.Drawing.Image)(resources.GetObject("TabControl1.PageDown")));
            this.TabControl1.PageHover = ((System.Drawing.Image)(resources.GetObject("TabControl1.PageHover")));
            this.TabControl1.PageImagePosition = CCWin.SkinControl.SkinTabControl.ePageImagePosition.Left;
            this.TabControl1.PageNorml = null;
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(280, 412);
            this.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.TabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.clb_friend);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(0, 36);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(280, 376);
            this.tabPage1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.clb_group);
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(0, 36);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(280, 376);
            this.tabPage2.TabIndex = 1;
            // 
            // skinPanel1
            // 
            this.skinPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.skinPanel1.Controls.Add(this.btn_search);
            this.skinPanel1.Controls.Add(this.tb_search);
            this.skinPanel1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel1.DownBack = null;
            this.skinPanel1.Location = new System.Drawing.Point(2, 113);
            this.skinPanel1.MouseBack = null;
            this.skinPanel1.Name = "skinPanel1";
            this.skinPanel1.NormlBack = null;
            this.skinPanel1.Size = new System.Drawing.Size(276, 30);
            this.skinPanel1.TabIndex = 5;
            // 
            // btn_search
            // 
            this.btn_search.BackColor = System.Drawing.Color.Transparent;
            this.btn_search.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.btn_search.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btn_search.DownBack = null;
            this.btn_search.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.btn_search.Image = global::testClient.Properties.Resources.btnSearch;
            this.btn_search.Location = new System.Drawing.Point(247, 3);
            this.btn_search.MouseBack = null;
            this.btn_search.Name = "btn_search";
            this.btn_search.NormlBack = null;
            this.btn_search.Size = new System.Drawing.Size(25, 25);
            this.btn_search.TabIndex = 1;
            this.btn_search.UseVisualStyleBackColor = false;
            // 
            // tb_search
            // 
            this.tb_search.BackAlpha = 0;
            this.tb_search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_search.Location = new System.Drawing.Point(10, 6);
            this.tb_search.Multiline = true;
            this.tb_search.Name = "tb_search";
            this.tb_search.Size = new System.Drawing.Size(217, 18);
            this.tb_search.TabIndex = 0;
            this.tb_search.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_search.WaterFont = new System.Drawing.Font("微软雅黑", 9F);
            this.tb_search.WaterText = "  搜索: 好友、群组";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Chat";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // timer2
            // 
            this.timer2.Interval = 300;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(158)))), ((int)(((byte)(225)))));
            this.ClientSize = new System.Drawing.Size(280, 600);
            this.CloseBoxSize = new System.Drawing.Size(30, 30);
            this.CloseDownBack = global::testClient.Properties.Resources.sysbtn_close_down;
            this.CloseMouseBack = global::testClient.Properties.Resources.sysbtn_close_hover;
            this.CloseNormlBack = global::testClient.Properties.Resources.sysbtn_close_normal;
            this.Controls.Add(this.skinPanel1);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.MaximizeBox = false;
            this.MiniDownBack = global::testClient.Properties.Resources.sysbtn_min_down;
            this.MiniMouseBack = global::testClient.Properties.Resources.sysbtn_min_hover;
            this.MiniNormlBack = global::testClient.Properties.Resources.sysbtn_min_normal;
            this.MiniSize = new System.Drawing.Size(30, 30);
            this.Name = "FormMain";
            this.ShowDrawIcon = false;
            this.Text = "";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.skinPanel1.ResumeLayout(false);
            this.skinPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private CCWin.SkinControl.SkinTabControl TabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private CCWin.SkinControl.ChatListBox clb_friend;
        private CCWin.SkinControl.ChatListBox clb_group;
        private CCWin.SkinControl.SkinPanel skinPanel1;
        private CCWin.SkinControl.SkinButton btn_search;
        private CCWin.SkinControl.SkinAlphaWaterTextBox tb_search;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Timer timer2;
    }
}

