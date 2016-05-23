using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ADB
{
    public partial class AutoSelectTreeView : TreeView
    {
        public AutoSelectTreeView()
        {
            InitializeComponent();
            this.AfterCheck+=new TreeViewEventHandler(AutoSelectTreeView_AfterCheck);
        }

        bool _handle_tvObjects_AfterCheck = true;

        private void AutoSelectTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_handle_tvObjects_AfterCheck)
            {
                _handle_tvObjects_AfterCheck = false;
                try
                {
                    if (e.Node.Checked)
                    {
                        TreeNode parent = e.Node.Parent;
                        while (parent != null)
                        {
                            parent.Checked = true;
                            parent = parent.Parent;
                        }
                    }
                    SelectAllSubNodes(e.Node, e.Node.Checked);
                }
                catch (Exception)
                {
                }
                _handle_tvObjects_AfterCheck = true;
            }
        }

        void SelectAllSubNodes(TreeNode node, bool selected)
        {
            foreach (TreeNode subNode in node.Nodes)
            {
                subNode.Checked = selected;
                SelectAllSubNodes(subNode, selected);
            }
        }
    }
}
