using NLog;
using SpineViewer.Spine;
using System.ComponentModel;
using System.Diagnostics;

namespace SpineViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeLogConfiguration();
        }

        /// <summary>
        /// ��ʼ��������־��
        /// </summary>
        private void InitializeLogConfiguration()
        {
            // ������־
            var rtbTarget = new NLog.Windows.Forms.RichTextBoxTarget
            {
                Name = "rtbTarget",
                TargetForm = this,
                TargetRichTextBox = rtbLog,
                AutoScroll = true,
                MaxLines = 3000,
                SupportLinks = true,
                Layout = "[${level:format=OneLetter}]${date:format=yyyy-MM-dd HH\\:mm\\:ss} - ${message}"
            };

            rtbTarget.WordColoringRules.Add(new("[D]", "Gray", "Empty", FontStyle.Bold));
            rtbTarget.WordColoringRules.Add(new("[I]", "DimGray", "Empty", FontStyle.Bold));
            rtbTarget.WordColoringRules.Add(new("[W]", "DarkOrange", "Empty", FontStyle.Bold));
            rtbTarget.WordColoringRules.Add(new("[E]", "Red", "Empty", FontStyle.Bold));
            rtbTarget.WordColoringRules.Add(new("[F]", "DarkRed", "Empty", FontStyle.Bold));

            LogManager.Configuration.AddTarget(rtbTarget);
            LogManager.Configuration.AddRule(LogLevel.Debug, LogLevel.Fatal, rtbTarget);
            LogManager.ReconfigExistingLoggers();
        }

        private void ExportPng_Work(object? sender, DoWorkEventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            spinePreviewer.StartPreview();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            spinePreviewer.StopPreview();
        }

        private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            spineListView.Add();
        }

        private void toolStripMenuItem_BatchOpen_Click(object sender, EventArgs e)
        {
            spineListView.BatchAdd();
        }

        private void toolStripMenuItem_Export_Click(object sender, EventArgs e)
        {
            lock (spineListView.Spines)
            {
                if (spineListView.Spines.Count <= 0)
                {
                    MessageBox.Show("�����ٴ�һ�������ļ�", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            //var openDialog = new BatchOpenSpineDialog();
            //if (openDialog.ShowDialog() != DialogResult.OK)
            //    return;

            //var progressDialog = new ProgressDialog();
            //progressDialog.DoWork += ExportPng_Work;
            //progressDialog.RunWorkerAsync(new { openDialog.SkelPaths, openDialog.Version });
            //progressDialog.ShowDialog();
        }

        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripMenuItem_ResetAnimation_Click(object sender, EventArgs e)
        {
            lock (spineListView.Spines)
            {
                foreach (var spine in spineListView.Spines)
                    spine.CurrentAnimation = spine.CurrentAnimation;
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ActiveControl = null;
        }

        private void splitContainer_MouseUp(object sender, MouseEventArgs e)
        {
            ActiveControl = null;
        }

        private void propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            (sender as PropertyGrid)?.Refresh();
        }

        private void spinePreviewer_MouseUp(object sender, MouseEventArgs e)
        {
            propertyGrid_Spine.Refresh();
        }
    }
}
