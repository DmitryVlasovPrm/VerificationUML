using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verification
{
    class WaitingForm : Form
    {
        Form waitingForm;
        ProgressBar pBar;

        public void show()
        {
            waitingForm.Show();
        }
        public void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pBar.MarqueeAnimationSpeed = 0;
            pBar.Style = ProgressBarStyle.Blocks;
            pBar.Value = pBar.Minimum;
            waitingForm.Visible = false;
            waitingForm.Close();
            waitingForm.Dispose();
        }
        private void OnLostFocus(object sender, EventArgs e)
        {
            base.OnLostFocus(e);
            waitingForm.Focus();
        }

        public void InitializationWaitingForm(Form parentForm, string msg)
        {
            waitingForm = new Form();
            waitingForm.Owner = parentForm;
            waitingForm.FormBorderStyle = FormBorderStyle.Sizable;
            waitingForm.ControlBox = false;
            waitingForm.StartPosition = FormStartPosition.CenterScreen;

            waitingForm.LostFocus += OnLostFocus;
            waitingForm.TopMost = true;

            waitingForm.Width = 200;
            waitingForm.Height = 100;

            Label label = new Label();
            label.Text = msg;
            label.Dock = DockStyle.Top;
            waitingForm.Controls.Add(label);


            pBar = new ProgressBar();
            pBar.Dock = DockStyle.Bottom;
            pBar.Style = ProgressBarStyle.Marquee;
            pBar.MarqueeAnimationSpeed = 50;
            waitingForm.Controls.Add(pBar);
            waitingForm.Visible = false;
        }
    }
}
