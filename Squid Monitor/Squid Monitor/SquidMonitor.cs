//-----------------------------------------------------------------------
// <copyright file="SquidMonitor.cs" company="_">
// Squid_Monitor
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace Squid_Monitor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// SquidMonitor class is primary dialog derived from "Form"
    /// </summary>
    public partial class SquidMonitor : Form
    {
        /// <summary>
        /// Placeholder text that should appear in empty search box
        /// </summary>
        private string placeholderText = "Search domain";

        /// <summary>
        /// DomainListController (ViewModel) handles data flow between the SquidManager web services (Model) and
        /// SquidMonitor dialog (View/Presentation)
        /// Together MVVM pattern is formed.
        /// </summary>
        private DomainListController dlc = new DomainListController();

        /// <summary>
        /// Points to "block" list "global" section
        /// </summary>
        private TreeNode blockIgnore = null;

        /// <summary>
        /// Points to "trust" list "miscelleneous" section
        /// </summary>
        private TreeNode blockTrust = null;

        /// <summary>
        /// Points to "New" list "Global" section
        /// </summary>
        private TreeNode blockNew = null;

        /// <summary>
        /// Separate thread to lazy load domain lists OR wait for service connection
        /// </summary>
        private Thread lazyLoader = null;

        /// <summary>
        /// Contains "load" state of lists
        /// </summary>
        private bool loadCompleted = false;

        /// <summary>
        /// Contains "isDirty" state of the searchbox
        /// </summary>
        private bool txtSearchChanged = false;

        /// <summary>
        /// Holds pointers to all nodes in the treeview
        /// </summary>
        private List<TreeNode> knownDomains = new List<TreeNode>();

        /// <summary>
        /// Holds pointers to all nodes in the treeview
        /// </summary>
        private List<TreeNode> knownNodes = new List<TreeNode>();

        /// <summary>
        /// Holds pointers to nodes that match search criteria
        /// </summary>
        private List<TreeNode> searchList = new List<TreeNode>();

        /// <summary>
        /// Index of a node in searchList
        /// </summary>
        private int focusedNode = 0;

        /// <summary>
        /// Maintains state of text colour in search box
        /// Changed to gray when search box contains placeholder text
        /// Changed to black when user enters something into search box or just clicks search box
        /// </summary>
        private Color searchBoxTextColor = Color.Black;

        /// <summary>
        /// Initializes a new instance of the <see cref="SquidMonitor"/> class.
        /// </summary>
        public SquidMonitor()
        {
            this.InitializeComponent();
            txtSearchBox.Text = this.placeholderText;
        }

        /// <summary>
        /// Adds a previously created tree node to the end of the tree node collection.
        /// </summary>
        /// <param name="node">The System.Windows.Forms.TreeNode to add to the collection.</param>
        /// <returns>The zero-based index value of the System.Windows.Forms.TreeNode added to the
        /// tree node collection.</returns>
        public delegate int Add(TreeNode node);
        
        /// <summary>
        /// SquidMonitor will load a domain list from dlc
        /// </summary>
        /// <param name="listName">Listname as desired to be displayed on UI</param>
        /// <param name="listType">Listname (trust/block) to pass dlc</param>
        private void LoadList(string listName, string listType)
        {
            TreeNode newRoot = this.dlc.LoadList(listName, listType, this.knownDomains, this.knownNodes, ref this.blockTrust, ref this.blockIgnore, ref this.blockNew);
            treeView1.Invoke(new Add(treeView1.Nodes.Add), newRoot);
        }

        /// <summary>
        /// Loads required three lists
        /// Currently "Ignored" list is "block"ed "inactive"
        /// </summary>
        private void LazyLoadLists()
        {
            this.LoadList("Trusted Domains", "trust");
            this.LoadList("Blocked Domains", "block");
            this.LoadList("New Domains", "new");
            this.loadCompleted = true;
            this.Invoke((MethodInvoker)delegate
            {
                this.connectingMessage.Visible = false;
                this.Text = "Squid Manager - Live";
                this.menuStrip1.Enabled = true;
                this.txtSearchBox.Enabled = true;
                this.btnClearSearch.Enabled = true;
            });
            this.ResetNodeExpansionState();
        }

        /// <summary>
        /// On Form Load -- Load lists
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void SquidMonitor_Load(object sender, EventArgs e)
        {
            this.lazyLoader = new Thread(this.LazyLoadLists);
            this.lazyLoader.Start();
        }

        /// <summary>
        /// Abort lazyLoader if in waiting state to help graceful exit
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">formclosedeventargs e</param>
        private void SquidMonitor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.lazyLoader.ThreadState != ThreadState.Stopped)
            {
                this.lazyLoader.Abort();
            }
        }

        /// <summary>
        /// ItemDrag (Drag start first) event handler for the treeview
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">itemdrageventargs e</param>
        private void TreeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            Console.WriteLine(e.Item.ToString());
            if (((TreeNode)e.Item).Text[0] == '.')
            {
                this.DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Scroll);
            }
        }

        /// <summary>
        /// DragDrop (Drop) event handler for the treeview
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">drageventargs e</param>
        private void TreeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode section = ((TreeView)sender).GetNodeAt(pt);
                TreeNode domain = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if ((section != domain) && this.IsSectionNode(section))
                {
                    switch (section.Parent.Parent.Text.ToLower())
                    {
                        case "trusted domains":
                            this.dlc.AddNewDomain(domain.Text, "trust", section.Parent.Text, section.Text);
                            break;
                        case "blocked domains":
                            this.dlc.AddNewDomain(domain.Text, "block", section.Parent.Text, section.Text);
                            break;
                    }

                    domain.Parent.Nodes.Remove(domain);
                    section.Nodes.Add(domain);
                }
            }
        }

        /// <summary>
        /// DragEnter (Drag start second) event handler for the treeview
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">drageventargs e</param>
        private void TreeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move | DragDropEffects.Scroll;
        }

        /// <summary>
        /// DragOver event handler for the treeview
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">drageventargs e</param>
        private void TreeView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode section = ((TreeView)sender).GetNodeAt(pt);
                TreeNode domain = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (this.IsDomainNode(domain))
                {
                    if ((section != domain) && this.IsSectionNode(section))
                    {
                        e.Effect = DragDropEffects.Move | DragDropEffects.Scroll;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
        }

        /// <summary>
        /// Form resize -- resize treeview
        /// </summary>
        private void ResizeTreeView()
        {
            treeView1.Height = this.ClientRectangle.Height - this.menuStrip1.Height - 16;
            treeView1.Width = this.Width - 40;
        }
        
        /// <summary>
        /// Form resize -- call resize treeview
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void SquidMonitor_ResizeEnd(object sender, EventArgs e)
        {
            this.ResizeTreeView();
        }

        /// <summary>
        /// Ask squid to reload (externally updated) lists from disk
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void ReloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dlc.ReloadLists();
        }

        /// <summary>
        /// Check with SquidManager for uncategorised domains
        /// </summary>
        private void ReloadNewDomains()
        {
            try
            {
                TreeNode newDomains = this.dlc.GetNewDomains(this.knownDomains, this.knownNodes);
                newDomains.Text = "New Domains";
                TreeNodeCollection blockNewNodes = newDomains.Nodes[0].Nodes[1].Nodes;
                TreeNodeCollection treeViewNewNodes = treeView1.Nodes[2].Nodes[0].Nodes[1].Nodes;

                // Add new nodes
                foreach (TreeNode node in blockNewNodes)
                {
                    TreeNode[] matching = treeViewNewNodes.Cast<TreeNode>()
                        .Where(r => (r.Text == node.Text))
                        .ToArray();
                    if (matching.Count() == 0)
                    {
                        treeViewNewNodes.Add(node);
                        this.knownDomains.Add(node);
                    }
                }

                this.ResetNodeExpansionState();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Query SquidManager for NewDomains
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void ReloadNewDomainsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ReloadNewDomains();
        }

        /// <summary>
        /// Move all new unique domains to ignore list (block list, inactive section)
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void IgnoreAllNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.IsValidNewBlockNode(this.blockNew))
            {
                TreeNodeCollection blockNewNodes = this.blockNew.Nodes[0].Nodes[1].Nodes;
                int newNodesCount = blockNewNodes.Count;
                for (; newNodesCount > 0; --newNodesCount)
                {
                    TreeNode node = blockNewNodes[0];
                    this.dlc.AddNewDomain(node.Text, "block", "Global", "inactive");
                    node.Parent.Nodes.Remove(node);
                    this.blockIgnore.Nodes.Add(node);
                }
            }
        }
        
        /// <summary>
        /// Resets expanded/collapsed state of nodes to initial setting
        /// TreeNode.Tag is used to store a non-null (bool) value to indicate initially "expanded"
        /// </summary>
        private void ResetNodeExpansionState()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(this.ResetNodeExpansionState));
            }
            else
            {
                this.knownNodes.Where(n => (n.Tag != null)).ToList().ForEach(n =>
                {
                    if (!n.IsExpanded)
                    {
                        n.Expand();
                    }
                });
                this.knownNodes.Where(n => (n.Tag == null)).ToList().ForEach(n =>
                {
                    if (n.IsExpanded)
                    {
                        n.Collapse();
                    }
                });
            }
        }

        /// <summary>
        /// Clear searchbox and set treenode expansion states to "initial" appearance
        /// </summary>
        /// <param name="sender">object sender, EventArgs e</param>
        /// <param name="e">eventargs e</param>
        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchBox.Clear();
            txtSearchBox.Text = this.placeholderText;
            txtSearchBox.ForeColor = Color.Gray;
            this.focusedNode = 0;
            this.txtSearchChanged = true;
            foreach (TreeNode list in treeView1.Nodes)
            {
                foreach (TreeNode section in list.Nodes)
                {
                    foreach (TreeNode subList in section.Nodes)
                    {
                        subList.Collapse();
                    }
                    
                    if ((string.Compare(section.Text, "Miscelleneous", StringComparison.OrdinalIgnoreCase) != 0) &&
                        ((string.Compare(section.Text, "Global", StringComparison.OrdinalIgnoreCase) != 0) ||
                        section.Parent.Text.Contains("Trusted Domains")))
                    {
                        section.Collapse();
                    }
                }
            }
        }

        /// <summary>
        /// Load new domains on SquidMonitor Form Activate event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void SquidMonitor_Activated(object sender, EventArgs e)
        {
            if (!this.loadCompleted)
            {
                return;
            }

            this.ReloadNewDomains();
        }

        /// <summary>
        /// Set dirty flag
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void TxtSearchBox_TextChanged(object sender, EventArgs e)
        {
            this.txtSearchChanged = true;
        }

        /// <summary>
        /// Enter key in searchbox finds and highlights first match
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">keypresseventargs e</param>
        private void TxtSearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                this.SelectNextSearchedNode();
            }
        }

        /// <summary>
        /// Select next node from list of searchbox matched domains
        /// </summary>
        private void SelectNextSearchedNode()
        {
            if (this.txtSearchChanged || (this.searchList.Count == 0))
            {
                this.searchList = this.knownDomains.Where(x => x.Text.Contains(txtSearchBox.Text)).ToList();
                this.focusedNode = 0;
                this.txtSearchChanged = false;
            }

            try
            {
                treeView1.Select();
                treeView1.SelectedNode = this.searchList[this.focusedNode];
            }
            catch (Exception)
            {
            }

            ++this.focusedNode;
            if (this.focusedNode >= this.searchList.Count)
            {
                this.focusedNode = 0;
            }
        }

        /// <summary>
        /// Move search index one step in reverse direction
        /// </summary>
        private void SelectPreviousSearchedNode()
        {
            --this.focusedNode;
            if (this.focusedNode < 0)
            {
                this.focusedNode += this.searchList.Count;
            }
        }

        /// <summary>
        /// Checks if current treenode represents a "section"
        /// </summary>
        /// <param name="node">treenode node</param>
        /// <returns>True if node is a section node</returns>
        private bool IsSectionNode(TreeNode node)
        {
            if ((node != null) && (node.Parent != null) &&
                (node.Parent.Parent != null))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if current treenode represents a "domain"
        /// </summary>
        /// <param name="node">treenode node</param>
        /// <returns>True if node is a domain node</returns>
        private bool IsDomainNode(TreeNode node)
        {
            if (this.IsSectionNode(node) && (node.Parent.Parent.Parent != null))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if current treenode represents a valid "New Domains" block
        /// </summary>
        /// <param name="node">treenode node</param>
        /// <returns>True if it is valid new block node with "Global" section and "Inactive" domain sub-list</returns>
        private bool IsValidNewBlockNode(TreeNode node)
        {
            if ((node != null) && (node.Nodes[0] != null) && (node.Nodes[0].Nodes[1] != null) &&
                (node.Nodes[0].Nodes[1].Nodes != null))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// "+"         Adds selected domain to trust list miscelleneous section, marks it active
        /// "-"         Adds selected domain to block list global section, marks it inactive
        /// "F3"        Repeats search
        /// "SHIFT+F3"  Repeats reverse search
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">keyeventargs e</param>
        private void TreeView1_KeyUp(object sender, KeyEventArgs e)
        {
            TreeNode selectedNode = treeView1.SelectedNode;
            bool addSubtractProcess = false;
            string listType = null, sectionName = null, activeInactive = null;
            switch (e.KeyCode)
            {
                case Keys.F3:
                    if (e.Shift)
                    {
                        this.SelectPreviousSearchedNode();
                    }
                    else
                    {
                        this.SelectNextSearchedNode();
                    }

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

            if (addSubtractProcess && this.IsDomainNode(selectedNode))
            {
                TreeNode next = (selectedNode.NextNode != null) ? selectedNode.NextNode :
                    ((selectedNode.PrevNode != null) ? selectedNode.PrevNode : selectedNode.Parent);
                this.dlc.AddNewDomain(selectedNode.Text, listType, sectionName, activeInactive);
                selectedNode.Parent.Nodes.Remove(selectedNode);
                this.blockTrust.Nodes.Add(selectedNode);
                treeView1.SelectedNode = next;
            }
        }

#region Placeholder text logic for domain search textbox
        /// <summary>
        /// Functionality for placeholder text in searchbox
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">event arguments</param>
        private void TxtSearchBox_GotFocus(object sender, EventArgs e)
        {
            if (txtSearchBox.Text == this.placeholderText)
            {
                txtSearchBox.Text = string.Empty;
                txtSearchBox.ForeColor = this.searchBoxTextColor;
            }
        }

        /// <summary>
        /// Put placeholder text if searchbox is empty
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">eventargs e</param>
        private void TxtSearchBox_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchBox.Text))
            {
                txtSearchBox.ForeColor = Color.Gray;
                txtSearchBox.Text = this.placeholderText;
            }
        }
        #endregion
    }
}
