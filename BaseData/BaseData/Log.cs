using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData
{
    public class Log
    {
        public RichTextBox rtb;

        public void LogInfo(string message)
        {
            log(message, Color.Black);
        }

        public void LogError(string message)
        {
            log(message, Color.Red);
        }

        public void LogWarning(string message)
        {
            log(message, Color.Orange);
        }

        private void log(string message, Color color)
        {
            if (rtb.InvokeRequired)
            {
                rtb.Invoke(new Action(() => log(message, color)));
                return;
            }

            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionColor = color;
            rtb.AppendText($"{DateTime.Now:HH:mm:ss} - {message}\n");
            rtb.SelectionColor = rtb.ForeColor;
            rtb.ScrollToCaret();
        }
    }
}
