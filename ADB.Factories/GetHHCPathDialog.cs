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

    internal partial class GetHHCPathDialog : Form
    {

        public GetHHCPathDialog()
        {
            InitializeComponent();
        }

        public string HHCPath
        {
            get { return textBox1.Text; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "HHC|hhc.exe";
            if (ofd.ShowDialog() == DialogResult.OK) textBox1.Text = ofd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                MessageBox.Show(this, Resources.strTip1, Resources.strTip);
            else
                DialogResult = DialogResult.OK;
        }
    }
}