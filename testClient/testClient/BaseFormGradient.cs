using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace testClient
{
    /// <summary>
    /// Created by Uytterhaegen Tommy
    /// Please let me be here, I feel comfortable here :)
    /// </summary>
    public class BaseFormGradient : Form
    {
        #region Private Variables

        private Color _Color1 = Color.Gainsboro;
        private Color _Color2 = Color.White;
        private float _ColorAngle = 240f;

        #region Gui

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion

        #endregion

        #region Public Properties

        public Color Color1
        {
            get { return _Color1; }
            set
            {
                _Color1 = value;
                this.Invalidate(); // Tell the Form to repaint itself
            }
        }

        public Color Color2
        {
            get { return _Color2; }
            set
            {
                _Color2 = value;
                this.Invalidate(); // Tell the Form to repaint itself
            }
        }

        public float ColorAngle
        {
            get { return _ColorAngle; }
            set
            {
                _ColorAngle = value;
                this.Invalidate(); // Tell the Form to repaint itself
            }
        }

        #endregion

        #region Constructors, Destructors

        public BaseFormGradient()
        {
            InitializeComponent();

            this.SetStyles();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BaseFormGradient
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(432, 198);
            this.Name = "BaseFormGradient";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.BaseFormGradient_Load);
            this.ResumeLayout(false);

        }
        #endregion

        #region Private Methods

        private void SetStyles()
        {
            // Makes sure the form repaints when it was resized
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #endregion

        #region Overriden Methods

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Getting the graphics object
            Graphics g = pevent.Graphics;

            // Creating the rectangle for the gradient
            Rectangle rBackground = new Rectangle(0, 0, this.Width, this.Height);

            // Creating the lineargradient
            System.Drawing.Drawing2D.LinearGradientBrush bBackground
                = new System.Drawing.Drawing2D.LinearGradientBrush(rBackground, _Color1, _Color2, _ColorAngle);

            // Draw the gradient onto the form
            g.FillRectangle(bBackground, rBackground);

            // Disposing of the resources held by the brush
            bBackground.Dispose();
        }

        #endregion

        private void BaseFormGradient_Load(object sender, EventArgs e)
        {

        }
    }
}
