using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ADB.Factories
{
    using Properties;

    internal partial class BuildProgress : Form
    {
        string _documentFileName = "";
        CHMDocumentBuilderFactory _builder;

        public BuildProgress(CHMDocumentBuilderFactory builder)
        {
            InitializeComponent();
            _builder = builder;
        }

        private delegate void NotifyDelegate(NotifyType notifyType, object args);

        public void Notify(NotifyType notifyType, object args)
        {
            switch (notifyType)
            {
            case NotifyType.ShowProgress:
                this.progressBar1.Value = (int)args;
                break;
            case NotifyType.OutputDataReceived:
                this.textBox1.AppendText((string)args);
                break;
			case NotifyType.Canceled:
				button1.Enabled = true;
				button3.Enabled = false;
				button2.Visible = false;
				textBox1.Text = Resources.strCanceled;
				textBox1.ForeColor = Color.Red;
				_documentFileName = (string)args;
				break;
            case NotifyType.Finished:
                button1.Enabled = true;
                button3.Enabled = false;
                button2.Visible = true;
                _documentFileName = (string)args;
                break;
			case NotifyType.FinishedNoCHM:
				button1.Enabled = true;
				button3.Enabled = false;
				button2.Visible = false;
				textBox1.Text = Resources.strNoCHM;
				textBox1.ForeColor = Color.Red;
				_documentFileName = (string)args;
				break;
            case NotifyType.ShowMsg:
                this.textBox1.Text = (string)args;
                break;
            }
		}

		public void BeginInvokeNotify(NotifyType notifyType, object args)
		{
			this.BeginInvoke(new NotifyDelegate(this.Notify), notifyType, args);
		}

		public void InvokeNotify(NotifyType notifyType, object args)
		{
			this.Invoke(new NotifyDelegate(this.Notify), notifyType, args);
		}

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(_documentFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_builder != null && !_builder.IsFinish &&
                MessageBox.Show(this, Resources.String55, Resources.strTip, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _builder.Cancel();
            }
        }
    }

    internal enum NotifyType
    {
        ShowProgress,
        OutputDataReceived,
        Finished,
		Canceled,
		FinishedNoCHM,
        ShowMsg
    }
}