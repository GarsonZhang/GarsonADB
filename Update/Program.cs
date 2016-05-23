using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Diagnostics;

namespace Update
{
    using Properties;

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool silent = (args.Length > 0 && string.Compare("-silent", args[0]) == 0) ? true : false;
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                XmlDocument updateServerFile = new XmlDocument();
                updateServerFile.Load(AppDomain.CurrentDomain.BaseDirectory + "UpdateConfig.xml");
                XmlNodeList serverNodes = updateServerFile.GetElementsByTagName("Update");
                string location = null, config = null;
                if (serverNodes.Count > 0 && serverNodes[0].NodeType == XmlNodeType.Element)
                {
                    location = (serverNodes[0] as XmlElement).GetAttribute("Location");
                    config = (serverNodes[0] as XmlElement).GetAttribute("Config");
                }
                if (string.IsNullOrEmpty(location) || string.IsNullOrEmpty(config))
                {
                    return;
                }

                string updataConfigPath = AppDomain.CurrentDomain.BaseDirectory + "latest.xml";
                string adbFileName = AppDomain.CurrentDomain.BaseDirectory + "ADB.exe";
                string updateConfigURL = location + "/" + config;

                WebClient client = new WebClient();
                client.DownloadFile(updateConfigURL, updataConfigPath);
                XmlDocument updataConfig = new XmlDocument();
                updataConfig.Load(updataConfigPath);
                AssemblyName adb = AssemblyName.GetAssemblyName(adbFileName);
                Version currentVersion = adb.Version;
                XmlElement ADBNode = updataConfig.GetElementsByTagName("Update")[0] as XmlElement;
                Version latestVersion = new Version(ADBNode.GetAttribute("Latest"));
                if (currentVersion < latestVersion)
                {
                    if (MessageBox.Show(Resources.strTip2, Resources.strTip, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        UpdateProgress updataProgress = new UpdateProgress(
                            ADBNode.GetAttribute("URL"),
                            AppDomain.CurrentDomain.BaseDirectory + "update.zip"
                            );
                        if (updataProgress.ShowDialog() == DialogResult.OK)
                        {
                            bool autoUpdate = true;
                            while (!DeleteFile(adbFileName))
                            {
                                if (MessageBox.Show(Resources.strTip3, Resources.strTip, MessageBoxButtons.OKCancel)
                                    == DialogResult.Cancel)
                                {
                                    autoUpdate = false;
                                    break;
                                }
                            }
                            if (autoUpdate)
                            {
                                CompressUtility.UnZip(
                                    AppDomain.CurrentDomain.BaseDirectory + "update.zip",
                                    AppDomain.CurrentDomain.BaseDirectory
                                 );
                                DeleteFile(AppDomain.CurrentDomain.BaseDirectory + "update.zip");

                                string extent = ADBNode.GetAttribute("Extent");
                                if (!string.IsNullOrEmpty(extent))
                                {
                                    Process extProcess = Process.Start(AppDomain.CurrentDomain.BaseDirectory + extent);
                                    extProcess.WaitForExit();
                                }
                                if (MessageBox.Show(Resources.strTip5, Resources.strTip, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    Process.Start(adbFileName);
                                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Update.txt"))
                                    Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Update.txt");
                            }
                        }
                    }
                }
                else
                {
                    if (!silent) MessageBox.Show(Resources.strTip1, Resources.strTip);
                }
            }
            catch (Exception ex)
            {
                if (!silent) MessageBox.Show(ex.Message, Resources.strError);
            }
        }

        public static bool DeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
    static class CompressUtility
    {
        public static void UnZip(string fileName, string target)
        {
            if (!target.EndsWith("\\")) target += "\\";
            using (ZipInputStream inputStream = new ZipInputStream(File.OpenRead(fileName)))
            {
                try
                {
                    ZipEntry theEntry;
                    while ((theEntry = inputStream.GetNextEntry()) != null)
                    {
                        if (theEntry.IsDirectory)
                        {
                            Directory.CreateDirectory(target + theEntry.Name);
                        }
                        else if (theEntry.IsFile)
                        {
                            using (FileStream fileWrite = new FileStream(target + theEntry.Name, FileMode.Create, FileAccess.Write))
                            {
                                try
                                {
                                    byte[] buffer = new byte[2048];
                                    int size;
                                    while (true)
                                    {
                                        size = inputStream.Read(buffer, 0, buffer.Length);
                                        if (size > 0)
                                            fileWrite.Write(buffer, 0, size);
                                        else
                                            break;
                                    }
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                finally
                                {
                                    fileWrite.Close();
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    inputStream.Close();
                }
            }
        }
   
    }
}