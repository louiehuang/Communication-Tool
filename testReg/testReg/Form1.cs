using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace testReg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string str = "Shanesf 11:05:39:\nhello";

            //int index = str.IndexOf("\n");
            //string realmsg = str.Substring(index);
            //MessageBox.Show(realmsg);

            string LeftMsg = "[talk][101511]Shane 18:33:59:\nhello[e][talk][101511]Shane 19:33:59:\nHi[e]";
            //string[] msg = System.Text.RegularExpressions.Regex.Split(LeftMsg, "[e]");
            //string[] msg = Regex.Split(LeftMsg, "[e]", RegexOptions.IgnoreCase);
            List<string> msg = new List<string>();
            int index = LeftMsg.IndexOf("[e]");
            while (index != -1)
            {
                msg.Add(LeftMsg.Substring(0, index));
                LeftMsg = LeftMsg.Substring(index + 3);

                index = LeftMsg.IndexOf("[e]");

            }

            foreach (string s in msg)
            {
                MessageBox.Show(s);
            }

        }
    }
}
