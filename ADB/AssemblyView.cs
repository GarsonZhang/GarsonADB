using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ADB
{
	partial class AssemblyView : UserControl
	{
		public AssemblyView()
		{
			InitializeComponent();
		}

		ViewMode _viewMode;

		[Browsable(true)]
		[DefaultValue(ViewMode.None)]
		public ViewMode ViewMode
		{
			get { return _viewMode; }
			set 
			{ 
				_viewMode = value; 
				switch(_viewMode)
				{
				case ViewMode.TypesAndMembers:
					{
						SuspendLayout();
						AssemblyView_All tree = new AssemblyView_All();
						IAssemblyView oldView = IAssembly, newView = tree;
						tree.Dock = System.Windows.Forms.DockStyle.Fill;
						tree.Name = "AssemblyView_TypesAndMembers";
						tree.TabIndex = 0;
						Controls.Clear();
						Controls.Add(tree);
						ResumeLayout();
						if (oldView != null) newView.AddAssamblys(oldView.GetAssemblys());
						_IAssembly = tree;
						break;
					}
				case ViewMode.AssemblyOnly:
					{
						SuspendLayout();
						AssemblyView_AssemblyOnly tree = new AssemblyView_AssemblyOnly();
						IAssemblyView oldView = IAssembly, newView = tree;
						tree.Dock = System.Windows.Forms.DockStyle.Fill;
						tree.Name = "AssemblyView_AssemblyOnly";
						tree.TabIndex = 0;
						Controls.Clear();
						Controls.Add(tree);
						ResumeLayout();
						if (oldView != null) newView.AddAssamblys(oldView.GetAssemblys());
						_IAssembly = tree;
						break;
					}
				}
			}
		}

		IAssemblyView _IAssembly = null;

		internal IAssemblyView IAssembly
		{
			get { return _IAssembly; }
		}
	}

	enum ViewMode : int
	{
		None = -1,
		TypesAndMembers = 0,
		AssemblyOnly = 1
	}
}
