namespace testClient
{
    partial class GroupChat
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.pb_header = new System.Windows.Forms.PictureBox();
            this.skinbtn_send = new CCWin.SkinControl.SkinButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_header)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(7, 115);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(442, 268);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(7, 392);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(442, 99);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // pb_header
            // 
            this.pb_header.BackColor = System.Drawing.Color.Transparent;
            this.pb_header.Location = new System.Drawing.Point(7, 31);
            this.pb_header.Name = "pb_header";
            this.pb_header.Size = new System.Drawing.Size(78, 78);
            this.pb_header.TabIndex = 2;
            this.pb_header.TabStop = false;
            // 
            // skinbtn_send
            // 
            this.skinbtn_send.BackColor = System.Drawing.Color.Transparent;
            this.skinbtn_send.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinbtn_send.DownBack = global::testClient.Properties.Resources.btn_Down;
            this.skinbtn_send.DrawType = CCWin.SkinControl.DrawStyle.Img;
            this.skinbtn_send.Location = new System.Drawing.Point(362, 497);
            this.skinbtn_send.MouseBack = global::testClient.Properties.Resources.btn_Mouse;
            this.skinbtn_send.Name = "skinbtn_send";
            this.skinbtn_send.NormlBack = global::testClient.Properties.Resources.btn_norml;
            this.skinbtn_send.Size = new System.Drawing.Size(87, 36);
            this.skinbtn_send.TabIndex = 3;
            this.skinbtn_send.Text = "发送";
            this.skinbtn_send.UseVisualStyleBackColor = false;
            this.skinbtn_send.Click += new System.EventHandler(this.skinbtn_send_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.Location = new System.Drawing.Point(98, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "GroupName";
            // 
            // GroupChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Back = global::testClient.Properties.Resources.main_2;
            this.ClientSize = new System.Drawing.Size(471, 544);
            this.CloseBoxSize = new System.Drawing.Size(30, 30);
            this.CloseDownBack = global::testClient.Properties.Resources.sysbtn_close_down;
            this.CloseMouseBack = global::testClient.Properties.Resources.sysbtn_close_hover;
            this.CloseNormlBack = global::testClient.Properties.Resources.sysbtn_close_normal;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.skinbtn_send);
            this.Controls.Add(this.pb_header);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.MaxDownBack = global::testClient.Properties.Resources.sysbtn_max_down;
            this.MaxMouseBack = global::testClient.Properties.Resources.sysbtn_max_hover;
            this.MaxNormlBack = global::testClient.Properties.Resources.sysbtn_max_normal;
            this.MaxSize = new System.Drawing.Size(30, 30);
            this.MiniDownBack = global::testClient.Properties.Resources.sysbtn_min_down;
            this.MiniMouseBack = global::testClient.Properties.Resources.sysbtn_min_hover;
            this.MiniNormlBack = global::testClient.Properties.Resources.sysbtn_min_normal;
            this.MiniSize = new System.Drawing.Size(30, 30);
            this.Name = "GroupChat";
            this.Text = "GroupChat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GroupChat_FormClosing);
            this.Load += new System.EventHandler(this.GroupChat_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_header)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.PictureBox pb_header;
        private CCWin.SkinControl.SkinButton skinbtn_send;
        private System.Windows.Forms.Label label1;
    }
}