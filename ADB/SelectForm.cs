using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ADB
{
    using Properties;
    public partial class SelectForm : Form
    {
        public SelectForm()
        {
            InitializeComponent();
        }

        private void SelectOption_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Add(Resources.String21);
            treeView1.Nodes[0].Nodes.Add(Resources.strPublicType);
            treeView1.Nodes[0].Nodes.Add(Resources.strNonPublicType);
            foreach (TreeNode rootNode in treeView1.Nodes[0].Nodes)
            {
                rootNode.Nodes.Add(Resources.String22);
                foreach(TreeNode classNode in rootNode.Nodes)
                {
                    classNode.Nodes.Add(Resources.strMember);
                    classNode.Nodes.Add(Resources.strInheritMember);
                    foreach (TreeNode classSubNode in classNode.Nodes)
                    {
                        classSubNode.Nodes.Add(Resources.strPublicMethod);
                        classSubNode.Nodes.Add(Resources.strProtectedMethod);
                        classSubNode.Nodes.Add(Resources.strPrivateMethod);
                        classSubNode.Nodes.Add(Resources.strPublicProperty);
                        classSubNode.Nodes.Add(Resources.strProtectedProperty);
                        classSubNode.Nodes.Add(Resources.strPrivateProperty);
                        classSubNode.Nodes.Add(Resources.strPublicField);
                        classSubNode.Nodes.Add(Resources.strProtectedField);
                        classSubNode.Nodes.Add(Resources.strPrivateField);
                        classSubNode.Nodes.Add(Resources.strEvent);
                    }
                }
            }
            treeView1.ExpandAll();
            treeView1.SelectedNode = treeView1.Nodes[0];
        }

		public TreeNodeCollection Nodes
		{
			get { return treeView1.Nodes; }
		}
    }
}