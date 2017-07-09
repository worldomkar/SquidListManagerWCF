//-----------------------------------------------------------------------
// <copyright file="SquidManager.cs" company="_">
// SquidManager
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.ServiceModel;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// SquidManager implements ISquidManager interface and exposes services
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class SquidManager : ISquidManager
    {
        // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    
        /// <summary>
        /// List of Trusted domains
        /// </summary>
        private DomainsList trustList;

        /// <summary>
        /// List of Blocked domains
        /// </summary>
        private DomainsList blockList;

        /// <summary>
        /// List of newly found domains from denied log
        /// </summary>
        private DomainsList newDomains = new DomainsList();

        /// <summary>
        /// State flag to indicate Lists have been loaded from disk
        /// </summary>
        private volatile bool isLoaded = false;

        /// <summary>
        /// Log file monitor thread
        /// </summary>
        private Thread monitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SquidManager"/> class.
        /// </summary>
        public SquidManager()
        {
            this.monitor = new Thread(this.DeniedLogMonitorWorker);
            this.monitor.Start();
        }

        /// <summary>
        /// Re-loads all lists from disk, ignoring all the domains in memory
        /// </summary>
        public void ReloadLists()
        {
            // Uses squid to use external changes
            this.isLoaded = false;
            this.trustList.Reload();
            this.blockList.Reload();
            this.SquidReconfigure();
            this.isLoaded = true;
        }

        /// <summary>
        /// SquidMonitor worker thread to monitor denied log for new domains
        /// </summary>
        public void DeniedLogMonitorWorker()
        {
            try
            {
                this.LoadLists();

                FileStream streamOfAccessLog = new FileStream("c:\\squid\\var\\logs\\access.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamOfAccessLog.Seek(0, SeekOrigin.End);
                StreamReader accessLog = new StreamReader(streamOfAccessLog);
                string line, domain;
                string[] tokens, hostname;
                while (true)
                {
                    while ((line = accessLog.ReadLine()) != null)
                    {
                        // trim multiple contiguous whitespaces to single space
                        line = Regex.Replace(line, @"\s+", " ");

                        // read space separated tokens
                        tokens = line.Split(' ');

                        // filter for "CONNECT" tokens -- these are HTTPS and other tokens
                        if ((tokens.Length > 6) && (tokens[5].CompareTo("CONNECT") == 0))
                        {
                            domain = tokens[6];
                            hostname = domain.Split(':', ' ');
                            if (hostname[0][0] != '.')
                            {
                                hostname[0] = "." + hostname[0];
                            }

                            if (!this.IsKnown(hostname[0]))
                            {
                                if (!this.newDomains.IsKnown(hostname[0]))
                                {
                                    this.newDomains.AddDomain(hostname[0]);
                                    ////Console.WriteLine("New domain: " + hostname[0]);
                                }
                            }
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Returns list of new unique uncategorized domains
        /// </summary>
        /// <returns>List of new unique uncategorized domains</returns>
        public DomainsList GetNewDomains()
        {
            while (!this.isLoaded)
            {
                Thread.Sleep(100);
            }

            return this.newDomains;
        }

        /// <summary>
        /// Adds uncategorized domain to either of trust or block list
        /// <para>Also helps move one domain name from one domain list to another domain list</para>
        /// </summary>
        /// <param name="domain">Domain name</param>
        /// <param name="listType">Trust or Block list type</param>
        /// <param name="sectionName">Target Section name</param>
        /// <param name="activeInactive">Desired state active or inactive</param>
        public void AddNewDomain(string domain, string listType, string sectionName, string activeInactive)
        {
            try
            {
                bool isAdded = true;
                switch (listType.ToLower())
                {
                    case "trust":
                        this.blockList.RemoveDomain(domain);
                        this.trustList.AddNewDomain(domain, sectionName, activeInactive);
                        break;
                    case "block":
                        this.trustList.RemoveDomain(domain);
                        this.blockList.AddNewDomain(domain, sectionName, activeInactive);
                        break;
                    default:
                        isAdded = false;
                        break;
                }

                if (isAdded)
                {
                    this.newDomains.RemoveDomain(domain);
                    this.SquidReconfigure();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Returns current trust list
        /// </summary>
        /// <returns>Domains List</returns>
        public DomainsList GetTrustList()
        {
            return this.trustList;
        }

        /// <summary>
        /// Returns current block list
        /// </summary>
        /// <returns>Domains List</returns>
        public DomainsList GetBlockList()
        {
            return this.blockList;
        }

        /// <summary>
        /// Loads Trust and Block lists
        /// </summary>
        private void LoadLists()
        {
            this.trustList = new DomainsList("c:\\squid\\etc\\trusted-sites.txt");
            this.blockList = new DomainsList("c:\\squid\\etc\\blacklist.txt");
            this.isLoaded = true;
        }

        /// <summary>
        /// Returns true if domain is known in (active or inactive sections of) trustList or blockList
        /// </summary>
        /// <param name="domain">Domain name</param>
        /// <returns>true false</returns>
        private bool IsKnown(string domain)
        {
            return this.trustList.IsKnown(domain) || this.blockList.IsKnown(domain);
        }

        /// <summary>
        /// Tell Squid to reload on-disk domain lists
        /// </summary>
        private void SquidReconfigure()
        {
            Process reloadSquid = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,

                    ////RedirectStandardOutput = true,
                    ////RedirectStandardError = true,
                    CreateNoWindow = true,
                    FileName = "c:\\squid\\sbin\\squid.exe",
                    Arguments = "-n squid -k reconfigure"
                }
            };
            reloadSquid.Start();

            ////string output = reloadSquid.StandardOutput.ReadToEnd();
            reloadSquid.WaitForExit();
        }
    }
}
