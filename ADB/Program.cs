using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace ADB
{
    using Properties;
    static class Program
    {
		static string _debugBuilder;
		public static Form MainDialog = null;

        public static string DebugBuilder
        {
            get { return Program._debugBuilder; }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (args.Length > 0) _debugBuilder = args[0]; else _debugBuilder = string.Empty;
			if (Settings.Default.AutoUpdate)
			{
				try
				{
					Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Update.exe", "-silent");
				}
				catch (Exception)
				{
				}
			}
			LoadSetting();
			Application.Run(MainDialog);
			SaveSetting();
		}

		static void LoadSetting()
        {
            if (string.Compare(Settings.Default.Language, "default", true) != 0)
            {
               CultureInfo ci = new CultureInfo(Settings.Default.Language);
                if (string.Compare(ci.Parent.Name, "zh-CHS", true) == 0 || string.Compare(ci.Parent.Name, "en", true) == 0)
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                }
                else
                {
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                }
            }
            else
            {
                if (string.Compare(Thread.CurrentThread.CurrentUICulture.Parent.Name, "zh-CHS", true) != 0
                    && string.Compare(Thread.CurrentThread.CurrentUICulture.Parent.Name, "en", true)!=0)
                {
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                }
            }
			MainDialog = new MainForm();
			MainDialog.Width = Settings.Default.Width;
			MainDialog.Height = Settings.Default.Height;

		}

		static void SaveSetting()
		{
			if (MainDialog.WindowState == FormWindowState.Normal)
			{
				Settings.Default.Width = MainDialog.Width;
				Settings.Default.Height = MainDialog.Height;
			}
			Settings.Default.Save();
		}
    }
}