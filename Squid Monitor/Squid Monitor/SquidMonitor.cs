using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Squid_Monitor
{
    public partial class SquidMonitor : Form
    {
        public SquidMonitor()
        {
            InitializeComponent();
        }
        
        public delegate int Add(TreeNode tn);
        DomainListController dlc = new DomainListController();
        void LoadList(string listName, string listType)
        {
            TreeNode tnNewRoot = dlc.LoadList(listName, listType, tn, ref BlockTrust, ref BlockIgnore, ref tnNewDomains);
            treeView1.Invoke(new Add(treeView1.Nodes.Add), tnNewRoot);
        }

        void LoadLists()
        {
            LoadList("Trusted Domains", "trust");
            LoadList("Blocked Domains", "block");
            LoadList("New Domains", "new");
            loadCompleted = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread loader = new Thread(LoadLists);
            loader.Start();
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Console.WriteLine(e.Item.ToString());
            if (((TreeNode)e.Item).Text[0] == '.')
            {
                DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Scroll);
            }
        }
        
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode Section = ((TreeView)sender).GetNodeAt(pt);
                TreeNode Domain = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if ((null != Section) &&
                    (Section != Domain) &&
                    (null != Section.Parent) &&
                    (null != Section.Parent.Parent))
                {
                    switch (Section.Parent.Parent.Text.ToLower())
                    {
                        case "trusted domains":
                            dlc.AddNewDomain(Domain.Text, "trust", Section.Parent.Text, Section.Text);
                            break;
                        case "blocked domains":
                            dlc.AddNewDomain(Domain.Text, "block", Section.Parent.Text, Section.Text);
                            break;
                    }
                    Domain.Parent.Nodes.Remove(Domain);
                    Section.Nodes.Add(Domain);
                }
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move | DragDropEffects.Scroll;
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode Section = ((TreeView)sender).GetNodeAt(pt);
                TreeNode Domain = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if ((null != Section) &&
                    (Section != Domain) &&
                    (null != Section.Parent) &&
                    (null != Section.Parent.Parent))
                {
                    e.Effect = DragDropEffects.Move | DragDropEffects.Scroll;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            treeView1.Height = SquidMonitor.ActiveForm.Height - 80;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlc.ReloadLists();
        }

        private void reloadNewDomains ()
        {
            try
            {
                TreeNode tnNewDomains = dlc.GetNewDomains(tn);
                tnNewDomains.Text = "New Domains";
                tnNewDomains.Nodes[0].Expand();
                tnNewDomains.Nodes[0].Nodes[1].Expand();
                foreach (TreeNode n in tnNewDomains.Nodes[0].Nodes[1].Nodes)    // Add new nodes
                {
                    TreeNode[] tnCol = treeView1.Nodes[2].Nodes[0].Nodes[1].Nodes.Cast<TreeNode>()
                        .Where(r => (r.Text == n.Text))
                        .ToArray();
                    if (0 == tnCol.Count())
                    {
                        treeView1.Nodes[2].Nodes[0].Nodes[1].Nodes.Add(n);
                        tn.Add(n);
                    }
                }
            } catch (Exception e) { }
        }

        private void reloadNewDomainsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reloadNewDomains();
        }

        TreeNode tnNewDomains = null;

        private void ignoreAllNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((null != tnNewDomains) && (null != tnNewDomains.Nodes[0]) &&
                (null != tnNewDomains.Nodes[0].Nodes[1]) && (null != tnNewDomains.Nodes[0].Nodes[1].Nodes))
            {
                int iNodes = tnNewDomains.Nodes[0].Nodes[1].Nodes.Count;
                for (int i = 0; i < iNodes; ++i)
                {
                    TreeNode n = tnNewDomains.Nodes[0].Nodes[1].Nodes[i];
                    dlc.AddNewDomain (n.Text, "block", "Global", "inactive");
                }
                for (int i = 0; i < iNodes; ++i)
                {
                    TreeNode n = tnNewDomains.Nodes[0].Nodes[1].Nodes[0];
                    n.Parent.Nodes.Remove(n);
                    BlockIgnore.Nodes.Add(n);
                }
            }
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchBox.Clear();
            focusedNode = 0;
            txtSearchChanged = true;
            foreach (TreeNode List in treeView1.Nodes)
                foreach (TreeNode Section in List.Nodes)
                {
                    foreach (TreeNode subList in Section.Nodes)
                        subList.Collapse();
                    if ((0 != String.Compare(Section.Text, "Miscelleneous", StringComparison.OrdinalIgnoreCase)) &&
                        ((0 != String.Compare(Section.Text, "Global", StringComparison.OrdinalIgnoreCase)) ||
                        (Section.Parent.Text.Contains("Trusted Domains"))))
                    {
                        Section.Collapse();
                    }
                }
        }

        private TreeNode BlockIgnore = null;
        private TreeNode BlockTrust = null;
        private bool loadCompleted = false;
        private void SquidMonitor_Activated(object sender, EventArgs e)
        {
            if (!loadCompleted)
                return;
            reloadNewDomains();
        }

        bool txtSearchChanged = false;
        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            txtSearchChanged = true;
        }

        List<TreeNode> tn = new List<TreeNode>();
        List<TreeNode> searchList = new List<TreeNode>();
        int focusedNode = 0;
        private void txtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                selectNextSearchedNode();
            }
        }

        private void selectNextSearchedNode()
        {
            if (txtSearchChanged || (0 == searchList.Count))
            {
                searchList = tn.Where(x => (x.Text.Contains(txtSearchBox.Text))).ToList();
                focusedNode = 0;
                txtSearchChanged = false;
            }
            try
            {
                treeView1.Select();
                treeView1.SelectedNode = searchList[focusedNode];
            }
            catch (Exception e2)
            {

            }
            ++focusedNode;
            if (focusedNode >= searchList.Count)
                focusedNode = 0;
        }

        private void selectPreviousSearchedNode ()
        {
            -- focusedNode;
            if (focusedNode < 0)
            {
                focusedNode += searchList.Count;
            }
        }
        private bool isDomainNode (TreeNode tNode)
        {
            if ((null != treeView1.SelectedNode) && (null != treeView1.SelectedNode.Parent) &&
                        (null != treeView1.SelectedNode.Parent.Parent))
                return true;
            return false;
        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            TreeNode tnSelected = treeView1.SelectedNode;
            bool addSubtractProcess = false;
            string listType = null, sectionName = null, activeInactive = null;
            switch (e.KeyCode)
            {
                case Keys.F3:
                    if (e.Shift)
                        selectPreviousSearchedNode();
                    else
                        selectNextSearchedNode();
                    break;
                case Keys.Add:
                    listType = "trust";
                    sectionName = "Miscelleneous";
                    activeInactive = "active";
                    addSubtractProcess = true;
                    break;
                case Keys.Subtract:
                    listType = "block";
                    sectionName = "Global";
                    activeInactive = "inactive";
                    addSubtractProcess = true;
                    break;
            }
            if ((addSubtractProcess) && isDomainNode(tnSelected))
            {
                TreeNode next = (null != tnSelected.NextNode)? tnSelected.NextNode:
                    ((null != tnSelected.PrevNode)? tnSelected.PrevNode : tnSelected.Parent);
                dlc.AddNewDomain(tnSelected.Text, listType, sectionName, activeInactive);
                tnSelected.Parent.Nodes.Remove(tnSelected);
                BlockTrust.Nodes.Add(tnSelected);
                treeView1.SelectedNode = next;
            }
        }

#region Placeholder text logic for domain search textbox
        Color searchBoxTextColor = Color.Black;
        string phText = "Search domain";
        private void txtSearchBox_GotFocus(object sender, EventArgs e)
        {
            if (txtSearchBox.Text == phText)
            {
                txtSearchBox.Text = "";
                txtSearchBox.ForeColor = searchBoxTextColor;
            }
        }

        private void txtSearchBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchBox.Text))
            {
                txtSearchBox.ForeColor = Color.Gray;
                txtSearchBox.Text = phText;
            }
        }
#endregion
    }
}
