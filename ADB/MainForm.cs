using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace ADB
{
    using Properties;
    using Factories;

	partial class MainForm : Form, IGetData, IInteract
	{
		#region 初始化
		static MainForm()
		{
			xmlSearchPath = new string[3];
			xmlSearchPath[0] = AppDomain.CurrentDomain.BaseDirectory;
			string windir = Environment.GetEnvironmentVariable("windir");
			xmlSearchPath[1] = string.Format(
				"{0}\\Microsoft.Net\\Framework\\v{1}.{2}.{3}\\{4}",
				windir,
				Environment.Version.Major,
				Environment.Version.Minor,
				Environment.Version.Build,
				Thread.CurrentThread.CurrentCulture.Name
			);
			xmlSearchPath[2] = string.Format(
				"{0}\\Microsoft.Net\\Framework\\v{1}.{2}.{3}",
				windir,
				Environment.Version.Major,
				Environment.Version.Minor,
				Environment.Version.Build
			);
			_supportBuilderVersion.Add("2.3.0.0", "2.3.0.0");
		}

		public MainForm()
		{
			InitializeComponent();
			assemblyView.ViewMode = (ViewMode)Settings.Default.AssemblyViewMode;
			_cbShowMembers.Checked =((ViewMode)Settings.Default.AssemblyViewMode == ViewMode.TypesAndMembers);
			foreach (ToolStripMenuItem item in menuLanguage.DropDownItems)
			{
				if (string.Compare(item.Tag as string, System.Threading.Thread.CurrentThread.CurrentUICulture.Name) == 0)
					item.Checked = true;
			}

			toolStripMenuItem5.Checked = Settings.Default.AutoUpdate;
			LoadBuilders();
			if (_cbBuilderType.Items.Count > 0) _cbBuilderType.SelectedIndex = 0;
		}
		#endregion

		#region 文档生成器
		static private List<BuilderInfo> Builders = new List<BuilderInfo>();
		private static Hashtable _supportBuilderVersion = new Hashtable();
		readonly string CustomBuildersPath = AppDomain.CurrentDomain.BaseDirectory + "\\builders";

		private void LoadBuilders()
		{

			if (string.IsNullOrEmpty(Program.DebugBuilder))
			{
				//加载自定义生成器
				StringBuilder unsupportVersion = new StringBuilder();
				string[] builderDiretories = Directory.GetDirectories(CustomBuildersPath);
				foreach (string builderDirectory in builderDiretories)
				{
					string[] buildersConfigs = Directory.GetFiles(builderDirectory, "*.builder");
					foreach (string cbf in buildersConfigs)
					{
						BuilderInfo[] builderInfos = GetBuilderInfos(cbf);
						foreach (BuilderInfo builderInfo in builderInfos)
						{
							if (_supportBuilderVersion.ContainsKey(builderInfo.ADBVersion))
								Builders.Add(builderInfo);
							else
							{
								unsupportVersion.AppendFormat("{0}[{1}]", builderInfo.Name, builderInfo.ADBVersion);
								unsupportVersion.Append("\r\n");
							}
						}
					}
				}
				//if (unsupportVersion.Length > 0)
				//{
				//    MessageBox.Show(Resources.strTip3 + "\r\n\r\n" + unsupportVersion, Resources.strTip);
				//}
			}
			else
			{
				BuilderInfo[] builderInfos = GetBuilderInfos(Program.DebugBuilder);
				if (_supportBuilderVersion.ContainsKey(builderInfos[0].ADBVersion))
				{
					foreach (BuilderInfo builderInfo in builderInfos) Builders.Add(builderInfo);
					Text += "——调试自定义文档生成器";
				}
				else
				{
					MessageBox.Show(Resources.strTip4, Resources.strTip);
				}
			}
			for (int i = 0; i < Builders.Count; i++)
			{
				_cbBuilderType.Items.Add(new ListItemWithTag(Builders[i].Name, i));
			}
		}

		private static BuilderInfo[] GetBuilderInfos(string builderConfigFileName)
		{
			string fileName = Path.GetFileNameWithoutExtension(builderConfigFileName);
			string assemblyFile = Path.GetDirectoryName(builderConfigFileName) + "\\" + fileName + ".dll";
			if (File.Exists(assemblyFile))
			{
				List<BuilderInfo> infos = new List<BuilderInfo>();
				XmlDocument config = new XmlDocument();
				string name, entry, adbVersion, localizable;
				config.Load(builderConfigFileName);
				XmlNodeList xnl = config.GetElementsByTagName("Builders");
				if (xnl.Count > 0 && xnl[0].NodeType == XmlNodeType.Element)
				{
					XmlElement builders = xnl[0] as XmlElement;
					adbVersion = builders.GetAttribute("ADBVersion");
					if (string.IsNullOrEmpty(adbVersion)) adbVersion = "2.0.0.0";
					foreach (XmlElement customBuilder in builders.GetElementsByTagName("CustomBuilder"))
					{
						if (customBuilder.NodeType == XmlNodeType.Element)
						{
							entry = customBuilder.GetAttribute("Entry");
							localizable = customBuilder.GetAttribute("Localizable");
							if (string.IsNullOrEmpty(localizable)) localizable = "false";
							if (string.Compare(localizable, "true", true) == 0)
							{
								XmlNodeList profileElements = customBuilder.GetElementsByTagName("Profile");
								XmlElement profileElement = profileElements[0] as XmlElement;
								foreach (XmlElement profile in profileElements)
								{
									if (string.Compare(profile.GetAttribute("Language"), Thread.CurrentThread.CurrentUICulture.Parent.Name, true) == 0) profileElement = profile;
								}
								name = profileElement.GetAttribute("Name");
							}
							else
							{
								name = customBuilder.GetAttribute("Name");
							}
							BuilderInfo builderInfo = new BuilderInfo(name, entry, builderConfigFileName, assemblyFile, adbVersion);
							infos.Add(builderInfo);
						}
					}
					return infos.ToArray();
				}
				else
					return null;
			}
			else
				return null;
		}
		#endregion

		#region 程序集和XML文档

		readonly static string[] xmlSearchPath;

		private void AddAssambly(string fileName)
		{
			if (!IAsmView.Exists(fileName))
			{
				Assembly asmFile;
				asmFile = Assembly.LoadFrom(fileName);
				IAsmView.AddAssambly(asmFile);

				string asmXmlFile = SearchXml(
					Path.GetFileNameWithoutExtension(fileName) + ".xml",
					Path.GetDirectoryName(fileName)
					);
				if (!string.IsNullOrEmpty(asmXmlFile) &&
					!_lbXMLList.Exists(asmXmlFile))
				{
					AddXML(asmXmlFile);
				}
				else
				{
					MessageBox.Show(this, string.Format(Resources.strTip_NoXML, Path.GetFileName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".xml"), Resources.strTip);
				}

				AssemblyName[] referencedAssemblies;
				referencedAssemblies = asmFile.GetReferencedAssemblies();
				foreach (AssemblyName asm in referencedAssemblies)
				{
					string xmlFile = SearchXml(
						asm.Name + ".xml",
						Path.GetDirectoryName(fileName)
						);
					if (!string.IsNullOrEmpty(xmlFile) &&
						!_lbXMLList.Exists(xmlFile))
					{
						AddXML(xmlFile);
					}
				}
			}
		}

		private void AddXML(string fileName)
		{
			_lbXMLList.Add(fileName);
		}

		private string SearchXml(string xmlFileName, string asmFileDir)
		{
			string filePath = asmFileDir + "\\" + xmlFileName;
			if (File.Exists(filePath)) return filePath;
			foreach (string sp in xmlSearchPath)
			{
				filePath = sp + "\\" + xmlFileName;
				if (File.Exists(filePath)) return filePath;
			}
			return null;
		}

		private void _btnSelect_Click(object sender, EventArgs e)
		{
			IAsmView.QuickSelect();
		}

		private void _btnAddAssambly_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = Resources.String17;
			open.Multiselect = true;
			if (open.ShowDialog() == DialogResult.OK)
			{
				foreach (string fileName in open.FileNames)
				{
					try
					{
						AddAssambly(fileName);
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, ex.Message, Resources.strError);
					}
				}
			}
		}

		private void _btnDeleteAssembly_Click(object sender, EventArgs e)
		{
			IAsmView.DeleteSelectedAssembly();
		}

		private void _btnAddXML_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = Resources.String18;
			open.Multiselect = true;
			if (open.ShowDialog() == DialogResult.OK)
			{
				foreach (string fileName in open.FileNames)
				{
					try
					{
						AddXML(open.FileName);
					}
					catch (Exception ex)
					{
						MessageBox.Show(this, ex.Message, Resources.strError);
					}
				}
			}
		}

		private void _btnDeleteXMLFile_Click(object sender, EventArgs e)
		{
			_lbXMLList.DeleteSelectItem();
		}
		#endregion

		#region 实现IInteract
		Object IInteract.Invoke(Delegate method)
		{
			return this.Invoke(method);
		}

		Object IInteract.Invoke(Delegate method, params object[] args)
		{
			return this.Invoke(method, args);
		}

		IAsyncResult IInteract.BeginInvoke(Delegate method)
		{
			return this.BeginInvoke(method);
		}

		IAsyncResult IInteract.BeginInvoke(Delegate method, params object[] args)
		{
			return this.BeginInvoke(method, args);
		}

		Form IInteract.MainForm
		{
			get
			{
				return this;
			}
		}
		#endregion

		#region 实现IGetData

		MemberXmlElement IGetData.GetMemberXmlNode(MemberInfo memInfo)
		{
			return _lbXMLList.GetMemberXmlNode(memInfo);
		}

		DocumentBuilderMember[] IGetData.GetTypes()
		{
			return IAsmView.GetTypes();
		}

		DocumentBuilderMember[] IGetData.GetTypeMembers(Type type)
		{
			return IAsmView.GetTypeMembers(type);
		}

		Type[] IGetData.GetSelectedTypes()
		{
			return IAsmView.GetSelectedTypes();
		}

		MemberInfo[] IGetData.GetTypeSelectedMembers(Type type)
		{
			return IAsmView.GetTypeSelectedMembers(type);
		}

		#endregion

		#region 菜单事件处理
		private void menuExtendTag_Click(object sender, EventArgs e)
		{
			GenerateSolutionForm gso = new GenerateSolutionForm(CustomBuilderSolutionType.ExtentTag);
			gso.ShowDialog(this);
		}

		private void menuCustomBuilder_Click(object sender, EventArgs e)
		{
			GenerateSolutionForm gso = new GenerateSolutionForm(CustomBuilderSolutionType.Common);
			gso.ShowDialog(this);
		}

		private void menuCustomStyle_Click(object sender, EventArgs e)
		{
			GenerateSolutionForm gso = new GenerateSolutionForm(CustomBuilderSolutionType.CustomPage);
			gso.ShowDialog(this);
		}

		private void menuExtendSection_Click(object sender, EventArgs e)
		{
			GenerateSolutionForm gso = new GenerateSolutionForm(CustomBuilderSolutionType.ExtentSection);
			gso.ShowDialog(this);
		}

		private void menuLanguage_Click(object sender, EventArgs e)
		{
			Settings.Default.Language = (sender as ToolStripMenuItem).Tag as string;
			MessageBox.Show(this, Resources.String49, Resources.strTip);
		}

		private void menuAbout_Click(object sender, EventArgs e)
		{
			AboutBox about = new AboutBox();
			about.ShowDialog(this);
		}

		private void toolStripMenuItem3_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void toolStripMenuItem4_Click(object sender, EventArgs e)
		{
			Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Update.exe");
		}

		private void toolStripMenuItem5_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.AutoUpdate = toolStripMenuItem5.Checked;
		}

		private void menuADBHelp_Click(object sender, EventArgs e)
		{
			Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\help.chm");
		}
		#endregion

		#region 生成文档
		private void _btnBuild_Click(object sender, EventArgs e)
		{
			try
			{
				using (SaveFileDialog save = new SaveFileDialog())
				{
					if (string.IsNullOrEmpty(_tbTitle.Text.Trim())) throw new Exception(Resources.String51);
					DocumentBuilder builder = null;
					BuilderInfo info = Builders[(int)(_cbBuilderType.SelectedItem as ListItemWithTag).Tag] as BuilderInfo;
					Assembly assembly = Assembly.LoadFrom(info.AssemblyFile);
					Type builderType = assembly.GetType(info.Entry);
					ConstructorInfo ctor = builderType.GetConstructor(new Type[] { typeof(IGetData), typeof(IInteract) });
					builder = ctor.Invoke(new object[] { this, this }) as DocumentBuilder;
					save.Filter = builder.Filter;
					save.AddExtension = true;
					if (save.ShowDialog(this) == DialogResult.OK)
					{
						if (builder.OptionDialog.ShowDialog(this) == DialogResult.OK)
						{
							Thread thread = new Thread(new ParameterizedThreadStart(Build));
							thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
							thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
							thread.Start(new BuildParameters(builder, _tbTitle.Text, save.FileName));
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, Resources.strError);
			}
		}

		private void Build(object param)
		{
			this.BeginInvoke(new BeforeBuildDelegate(this.BeforeBuild));
			BuildParameters buildParams = param as BuildParameters;
			try
			{
				buildParams.Builder.Build(buildParams.Title, buildParams.FileName);
			}
			catch (Exception ex)
			{
				BeginInvoke(new ShowExceptionDelegate(this.ShowException), this, "", ex);
			}
			this.BeginInvoke(new AfterBuildDelegate(this.AfterBuild));
		}

		delegate void BeforeBuildDelegate();

		private void BeforeBuild()
		{
			this.panel1.Enabled = false;
		}

		delegate void AfterBuildDelegate();

		private void AfterBuild()
		{
			this.panel1.Enabled = true;
		}

		private delegate void ShowExceptionDelegate(IWin32Window owner, string caption, Exception ex);

		private void ShowException(IWin32Window owner, string caption, Exception ex)
		{
			MessageBox.Show(owner, ex.Message, caption);
		}
		#endregion

		private void _cbShowMembers_CheckedChanged(object sender, EventArgs e)
		{
			this._btnBuild.Enabled = false;
			this._btnAddAssambly.Enabled = false;
			this._btnDeleteAssembly.Enabled = false;
			this._btnSelect.Enabled = false;
			this._cbShowMembers.Enabled = false;
			try
			{
				if (_cbShowMembers.Checked)
					assemblyView.ViewMode = ViewMode.TypesAndMembers;
				else
					assemblyView.ViewMode = ViewMode.AssemblyOnly;
				Settings.Default.AssemblyViewMode = (int)assemblyView.ViewMode;
			}
			finally
			{
				this._btnBuild.Enabled = true;
				this._btnAddAssambly.Enabled = true;
				this._btnDeleteAssembly.Enabled = true;
				this._btnSelect.Enabled = true;
				this._cbShowMembers.Enabled = true;
			}
		}

		public IAssemblyView IAsmView
		{
			get { return assemblyView.IAssembly; }
		}
	}

    class BuildParameters
    {
        private DocumentBuilder _builder;
        private string _title, _fileName;

        public DocumentBuilder Builder
        {
            get { return _builder; }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string Title
        {
            get { return _title; }
        }

        public BuildParameters(DocumentBuilder builder, string title, string fileName)
        {
            _builder = builder;
            _title = title;
            _fileName = fileName;
        }
    }
}