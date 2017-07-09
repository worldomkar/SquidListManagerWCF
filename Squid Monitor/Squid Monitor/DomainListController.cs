//-----------------------------------------------------------------------
// <copyright file="DomainListController.cs" company="_">
// Squid_Monitor
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace Squid_Monitor
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SquidManager;

    /// <summary>
    /// DomainListController maintains interaction with Squid Manager WCF service
    ///   and acts as a controller for the presentation layer.
    /// </summary>
    public class DomainListController
    {
        /// <summary>
        /// Client of WCF service
        /// </summary>
        private SquidManagerClient squidManager = null;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainListController"/> class
        /// </summary>
        public DomainListController()
        {
            try
            {
                this.squidManager = new SquidManager.SquidManagerClient();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// LoadList fetches domains list from Squid Manager and prepares a TreeNode structure
        /// </summary>
        /// <param name="listName">Name of a domain list</param>
        /// <param name="listType">Type of a domain list</param>
        /// <param name="knownDomains">List of "Loaded" domains</param>
        /// <param name="blockTrust">Trusted domains List root node</param>
        /// <param name="blockIgnore">Ignored domains List root node</param>
        /// <param name="newDomains">Newly detected List root node</param>
        /// <returns>Returns root node of created TreeNode list</returns>
        public TreeNode LoadList(string listName, string listType, List<TreeNode> knownDomains, ref TreeNode blockTrust, ref TreeNode blockIgnore, ref TreeNode newDomains)
        {
            SquidManager.DomainsList domainsList = null;
            switch (listType.ToLower())
            {
                case "trust":
                    domainsList = this.squidManager.GetTrustList();
                    break;
                case "block":
                    domainsList = this.squidManager.GetBlockList();
                    break;
                case "new":
                    domainsList = this.squidManager.GetNewDomains();
                    break;
            }

            TreeNode newRoot = this.ConstructTreeFromDlist(domainsList, knownDomains);
            switch (listType.ToLower())
            {
                case "trust":
                    {
                        TreeNode n = newRoot.Nodes["Miscelleneous"];
                        if (n == null)
                        {
                            n = newRoot.Nodes["Global"];
                        }

                        n.Expand();
                        n = n.Nodes[0];
                        blockTrust = n;
                    }

                    break;
                case "block":
                    {
                        TreeNode n = newRoot.Nodes["Global"];
                        if (n != null)
                        {
                            n.Expand();
                            blockIgnore = n.Nodes[1];
                        }
                    }

                    break;
                case "new":
                    if (newRoot.Nodes.Count > 0)
                    {
                        newRoot.Nodes[0].Expand();
                        newRoot.Nodes[0].Nodes[1].Expand();
                    }

                    newDomains = newRoot;
                    break;
            }

            newRoot.Text = listName;
            return newRoot;
        }

        /// <summary>
        /// Tells Squid Manager to reload lists
        /// Particularly useful to call as soon as lists are finished editing externally
        /// </summary>
        public void ReloadLists()
        {
            this.squidManager.ReloadLists();
        }

        /// <summary>
        /// Returns detected unknown/new domains from squid deny log
        /// The domains are guarenteed to be absent from Trust or Block list
        /// Also, it is guarenteed that the domains are not subdomain of a domain in Trust or Block list
        /// </summary>
        /// <param name="knownDomains">List of known domains</param>
        /// <returns>TreeNode hierarchy of new domains</returns>
        public TreeNode GetNewDomains(List<TreeNode> knownDomains)
        {
            return this.ConstructTreeFromDlist(this.squidManager.GetNewDomains(), knownDomains);
        }

        /// <summary>
        /// Adds a "New" domain to "Trust" or "Block" list
        /// </summary>
        /// <param name="domain">Domain name to be added</param>
        /// <param name="listType">List name to which the domain is to be added</param>
        /// <param name="sectionName">Section name in the list</param>
        /// <param name="activeInactive">Domain entry in the list should be active or inactive</param>
        public void AddNewDomain(string domain, string listType, string sectionName, string activeInactive)
        {
            this.squidManager.AddNewDomain(domain, listType, sectionName, activeInactive);
        }

        /// <summary>
        /// Constructs presentable tree from plain-text domains list
        /// </summary>
        /// <param name="domainsList">List of domains</param>
        /// <param name="knownDomains">"Loaded list" of domains</param>
        /// <returns>TreeNode hierarchy containing domains list</returns>
        private TreeNode ConstructTreeFromDlist(SquidManager.DomainsList domainsList, List<TreeNode> knownDomains)
        {
            if (domainsList != null)
            {
                TreeNode newRoot = new TreeNode();
                newRoot.Expand();
                if (domainsList.sections.Length == 0)
                {
                    domainsList.sections = new Section[1];
                    Section s = domainsList.sections[0] = new Section();
                    s.Name = "Global";
                    s.ActiveDomain = new string[0];
                    s.InactiveDomain = new string[0];
                }

                foreach (SquidManager.Section s in domainsList.sections)
                {
                    TreeNode section = new TreeNode(s.Name);
                    if ((s.Name == "Miscelleneous") || (s.Name == "Global"))
                    {
                        section.Name = s.Name;
                    }

                    TreeNode active = new TreeNode("Active");
                    TreeNode inactive = new TreeNode("Inactive");
                    foreach (string domain in s.ActiveDomain)
                    {
                        TreeNode dmn = new TreeNode(domain);
                        active.Nodes.Add(dmn);
                        knownDomains.Add(dmn);
                    }

                    foreach (string domain in s.InactiveDomain)
                    {
                        TreeNode dmn = new TreeNode(domain);
                        inactive.Nodes.Add(dmn);
                        knownDomains.Add(dmn);
                    }

                    section.Nodes.Add(active);
                    section.Nodes.Add(inactive);
                    newRoot.Nodes.Add(section);
                }

                return newRoot;
            }

            return null;
        }
    }
}
