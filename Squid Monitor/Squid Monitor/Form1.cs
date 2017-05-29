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
    public partial class Form1 : Form
    {
        SquidManager.SqMgrClient sm = new SquidManager.SqMgrClient();//("ISqMgr", "http://localhost:8733/Design_Time_Addresses/sqmgr_wcf_svc/Service1/");
        public Form1()
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
                foreach (SquidManager.Section s in dl.Sections)
                {
                    TreeNode tnSection = new TreeNode(s.Name);
                    TreeNode tnActive = new TreeNode("Active");
                    TreeNode tnInactive = new TreeNode("Inactive");
                    foreach (string domain in s.ActiveDomain)
                        tnActive.Nodes.Add(new TreeNode(domain));
                    foreach (string domain in s.InactiveDomain)
                        tnInactive.Nodes.Add(new TreeNode(domain));
                    tnSection.Nodes.Add(tnActive);
                    tnSection.Nodes.Add(tnInactive);
                    tnNewRoot.Nodes.Add(tnSection);
                }
                return tnNewRoot;
            }
            return null;
        }

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
                    break;
                case "new":
                    if (0 < tnNewRoot.Nodes.Count)
                    {
                        tnNewRoot.Nodes[0].Expand();
                        tnNewRoot.Nodes[0].Nodes[1].Expand();
                    }
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
                    Section.Nodes.Add((TreeNode)Domain.Clone());
                    Domain.Remove();
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
            treeView1.Height = Form1.ActiveForm.Height - 80;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sm.ReloadLists();
        }

        private void reloadNewDomainsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SquidManager.DList dl = sm.GetNewDomains();
            treeView1.Nodes.RemoveAt(2);
            TreeNode tnNewDomains = ConstructTreeFromDlist(dl);
            tnNewDomains.Text = "New Domains";
            tnNewDomains.Nodes[0].Expand();
            tnNewDomains.Nodes[0].Nodes[1].Expand();
            treeView1.Nodes.Add(tnNewDomains);
        }
    }
}
