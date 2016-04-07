using System;
using System.Collections.Generic;
using System.Text;

namespace testServer
{
    class HandleMsg
    {
        private string PartMsg = "";
        private string Msg;

        public string MSG
        {
            set { Msg = value; }
            get { return Msg; }
        }

        public string[] process()
        {
            List<string> news = new List<string>();
            for (int i = 0; i < 1; i++)
            {
                int x = Msg.IndexOf("[e]");
                if (x == -1)
                {
                    PartMsg = Msg;
                }
                else
                {
                    news.Add((PartMsg + Msg.Substring(0, x)));
                    Msg = Msg.Substring(x + 3);
                    i -= 1;
                    PartMsg = "";
                }
            }
            return news.ToArray();
        }
    }
}
