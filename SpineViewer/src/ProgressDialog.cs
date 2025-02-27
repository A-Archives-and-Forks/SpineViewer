﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpineViewer
{
    public partial class ProgressDialog : Form
    {
        [Category("自定义"), Description("BackgroundWorker 的 DoWork 事件")]
        public event DoWorkEventHandler? Dowork
        {
            add { backgroundWorker.DoWork += value; }
            remove { backgroundWorker.DoWork -= value; }
        }

        public void RunWorkerAsync() { backgroundWorker.RunWorkerAsync(); }
        public void RunWorkerAsync(object? argument) { backgroundWorker.RunWorkerAsync(argument); }

        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label_Tip.Text = e.UserState as string;
            progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Program.Logger.Error(e.Error.ToString());
                MessageBox.Show(e.Error.Message, "执行出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
            }
            else if (e.Cancelled)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                DialogResult= DialogResult.OK;
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
            button_Cancel.Enabled = false;
        }
    }
}
