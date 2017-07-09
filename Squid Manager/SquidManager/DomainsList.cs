//-----------------------------------------------------------------------
// <copyright file="DomainsList.cs" company="_">
// DomainsList
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;

    /// <summary>
    /// DomainsList simply contains list of domain-names
    /// </summary>
    [DataContract]
    public class DomainsList
    {
        /// <summary>
        /// List of Sections
        /// Sections contain domain-names
        /// </summary>
        [DataMember]
        private List<Section> sections;

        /// <summary>
        /// Associated FileName for synching domain list to disk
        /// </summary>
        private string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainsList"/> class.
        /// </summary>
        public DomainsList()
        {
            this.sections = new List<Section>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainsList"/> class.
        /// </summary>
        /// <param name="fileName">File name to load Domain list from</param>
        public DomainsList(string fileName)
        {
            this.sections = new List<Section>();
            this.Load(fileName);
        }

        /// <summary>
        /// Return sections contained in the current list
        /// </summary>
        /// <returns>List of sections</returns>
        public List<Section> GetSections()
        {
            return this.sections;
        }

        /// <summary>
        /// Reload list of domains from associated fileName
        /// </summary>
        public void Reload()
        {
            if (this.sections == null)
            {
                this.sections = new List<Section>();
            }
            else
            {
                this.sections.Clear();
            }

            if (this.fileName != null)
            {
                this.Load(this.fileName);
            }
        }

        /// <summary>
        /// Loads a text based domains list
        /// Simple format of domains in the file is expected to start with a "." followed by domain-name
        /// one domain-name per line
        /// </summary>
        /// <param name="fileName">File name to load domains list from</param>
        public void Load(string fileName)
        {
            FileStream streamOfDomains = null;
            string line;
            Section s = new Section();
            s.Name = "Global";
            this.sections.Add(s);

            try
            {
                streamOfDomains = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                this.fileName = fileName;
                StreamReader domainsList = new StreamReader(streamOfDomains);
                try
                {
                    while ((line = domainsList.ReadLine()) != null)
                    {
                        line.Trim();
                        if (line.Length == 0)
                        {
                            continue;
                        }

                        if (line.Substring(0, 5) == "#####")
                        {
                            if ((s.ActiveDomain.Count != 0) || (s.InactiveDomain.Count != 0))
                            {
                                s = new Section();
                                this.sections.Add(s);
                            }

                            s.Name = line.Substring(5).Trim();
                        }
                        else
                        {
                            if (line[0] == '#')
                            {
                                s.InactiveDomain.Add(line.Substring(1).Trim());
                            }
                            else
                            {
                                s.ActiveDomain.Add(line);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Returns true if "domain" is a direct match to (or subdomain of) any domain in trust list or block list
        /// </summary>
        /// <param name="domain">Domain name</param>
        /// <returns>true false</returns>
        public bool IsKnown(string domain)
        {
            foreach (Section s in this.sections)
            {
                try
                {
                    if ((s.ActiveDomain.Find(d => (d == domain) ? true : this.IsSubDomain(d, domain)) != null) ||
                        (s.InactiveDomain.Find(d => (d == domain) ? true : this.IsSubDomain(d, domain)) != null))
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a domain to Global section and inactive state of current domains list
        /// </summary>
        /// <param name="domain">Domain name</param>
        public void AddDomain(string domain)
        {
            if (this.sections.Count == 0)
            {
                this.sections.Add(new Section());
                this.sections[0].Name = "Global";
            }

            this.sections[0].InactiveDomain.Add(domain);
        }

        /// <summary>
        /// Removes a domain from current domain list
        /// </summary>
        /// <param name="domain">Domain name</param>
        public void RemoveDomain(string domain)
        {
            foreach (Section s in this.sections)
            {
                if (s.ActiveDomain.Remove(domain) || s.InactiveDomain.Remove(domain))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Receive from ViewModel a "movement" of new domain to trust or block list
        /// </summary>
        /// <param name="domain">Domain name</param>
        /// <param name="sectionName">Desired target section name</param>
        /// <param name="activeInactive">Desired active or inactive state</param>
        public void AddNewDomain(string domain, string sectionName, string activeInactive)
        {
            try
            {
                // Skip known domain/sub-domains addition.
                if (this.IsKnown(domain))
                {
                    return;
                }

                Section s = this.sections.Find(x => x.Name == sectionName);
                if (s == null)
                {
                    s = new Section();
                    s.Name = sectionName;
                    this.sections.Add(s);
                }

                switch (activeInactive.ToLower())
                {
                    case "active":
                        s.ActiveDomain.Add(domain);
                        break;
                    case "inactive":
                        s.InactiveDomain.Add(domain);
                        break;
                }

                this.Save();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Check if "subDomainUnderTest" is a subdomain of "domain"
        /// </summary>
        /// <param name="domain">Domain name</param>
        /// <param name="subDomainUnderTest">Domain to be tested</param>
        /// <returns>true false</returns>
        private bool IsSubDomain(string domain, string subDomainUnderTest)
        {
            // domain is not in expected format (starting with a '.'),
            // probably is a comment, ignore.
            if ((domain != null) && (domain[0] != '.'))
            {
                return false;
            }

            string[] sourceSubDomains = domain.Split('.');
            string[] subDomainsUnderTest = subDomainUnderTest.Split('.');
            
            // subDomainUnderTest May be a parent domain or is another domain.
            if (sourceSubDomains.Length > subDomainsUnderTest.Length)
            {
                return false;
            }

            for (int i = sourceSubDomains.Length - 1, j = subDomainsUnderTest.Length - 1; i > 0; --i, --j)
            {
                if (sourceSubDomains[i] != subDomainsUnderTest[j])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// <para>Saves in-memory list of domains to disk, format of domain list file is simple:</para>
        /// <para>##### Section name</para>
        /// <para>.active.domain</para>
        /// <para>#.inactive.domain</para>
        /// </summary>
        private void Save()
        {
            FileStream streamOfDomains = new FileStream(this.fileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter domainsList = new StreamWriter(streamOfDomains);
            foreach (Section s in this.sections)
            {
                domainsList.WriteLine("##### " + s.Name);
                foreach (string domain in s.ActiveDomain)
                {
                    domainsList.WriteLine(domain);
                }

                foreach (string domain in s.InactiveDomain)
                {
                    string d = "#" + domain;
                    domainsList.WriteLine(d);
                }

                domainsList.WriteLine(string.Empty);
            }

            domainsList.Flush();
            domainsList.Close();
        }
    }
}
