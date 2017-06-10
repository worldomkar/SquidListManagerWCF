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
using Squid_Monitor.SquidManager;
using System.Diagnostics;

namespace Squid_Monitor
{
    public partial class SquidMonitor : Form
    {
        SquidManager.SqMgrClient sm = new SquidManager.SqMgrClient();//("ISqMgr", "http://localhost:8733/Design_Time_Addresses/sqmgr_wcf_svc/Service1/");
        public SquidMonitor()
        {
            InitializeComponent();
        }

        public delegate int Add(TreeNode tn);

        private TreeNode ConstructTreeFromDlist(SquidManager.DList dl)
        {
            if (null != dl)
            {
                TreeNode tnNewRoot = new TreeNode();
                tnNewRoot.Expand();
                if (0 == dl.Sections.Length)
                {
                    dl.Sections = new Section[1];
                    Section s = dl.Sections[0] = new Section();
                    s.Name = "Global";
                    s.ActiveDomain = new string[0];
                    s.InactiveDomain = new string[0];
                }
                foreach (SquidManager.Section s in dl.Sections)
                {
                    TreeNode tnSection = new TreeNode(s.Name);
                    TreeNode tnActive = new TreeNode("Active");
                    TreeNode tnInactive = new TreeNode("Inactive");
                    foreach (string domain in s.ActiveDomain)
                    {
                        TreeNode dmn = new TreeNode(domain);
                        tnActive.Nodes.Add(dmn);
                        tn.Add(dmn);
                    }
                    foreach (string domain in s.InactiveDomain)
                    {
                        TreeNode dmn = new TreeNode(domain);
                        tnInactive.Nodes.Add(dmn);
                        tn.Add(dmn);
                    }
                    tnSection.Nodes.Add(tnActive);
                    tnSection.Nodes.Add(tnInactive);
                    tnNewRoot.Nodes.Add(tnSection);
                }
                return tnNewRoot;
            }
            return null;
        }

        TreeNode tnNewDomains = null;
        void LoadList(string listName, string listType)
        {
            SquidManager.DList dl = null;
            switch (listType.ToLower())
            {
                case "trust":
                    dl = sm.GetTrustList();
                    break;
                case "block":
                    dl = sm.GetBlockList();
                    break;
                case "new":
                    dl = sm.GetNewDomains();
                    break;
            }
            TreeNode tnNewRoot = ConstructTreeFromDlist(dl);
            switch (listType.ToLower())
            {
                case "trust":
                    tnNewRoot.Nodes[19].Expand();
                    break;
                case "block":
                    tnNewRoot.Nodes[0].Expand();
                    BlockIgnore = tnNewRoot.Nodes[0].Nodes[1];
                    break;
                case "new":
                    if (0 < tnNewRoot.Nodes.Count)
                    {
                        tnNewRoot.Nodes[0].Expand();
                        tnNewRoot.Nodes[0].Nodes[1].Expand();
                    }
                    tnNewDomains = tnNewRoot;
                    break;
            }
            tnNewRoot.Text = listName;
            treeView1.Invoke(new Add(treeView1.Nodes.Add), tnNewRoot);
        }

        void LoadData()
        {
            LoadList("Trusted Domains", "trust");
            LoadList("Blocked Domains", "block");
            LoadList("New Domains", "new");
            loadCompleted = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread loader = new Thread(LoadData);
            loader.Start();
            treeView1.AllowDrop = true;
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Console.WriteLine(e.Item.ToString());
            if (((TreeNode)e.Item).Text[0] == '.')
            {
                DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Scroll);
            }
        }

        static int i = 0;
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("In DragDrop");
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
                            sm.AddNewDomain(Domain.Text, "trust", Section.Parent.Text, Section.Text);
                            break;
                        case "blocked domains":
                            sm.AddNewDomain(Domain.Text, "block", Section.Parent.Text, Section.Text);
                            break;
                    }
                    Domain.Parent.Nodes.Remove(Domain);
                    Section.Nodes.Add(Domain);
                }
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("In DragEnter");
            e.Effect = DragDropEffects.Move | DragDropEffects.Scroll;
        }

        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            i++;
            Console.WriteLine("In DragOver" + i);
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
            sm.ReloadLists();
        }

        private void reloadNewDomains ()
        {
            try
            {
                SquidManager.DList dl = sm.GetNewDomains();
                TreeNode tnNewDomains = ConstructTreeFromDlist(dl);
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

        private void ignoreAllNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((null != tnNewDomains) && (null != tnNewDomains.Nodes[0]) &&
                (null != tnNewDomains.Nodes[0].Nodes[1]) && (null != tnNewDomains.Nodes[0].Nodes[1].Nodes))
            {
                int iNodes = tnNewDomains.Nodes[0].Nodes[1].Nodes.Count;
                for (int i = 0; i < iNodes; ++i)
                {
                    TreeNode n = tnNewDomains.Nodes[0].Nodes[1].Nodes[i];
                    sm.AddNewDomain (n.Text, "block", "Global", "inactive");
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

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (e.Shift)
                {
                    focusedNode -= 2;
                    if (focusedNode < 0)
                        focusedNode += searchList.Count;
                    if (focusedNode < 0)
                        focusedNode = 0;
                }
                selectNextSearchedNode();
            }
        }
    }
}
