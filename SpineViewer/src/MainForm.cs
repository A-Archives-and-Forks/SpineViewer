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
            propertyGrid_Previewer.SelectedObject = spinePreviewer.Property;

            InitializeLogConfiguration();
            spinePreviewer.Property.Resolution = new(1280, 720);
            spinePreviewer.StartPreview();
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

        #region �˵���

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
            Program.Logger.Debug("Debug Test");
            Program.Logger.Info("Info Test");
            Program.Logger.Warn("Warn Test");
            Program.Logger.Error("Error Test");
            Program.Logger.Fatal("Fatal Test");
        }

        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region �������
        #endregion

        #region Ԥ������

        private void spinePreviewer_RenderFrame(object sender, RenderFrameEventArgs e)
        {
            var target = e.RenderTarget;
            var delta = e.Delta;
            Spine.Spine[] spines = null;

            // ��Ҫ�ڿؼ��߳��õ�����ǳ��������
            if (spineListView.InvokeRequired)
                spineListView.Invoke(() => spines = spineListView.Spines.ToArray());
            else
                spines = spineListView.Spines.ToArray();

            foreach (var spine in spines.Reverse())
            {
                spine.Update(delta);
                target.Draw(spine);
            }
        }

        #endregion

        #region ����

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
            (sender as PropertyGrid).Refresh();
        }

        #endregion
    }
}
