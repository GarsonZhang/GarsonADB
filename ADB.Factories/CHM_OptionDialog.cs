using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;

namespace ADB.Factories
{
	public enum BuildMode
	{
		HTMLAndCHM, HTMLOnly
	}
	public partial class CHM_OptionDialog : Form
	{
		Encoding _encoding;

		public BuildMode BuildMode
		{
			get
			{
				return rbHTMLAndCHM.Checked ? BuildMode.HTMLAndCHM : BuildMode.HTMLOnly;
			}
		}
		public Encoding Encoding
		{
			get { return _encoding; }
		}
		public CHM_OptionDialog()
		{
			InitializeComponent();
			CultureInfo[] cis = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
			MyArrayList eds = new MyArrayList();
			foreach (CultureInfo ci in cis)
			{
				string name=Encoding.GetEncoding(ci.TextInfo.ANSICodePage).HeaderName;
				if (!eds.Exists(name)) eds.Add(name);
			}
			eds.SortItems();
			foreach (string s in eds)
			{
				_cbAllCodePages.Items.Add(s);
			}
			_cbAllCodePages.Text = Encoding.GetEncoding(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage).HeaderName;
		}
		private void button1_Click(object sender, EventArgs e)
		{
			_encoding = Encoding.GetEncoding(_cbAllCodePages.Text);
			DialogResult = DialogResult.OK;
		}
	}

	class MyArrayList : ArrayList, IComparer
	{
		int IComparer.Compare(object s1, object s2)
		{
			return string.Compare(s1.ToString(), s2.ToString(), false);
		}

		public void SortItems()
		{
			Sort(this);
		}

		public bool Exists(string val)
		{
			foreach (object s in this)
			{
				if (s.GetType() == typeof(string) &&
					string.Compare(s as string, val, true) == 0)
					return true;
			}
			return false;
		}
	}
}