using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace ADB
{
	using Properties;
	using Factories;

	partial class AssemblyView_All : AutoSelectTreeView, IAssemblyView
	{
		private ImageList imageList1;
		private IContainer components;
		private Hashtable _asmFiles = new Hashtable();

		public AssemblyView_All()
		{
			InitializeComponent();
			imageList1.Images.Add(Resources.strPublicType, Resources.ImgType);
			imageList1.Images.Add(Resources.strNonPublicType, Resources.ImgType);
			imageList1.Images.Add(Resources.strType, Resources.ImgType);
			imageList1.Images.Add(Resources.strInheritMember, Resources.ImgMember);
			imageList1.Images.Add(Resources.strMember, Resources.ImgMember);
			imageList1.Images.Add(Resources.strEvent, Resources.ImgEvent);
			imageList1.Images.Add(Resources.strPublicProperty, Resources.ImgPublicProperty);
			imageList1.Images.Add(Resources.strPrivateProperty, Resources.ImgPrivateProperty);
			imageList1.Images.Add(Resources.strProtectedProperty, Resources.ImgProtectedProperty);
			imageList1.Images.Add(Resources.strPublicField, Resources.ImgPublicField);
			imageList1.Images.Add(Resources.strPrivateField, Resources.ImgPrivateField);
			imageList1.Images.Add(Resources.strProtectedField, Resources.ImgProtectedField);
			imageList1.Images.Add(Resources.strPublicMethod, Resources.ImgPublicMethod);
			imageList1.Images.Add(Resources.strProtectedMethod, Resources.ImgProtectedMethod);
			imageList1.Images.Add(Resources.strPrivateMethod, Resources.ImgPrivateMethod);
			imageList1.Images.Add(Resources.strAssembly, Resources.ImgAssembly);
			imageList1.Images.Add(Resources.strClass, Resources.ImgClass);
			imageList1.Images.Add(Resources.strInterface, Resources.ImgInterface);
			imageList1.Images.Add(Resources.strEnumeration, Resources.ImgEnum);
			imageList1.Images.Add(Resources.strDelegate, Resources.ImgDelegate);
			imageList1.Images.Add(Resources.strStructure, Resources.ImgStructure);
		}

		private void AddAssemblyObjects(Assembly asmFile)
		{
			TreeNode node = null;
			TreeNode asmNode = new TreeNode(asmFile.FullName);
			asmNode.Name = asmFile.FullName;
			asmNode.ImageKey = Resources.strAssembly;
			asmNode.SelectedImageKey = Resources.strAssembly;
			asmNode.Tag = asmFile.Location.ToUpper();

			node = new TreeNode(Resources.strPublicType);
			node.Name = Resources.strPublicType;
			node.ImageKey = Resources.strPublicType;
			node.SelectedImageKey = Resources.strPublicType;
			asmNode.Nodes.Add(node);

			node = new TreeNode(Resources.strNonPublicType);
			node.Name = Resources.strNonPublicType;
			node.ImageKey = Resources.strNonPublicType;
			node.SelectedImageKey = Resources.strNonPublicType;
			asmNode.Nodes.Add(node);

			AddTypeNodes(asmFile, asmNode);

			this.Nodes.Add(asmNode);
		}

		private void AddTypeNodes(Assembly asmFile, TreeNode asmNodes)
		{
			Type[] types = asmFile.GetTypes();
			TreeNode node = null;
			int i = 0;
			foreach (Type type in types)
			{
				node = new TreeNode(type.Name);
				if (type.IsClass)
					node.ImageKey = Resources.strClass;
				else if (type.IsValueType)
					node.ImageKey = Resources.strStructure;
				else if (type.IsEnum)
					node.ImageKey = Resources.strEnumeration;
				else if (type.IsInterface)
					node.ImageKey = Resources.strInterface;
				else
					node.ImageKey = Resources.strDelegate;
				node.SelectedImageKey = node.ImageKey;
				node.ToolTipText = type.FullName;
				node.Tag = type;
				AddClassMembers(node);
				if (type.IsPublic || type.IsNestedPublic)
					asmNodes.Nodes[Resources.strPublicType].Nodes.Add(node);
				else
					asmNodes.Nodes[Resources.strNonPublicType].Nodes.Add(node);
				i++;
			}
		}

		private void AddClassMembers(TreeNode classNode)
		{
			Type type = classNode.Tag as Type;
			foreach (MemberInfo member in type.GetMembers(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				string nodePath;
				if (member.DeclaringType == type)
					nodePath = Resources.strMember;
				else
					nodePath = Resources.strInheritMember;

				#region 添加成员到列表中

				switch (member.MemberType)
				{
				case MemberTypes.Constructor:
					{
						ConstructorInfo ctor = member as ConstructorInfo;
						if (ctor.IsPublic)
						{
							nodePath += "\\" + Resources.strPublicMethod;
						}
						else if (ctor.IsPrivate)
						{
							nodePath += "\\" + Resources.strPrivateMethod;
						}
						else
						{
							nodePath += "\\" + Resources.strProtectedMethod;
						}

						TreeNode node = new TreeNode(member.Name);
						node.Tag = member;
						AddMemberNode(classNode, node, nodePath);
						break;
					}
				case MemberTypes.Method:
					{
						MethodInfo method = member as MethodInfo;
						if (!method.IsSpecialName)
						{
							if (method.IsPublic)
							{
								nodePath += "\\" + Resources.strPublicMethod;
							}
							else if (method.IsPrivate)
							{
								nodePath += "\\" + Resources.strPrivateMethod;
							}
							else
							{
								nodePath += "\\" + Resources.strProtectedMethod;
							}

							TreeNode node = new TreeNode(member.Name);
							node.Tag = member;
							AddMemberNode(classNode, node, nodePath);
						}
						break;
					}
				case MemberTypes.Property:
					{
						PropertyInfo property = member as PropertyInfo;
						MethodAttributes attr = 0;
						MethodInfo getMethod, setMethod;
						getMethod = property.GetGetMethod(true);
						setMethod = property.GetSetMethod(true);
						if (getMethod != null)
							attr = getMethod.Attributes;
						else if (setMethod != null)
							attr = setMethod.Attributes;
						else
							attr = 0;
						if (attr != 0)
						{
							if ((attr & MethodAttributes.Public) == MethodAttributes.Public)
							{
								nodePath += "\\" + Resources.strPublicProperty;
							}
							else if ((attr & MethodAttributes.Private) == MethodAttributes.Private)
							{
								nodePath += "\\" + Resources.strPrivateProperty;
							}
							else
							{
								nodePath += "\\" + Resources.strProtectedProperty;
							}
							TreeNode node = new TreeNode(member.Name);
							node.Tag = member;
							AddMemberNode(classNode, node, nodePath);
						}
						break;
					}
				case MemberTypes.Field:
					{
						FieldInfo field = member as FieldInfo;

						if (field.IsPublic)
						{
							nodePath += "\\" + Resources.strPublicField;
						}
						else if (field.IsPrivate)
						{
							nodePath += "\\" + Resources.strPrivateField;
						}
						else
						{
							nodePath += "\\" + Resources.strProtectedField;
						}

						TreeNode node = new TreeNode(member.Name);
						node.Tag = member;
						AddMemberNode(classNode, node, nodePath);
						break;
					}
				case MemberTypes.Event:
					{
						nodePath += "\\" + Resources.strEvent;

						TreeNode node = new TreeNode(member.Name);
						node.Tag = member;
						AddMemberNode(classNode, node, nodePath);
						break;
					}
				}

				#endregion
			}
		}

		private void AddMemberNode(TreeNode classNode, TreeNode node, string subNodePath)
		{
			string[] pathNode = subNodePath.Split('\\');
			TreeNode pNode = classNode;
			for (int i = 0; i < pathNode.Length; i++)
			{
				if (pNode.Nodes[pathNode[i]] == null)
				{
					for (; i < pathNode.Length; i++)
					{
						TreeNode newNode = new TreeNode(pathNode[i]);
						newNode.ImageKey = pathNode[i];
						newNode.SelectedImageKey = pathNode[i];
						newNode.Name = pathNode[i];
						pNode.Nodes.Add(newNode);
						pNode = newNode;
					}
					break;
				}
				else
					pNode = pNode.Nodes[pathNode[i]];
			}
			node.ImageKey = pNode.ImageKey;
			node.SelectedImageKey = pNode.SelectedImageKey;
			pNode.Nodes.Add(node);
		}

		public void QuickSelect()
		{
			SelectForm quickSelectForm = new SelectForm();
			if (quickSelectForm.ShowDialog(this) == DialogResult.OK)
			{
				foreach (TreeNode rootNode in this.Nodes)
					rootNode.Checked = false;

				foreach (TreeNode rootNode in quickSelectForm.Nodes[0].Nodes)
					foreach (TreeNode classSubNode in rootNode.Nodes[0].Nodes)
						foreach (TreeNode memberNode in classSubNode.Nodes)
						{
							foreach (TreeNode asmNode in this.Nodes)
							{
								if (asmNode.Nodes[rootNode.Text] != null)
								{
									foreach (TreeNode classNode in asmNode.Nodes[rootNode.Text].Nodes)
									{
										TreeNode node = classNode.Nodes[classSubNode.Text];
										if (node != null)
										{
											node = node.Nodes[memberNode.Text];
											if (node != null) node.Checked = memberNode.Checked;
										}
									}
								}
							}
						}
			}
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// AssemblyView_TypesAndMembers
			// 
			this.ImageIndex = 0;
			this.ImageList = this.imageList1;
			this.ItemHeight = 17;
			this.SelectedImageIndex = 0;
			this.ResumeLayout(false);

		}

		#region 实现IAssemblyView
		bool IAssemblyView.Exists(string path)
		{
			return _asmFiles.ContainsKey(path.ToUpper());
		}

		void IAssemblyView.AddAssambly(Assembly assembly)
		{
			if (!_asmFiles.ContainsKey(assembly.Location.ToUpper()))
			{
				AddAssemblyObjects(assembly);
				_asmFiles.Add(assembly.Location.ToUpper(), assembly);
			}
		}

		void IAssemblyView.AddAssamblys(Assembly[] assemblys)
		{
			foreach (Assembly assembly in assemblys)
			{
				if (!_asmFiles.ContainsKey(assembly.Location.ToUpper()))
				{
					AddAssemblyObjects(assembly);
					_asmFiles.Add(assembly.Location.ToUpper(), assembly);
				}
			}
		}

		void IAssemblyView.Clear()
		{
			_asmFiles.Clear();
			Nodes.Clear();
		}

		DocumentBuilderMember[] IAssemblyView.GetTypes()
		{
			List<DocumentBuilderMember> types = new List<DocumentBuilderMember>();
			int i = 0;
			foreach (TreeNode asmNode in this.Nodes)
			{
				foreach (TreeNode asmSubNode in asmNode.Nodes)
				{
					foreach (TreeNode node in asmSubNode.Nodes)
					{
						types.Add(new DocumentBuilderMember(node.Checked, (MemberInfo)node.Tag));
						i++;
					}
				}
			}
			return types.ToArray();
		}

		Assembly[] IAssemblyView.GetAssemblys()
		{
			Assembly[] asms = new Assembly[_asmFiles.Count];
			int i = 0;
			foreach (DictionaryEntry ent in _asmFiles)
			{
				asms[i] = ent.Value as Assembly;
				i++;
			}
			return asms;
		}

		DocumentBuilderMember[] IAssemblyView.GetTypeMembers(Type type)
		{
			List<DocumentBuilderMember> mems = new List<DocumentBuilderMember>();
			TreeNode typeNode = null;
			foreach (TreeNode asmNode in this.Nodes)
			{
				foreach (TreeNode asmSubNode in asmNode.Nodes)
				{
					foreach (TreeNode classNode in asmSubNode.Nodes)
					{
						if (classNode.Tag == type)
						{
							typeNode = classNode;
							break;
						}
					}
					if (typeNode != null) break;
				}
				if (typeNode != null) break;
			}
			foreach (TreeNode classNode in typeNode.Nodes)
			{
				foreach (TreeNode classSubNode in classNode.Nodes)
				{
					foreach (TreeNode memberNode in classSubNode.Nodes)
					{
						mems.Add(new DocumentBuilderMember(memberNode.Checked, memberNode.Tag as MemberInfo));
					}
				}
			}
			DocumentBuilderMember[] ret = new DocumentBuilderMember[mems.Count];
			mems.CopyTo(ret);
			return ret;
		}

		Type[] IAssemblyView.GetSelectedTypes()
		{
			List<Type> types = new List<Type>();
			foreach (TreeNode asmNode in this.Nodes)
			{
				foreach (TreeNode asmSubNode in asmNode.Nodes)
				{
					foreach (TreeNode node in asmSubNode.Nodes)
					{
						if (node.Checked) types.Add(node.Tag as Type);
					}
				}
			}
			return types.ToArray();
		}

		MemberInfo[] IAssemblyView.GetTypeSelectedMembers(Type type)
		{
			List<MemberInfo> mems = new List<MemberInfo>();
			TreeNode typeNode = null;
			foreach (TreeNode asmNode in this.Nodes)
			{
				foreach (TreeNode asmSubNode in asmNode.Nodes)
				{
					foreach (TreeNode classNode in asmSubNode.Nodes)
					{
						if (classNode.Tag == type)
						{
							typeNode = classNode;
							break;
						}
					}
					if (typeNode != null) break;
				}
				if (typeNode != null) break;
			}
			foreach (TreeNode classNode in typeNode.Nodes)
			{
				foreach (TreeNode classSubNode in classNode.Nodes)
				{
					foreach (TreeNode memberNode in classSubNode.Nodes)
					{
						if (memberNode.Checked)
							mems.Add(memberNode.Tag as MemberInfo);
					}
				}

			}
			return mems.ToArray();
		}

		void IAssemblyView.DeleteSelectedAssembly()
		{
			if (this.SelectedNode != null && this.SelectedNode.Parent == null &&
				MessageBox.Show(this, Resources.String19 + "\r\n" + this.SelectedNode.Text + "?", Resources.String20, MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				_asmFiles.Remove(SelectedNode.Tag);
				this.Nodes.Remove(this.SelectedNode);
				if (this.Nodes.Count > 0) this.SelectedNode = this.Nodes[0];
			}
		}
		#endregion
	}
}
