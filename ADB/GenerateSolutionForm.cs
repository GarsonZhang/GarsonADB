using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace ADB
{
	using Properties;
	using Factories;

	partial class GenerateSolutionForm : Form
	{
		static TextTemplate _tempCode_ExtentTag = new TextTemplate(CustomBuilderSolutionTempRes.Code_ExtendTag);
		static TextTemplate _tempCode_Common = new TextTemplate(CustomBuilderSolutionTempRes.Code_Common);
		static TextTemplate _tempCode_CustomStyle = new TextTemplate(CustomBuilderSolutionTempRes.Code_CustomStyle);
		static TextTemplate _tempReadme = new TextTemplate(CustomBuilderSolutionTempRes.Readme);
		static TextTemplate _tempConfig = new TextTemplate(CustomBuilderSolutionTempRes.Config);
		static TextTemplate _tempInfo = new TextTemplate(CustomBuilderSolutionTempRes.Info);
		static TextTemplate _tempProject = new TextTemplate(CustomBuilderSolutionTempRes.Project);
		static TextTemplate _tempProject_User = new TextTemplate(CustomBuilderSolutionTempRes.Project_User);
		static TextTemplate _tempSolution = new TextTemplate(CustomBuilderSolutionTempRes.Solution);

		CustomBuilderSolutionType _solutionType;

		internal CustomBuilderSolutionType SolutionType
		{
			get { return _solutionType; }
		}

		public GenerateSolutionForm(CustomBuilderSolutionType solutionType)
		{
			InitializeComponent();
			_solutionType = solutionType;
			switch (SolutionType)
			{
			case CustomBuilderSolutionType.ExtentSection:
				Text = Resources.strGSOText4;
				break;
			case CustomBuilderSolutionType.CustomPage:
				Text = Resources.strGSOText2;
				break;
			case CustomBuilderSolutionType.Common:
				Text = Resources.strGSOText3;
				break;
			case CustomBuilderSolutionType.ExtentTag:
				Text = Resources.strGSOText1;
				break;
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(txtName.Text)) throw new Exception(Resources.strError2);
				if (string.IsNullOrEmpty(txtNamespace.Text)) throw new Exception(Resources.strError5);
				if (string.IsNullOrEmpty(txtCustomBuilderDescption.Text)) throw new Exception(Resources.strError3);
				if (string.IsNullOrEmpty(txtPosition.Text)) throw new Exception(Resources.strError4);

				Generate();

				if (AutoOpen)
				{
					Process.Start(string.Format("{0}\\{1}\\{1}.sln", TargetDirectory, CustomBuilderName));
				}
				else
				{
					MessageBox.Show(this, Resources.strTip5, Resources.strTip);
				}
				DialogResult = DialogResult.OK;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, Resources.strTip);
			}
		}

		public string CustomBuilderName
		{
			get { return txtName.Text; }
		}

		public string CustomBuilderDescption
		{
			get { return this.txtCustomBuilderDescption.Text; }
		}

		public string Namespace
		{
			get { return txtNamespace.Text; }
		}

		public string TargetDirectory
		{
			get { return txtPosition.Text; }
		}

		public bool AutoOpen
		{
			get { return cbAutoOpen.Checked; }
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog fbd = new FolderBrowserDialog();
			fbd.Description = Resources.strTip7;
			if (fbd.ShowDialog(this) == DialogResult.OK)
			{
				txtPosition.Text = fbd.SelectedPath;
			}
		}

		private void Generate()
		{
			string slnDirectory;
			if (!TargetDirectory.EndsWith("\\") && !TargetDirectory.EndsWith("//"))
				slnDirectory = TargetDirectory + "\\" + CustomBuilderName;
			else
				slnDirectory = TargetDirectory + CustomBuilderName;

			if (Directory.Exists(slnDirectory)) throw new Exception(Resources.strError1);

			Directory.CreateDirectory(slnDirectory);

			string projectDirectory = slnDirectory + "\\" + CustomBuilderName;
			Directory.CreateDirectory(projectDirectory);
			Directory.CreateDirectory(projectDirectory + "\\bin");
			Directory.CreateDirectory(projectDirectory + "\\bin\\Release\\" + CustomBuilderName);
			Directory.CreateDirectory(projectDirectory + "\\bin\\Debug");
			Directory.CreateDirectory(projectDirectory + "\\Properties");

			Hashtable vals = new Hashtable();
			vals.Add("CustomBuilderName", CustomBuilderName);
			vals.Add("CustomBuilderDescption", CustomBuilderDescption);
			vals.Add("RootNamespace", Namespace);
			vals.Add("COMGUID", Guid.NewGuid().ToString());
			vals.Add("ProjectGUID", "{" + Guid.NewGuid().ToString() + "}");
			vals.Add("StartProgram", AppDomain.CurrentDomain.BaseDirectory + "\\ADB.exe");
			vals.Add("StartArguments", string.Format("{0}\\bin\\debug\\{1}.builder", projectDirectory, CustomBuilderName));
			vals.Add("CustomBuilderProjectGUID", "{" + Guid.NewGuid().ToString() + "}");

			_tempSolution.SaveAs(slnDirectory + "\\" + CustomBuilderName + ".sln", Encoding.UTF8, vals);
			_tempReadme.SaveAs(slnDirectory + "\\readme.txt", Encoding.UTF8, vals);

			_tempProject.SaveAs(projectDirectory + "\\" + CustomBuilderName + ".csproj", Encoding.UTF8, vals);
			_tempProject_User.SaveAs(projectDirectory + "\\" + CustomBuilderName + ".csproj.user", Encoding.UTF8, vals);
			_tempConfig.SaveAs(projectDirectory + "\\bin\\Debug\\" + CustomBuilderName + ".builder", Encoding.UTF8, vals);
			_tempConfig.SaveAs(projectDirectory + "\\bin\\Release\\" + CustomBuilderName + "\\" + CustomBuilderName + ".builder", Encoding.UTF8, vals);
			_tempInfo.SaveAs(projectDirectory + "\\Properties\\AssemblyInfo.cs", Encoding.UTF8, vals);
			switch (SolutionType)
			{
			case CustomBuilderSolutionType.ExtentSection:
			case CustomBuilderSolutionType.ExtentTag:
				{
					_tempCode_ExtentTag.SaveAs(projectDirectory + "\\" + CustomBuilderName + ".cs", Encoding.UTF8, vals);
					break;
				}
			case CustomBuilderSolutionType.CustomPage:
				{
					_tempCode_CustomStyle.SaveAs(projectDirectory + "\\" + CustomBuilderName + ".cs", Encoding.UTF8, vals);
					break;
				}
			default:
				{
					_tempCode_Common.SaveAs(projectDirectory + "\\" + CustomBuilderName + ".cs", Encoding.UTF8, vals);
					break;
				}
			}
			File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\ADB.Factories.dll", projectDirectory + "\\ADB.Factories.dll");
		}
	}
	
	enum CustomBuilderSolutionType
	{
		ExtentTag,
		Common,
		ExtentSection,
		CustomPage
	}
}