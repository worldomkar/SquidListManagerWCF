using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Squid_Monitor.SquidManager;
using System.Diagnostics;

namespace Squid_Monitor
{
    class DomainListController
    {
        SquidManager.SqMgrClient sm = new SquidManager.SqMgrClient();
        
        public DomainListController()
        {
        }

        public delegate int Add(TreeNode tn);

        private TreeNode ConstructTreeFromDlist(SquidManager.DList dl, List<TreeNode> tn)
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
                    if (("Miscelleneous" == s.Name) || ("Global" == s.Name))
                        tnSection.Name = s.Name;
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

        public TreeNode LoadList(string listName, string listType, List<TreeNode> tn, ref TreeNode BlockTrust, ref TreeNode BlockIgnore, ref TreeNode NewDomains)
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
            TreeNode tnNewRoot = ConstructTreeFromDlist(dl, tn);
            switch (listType.ToLower())
            {
                case "trust":
                    {
                        TreeNode n = tnNewRoot.Nodes["Miscelleneous"];
                        if (null == n)
                            n = tnNewRoot.Nodes["Global"];
                        n.Expand();
                        n = n.Nodes[0];
                        BlockTrust = n;
                    }
                    break;
                case "block":
                    {
                        TreeNode n = tnNewRoot.Nodes["Global"];
                        if (null != n)
                        {
                            n.Expand();
                            BlockIgnore = n.Nodes[1];
                        }
                    }
                    break;
                case "new":
                    if (0 < tnNewRoot.Nodes.Count)
                    {
                        tnNewRoot.Nodes[0].Expand();
                        tnNewRoot.Nodes[0].Nodes[1].Expand();
                    }
                    NewDomains = tnNewRoot;
                    break;
            }
            tnNewRoot.Text = listName;
            return tnNewRoot;
        }

        public void ReloadLists ()
        {
            sm.ReloadLists();
        }

        public TreeNode GetNewDomains(List<TreeNode> tn)
        {
            return ConstructTreeFromDlist(sm.GetNewDomains(), tn);
        }

        public void AddNewDomain (string domain, string listType, string sectionName, string activeInactive)
        {
            sm.AddNewDomain(domain, listType, sectionName, activeInactive);
        }
    }
}
