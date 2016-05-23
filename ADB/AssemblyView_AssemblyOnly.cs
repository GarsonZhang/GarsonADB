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
	class AssemblyView_AssemblyOnly : AutoSelectTreeView, IAssemblyView
	{
		private ImageList imageList1;
		private IContainer components;
		private Hashtable _asmFiles = new Hashtable();

		public AssemblyView_AssemblyOnly()
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
			TreeNode asmNode = new AssemblyView_AssemblyOnly_TreeNode(asmFile);

			node = new TreeNode(Resources.strPublicType);
			node.Name = Resources.strPublicType;
			node.ImageKey = Resources.strPublicType;
			node.SelectedImageKey = Resources.strPublicType;
			AddSubNodes(node);
			asmNode.Nodes.Add(node);

			node = new TreeNode(Resources.strNonPublicType);
			node.Name = Resources.strNonPublicType;
			node.ImageKey = Resources.strNonPublicType;
			node.SelectedImageKey = Resources.strNonPublicType;
			AddSubNodes(node);
			asmNode.Nodes.Add(node);
			this.Nodes.Add(asmNode);
		}

		void AddSubNodes(TreeNode node)
		{
			TreeNode classNode = node.Nodes.Add("", Resources.String22, Resources.strClass, Resources.strClass);
			classNode.Nodes.Add(Resources.strMember, Resources.strMember, Resources.strMember, Resources.strMember);
			classNode.Nodes.Add(Resources.strInheritMember, Resources.strInheritMember, Resources.strInheritMember, Resources.strInheritMember);
			AddMemberCategories(classNode.Nodes[0]);
			AddMemberCategories(classNode.Nodes[1]);
		}

		void AddMemberCategories(TreeNode node)
		{
			node.Nodes.Add(Resources.strPublicMethod, Resources.strPublicMethod, Resources.strPublicMethod, Resources.strPublicMethod);
			node.Nodes.Add(Resources.strProtectedMethod, Resources.strProtectedMethod, Resources.strProtectedMethod, Resources.strProtectedMethod);
			node.Nodes.Add(Resources.strPrivateMethod, Resources.strPrivateMethod, Resources.strPrivateMethod, Resources.strPrivateMethod);
			node.Nodes.Add(Resources.strPublicProperty, Resources.strPublicProperty, Resources.strPublicProperty, Resources.strPublicProperty);
			node.Nodes.Add(Resources.strProtectedProperty, Resources.strProtectedProperty, Resources.strProtectedProperty, Resources.strProtectedProperty);
			node.Nodes.Add(Resources.strPrivateProperty, Resources.strPrivateProperty, Resources.strPrivateProperty, Resources.strPrivateProperty);
			node.Nodes.Add(Resources.strPublicField, Resources.strPublicField, Resources.strPublicField, Resources.strPublicField);
			node.Nodes.Add(Resources.strProtectedField, Resources.strProtectedField, Resources.strProtectedField, Resources.strProtectedField);
			node.Nodes.Add(Resources.strPrivateField, Resources.strPrivateField, Resources.strPrivateField, Resources.strPrivateField);
			node.Nodes.Add(Resources.strEvent, Resources.strEvent, Resources.strEvent, Resources.strEvent);
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
			// AssemblyView_AssemblyOnly
			// 
			this.CheckBoxes = true;
			this.ImageIndex = 0;
			this.ImageList = this.imageList1;
			this.ItemHeight = 17;
			this.SelectedImageIndex = 0;
			this.ResumeLayout(false);

		}

		public static MemberTree GetAllMembers(Type type)
		{
			MemberTree tree=new MemberTree();
			foreach (MemberInfo member in type.GetMembers(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				string cate1=null;

				if (member.DeclaringType == type)
					cate1=Resources.strMember;
				else
					cate1=Resources.strInheritMember;

				#region 添加成员到列表中

				switch (member.MemberType)
				{
				case MemberTypes.Constructor:
					{
						ConstructorInfo ctor = member as ConstructorInfo;
						if (ctor.IsPublic)
						{
							tree.Add(cate1, Resources.strPublicMethod, member);
						}
						else if (ctor.IsPrivate)
						{
							tree.Add(cate1, Resources.strPrivateMethod, member);
						}
						else
						{
							tree.Add(cate1, Resources.strProtectedMethod, member);
						}
						break;
					}
				case MemberTypes.Method:
					{
						MethodInfo method = member as MethodInfo;
						if (!method.IsSpecialName)
						{
							if (method.IsPublic)
							{
								tree.Add(cate1, Resources.strPublicMethod, member);
							}
							else if (method.IsPrivate)
							{
								tree.Add(cate1, Resources.strPrivateMethod, member);
							}
							else
							{
								tree.Add(cate1, Resources.strProtectedMethod, member);
							}
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
								tree.Add(cate1, Resources.strPublicProperty, member);
							}
							else if ((attr & MethodAttributes.Private) == MethodAttributes.Private)
							{
								tree.Add(cate1, Resources.strPrivateProperty, member);
							}
							else
							{
								tree.Add(cate1, Resources.strProtectedProperty, member);
							}
						}
						break;
					}
				case MemberTypes.Field:
					{
						FieldInfo field = member as FieldInfo;

						if (field.IsPublic)
						{
							tree.Add(cate1, Resources.strPublicField, member);
						}
						else if (field.IsPrivate)
						{
							tree.Add(cate1, Resources.strPrivateField, member);
						}
						else
						{
							tree.Add(cate1, Resources.strProtectedField, member);
						}
						break;
					}
				case MemberTypes.Event:
					{
						tree.Add(cate1, Resources.strEvent, member);
						break;
					}
				}

				#endregion
			}
			return tree;
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
			foreach (AssemblyView_AssemblyOnly_TreeNode asmNode in this.Nodes)
			{
				if (asmNode.Nodes[0].Checked && asmNode.Nodes[1].Checked)
				{
					foreach (Type type in asmNode.Assembly.GetTypes())
					{
						types.Add(new DocumentBuilderMember(true, type));
					}
				}
				else if (asmNode.Nodes[0].Checked)
				{
					foreach (Type type in asmNode.Assembly.GetTypes())
					{
						if(type.IsPublic) types.Add(new DocumentBuilderMember(true, type));
					}
				}
				else if (asmNode.Nodes[1].Checked)
				{
					foreach (Type type in asmNode.Assembly.GetTypes())
					{
						if (!type.IsPublic) types.Add(new DocumentBuilderMember(true, type));
					}
				}
			}
			return types.ToArray();
		}

		DocumentBuilderMember[] IAssemblyView.GetTypeMembers(Type type)
		{
			List<DocumentBuilderMember> mems = new List<DocumentBuilderMember>();

			AssemblyView_AssemblyOnly_TreeNode asmNode = null;
			foreach (AssemblyView_AssemblyOnly_TreeNode node in Nodes)
			{
				if (node.Assembly == type.Assembly)
				{
					asmNode = node;
					break;
				}
			}
			TreeNode asmSubNode = type.IsPublic ? asmNode.Nodes[Resources.strPublicType] : asmNode.Nodes[Resources.strNonPublicType];
			MemberTree tree = GetAllMembers(type);	
			foreach (TreeNode classNode in asmSubNode.Nodes)
			{
				foreach (TreeNode cate1Node in classNode.Nodes)
				{
					foreach (TreeNode cate2Node in cate1Node.Nodes)
					{
						foreach (MemberInfo member in tree.GetMembers(cate1Node.Name, cate2Node.Name))
						{
							mems.Add(new DocumentBuilderMember(cate1Node.Checked, member));
						}
					}
				}
			}
			return mems.ToArray();
		}

		Type[] IAssemblyView.GetSelectedTypes()
		{
			List<Type> types = new List<Type>();
			foreach (AssemblyView_AssemblyOnly_TreeNode asmNode in this.Nodes)
			{
				if (asmNode.Nodes[0].Checked && asmNode.Nodes[1].Checked)
				{
					types.AddRange(asmNode.Assembly.GetTypes());
				}
				else if (asmNode.Nodes[0].Checked)
				{
					foreach (Type type in asmNode.Assembly.GetTypes())
					{
						if (type.IsPublic) types.Add(type);
					}
				}
				else if (asmNode.Nodes[1].Checked)
				{
					foreach (Type type in asmNode.Assembly.GetTypes())
					{
						if (!type.IsPublic) types.Add(type);
					}
				}
			}
			return types.ToArray();
		}

		MemberInfo[] IAssemblyView.GetTypeSelectedMembers(Type type)
		{
			List<MemberInfo> mems = new List<MemberInfo>();

			AssemblyView_AssemblyOnly_TreeNode asmNode = null;
			foreach (AssemblyView_AssemblyOnly_TreeNode node in Nodes)
			{
				if (node.Assembly == type.Assembly)
				{
					asmNode = node;
					break;
				}
			}
			TreeNode asmSubNode = type.IsPublic ? asmNode.Nodes[Resources.strPublicType] : asmNode.Nodes[Resources.strNonPublicType];
			MemberTree tree = GetAllMembers(type);
			foreach (TreeNode classNode in asmSubNode.Nodes)
			{
				foreach (TreeNode cate1Node in classNode.Nodes)
				{
					foreach (TreeNode cate2Node in cate1Node.Nodes)
					{
                        if (cate2Node.Checked)
                        {
                            mems.AddRange(tree.GetMembers(cate1Node.Name, cate2Node.Name));
                        }
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
				_asmFiles.Remove((SelectedNode as AssemblyView_AssemblyOnly_TreeNode).Assembly.Location.ToUpper());
				this.Nodes.Remove(this.SelectedNode);
				if (this.Nodes.Count > 0) this.SelectedNode = this.Nodes[0];
			}
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
		#endregion
	}

	class AssemblyView_AssemblyOnly_TreeNode:TreeNode
	{
		Assembly _assembly;

		public Assembly Assembly
		{
			get { return _assembly; }
		}

		public AssemblyView_AssemblyOnly_TreeNode(Assembly assembly)
		{
			_assembly = assembly;
			Text = assembly.FullName;
			SelectedImageKey = Resources.strAssembly;
			ImageKey = Resources.strAssembly;
		}
	}

	class MemberTree
	{
		Hashtable _memberTree = new Hashtable();

		public MemberTree()
		{
			Hashtable members = new Hashtable();
			members.Add(Resources.strPublicMethod, new List<MemberInfo>());
			members.Add(Resources.strProtectedMethod, new List<MemberInfo>());
			members.Add(Resources.strPrivateMethod, new List<MemberInfo>());
			members.Add(Resources.strPublicProperty, new List<MemberInfo>());
			members.Add(Resources.strProtectedProperty, new List<MemberInfo>());
			members.Add(Resources.strPrivateProperty, new List<MemberInfo>());
			members.Add(Resources.strPublicField, new List<MemberInfo>());
			members.Add(Resources.strProtectedField, new List<MemberInfo>());
			members.Add(Resources.strPrivateField, new List<MemberInfo>());
			members.Add(Resources.strEvent, new List<MemberInfo>());

			Hashtable inhertedMembers = new Hashtable();
			inhertedMembers.Add(Resources.strPublicMethod, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strProtectedMethod, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strPrivateMethod, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strPublicProperty, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strProtectedProperty, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strPrivateProperty, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strPublicField, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strProtectedField, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strPrivateField, new List<MemberInfo>());
			inhertedMembers.Add(Resources.strEvent, new List<MemberInfo>());

			_memberTree.Add(Resources.strMember, members);
			_memberTree.Add(Resources.strInheritMember, inhertedMembers);
		}

		public void Add(string cateroty1, string category2, MemberInfo info)
		{
			((_memberTree[cateroty1] as Hashtable)[category2] as List<MemberInfo>).Add(info);
		}

		public MemberInfo[] GetMembers(string cateroty1, string category2)
		{
			return ((_memberTree[cateroty1] as Hashtable)[category2] as List<MemberInfo>).ToArray();
		}
	}
}
