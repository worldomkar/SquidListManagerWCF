using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;

namespace sqmgr
{
    public class SquidMonitor
    {
        DList TrustList;
        DList BlockList;

        public volatile bool isLoaded = false;
        void LoadLists()
        {
            TrustList = new DList("c:\\squid\\etc\\trusted-sites.txt");
            BlockList = new DList("c:\\squid\\etc\\blacklist.txt");

            isLoaded = true;
        }

        public void Reload()
        {
            // Uses squid to use external changes
            isLoaded = false;
            TrustList.Reload();
            BlockList.Reload();
            SquidReconfigure();
            isLoaded = true;
        }

        bool isKnown(string domain)
        {
            return (TrustList.isKnown(domain) || BlockList.isKnown(domain));
        }

        DList NewDomains = new DList();
        public void SqMonWorker()
        {
            try
            {
                LoadLists();

                FileStream fsAccessLog = new FileStream("c:\\squid\\var\\logs\\access.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fsAccessLog.Seek(0, SeekOrigin.End);
                StreamReader srAccessLog = new StreamReader(fsAccessLog);
                string line, domain;
                string[] Tokens, hostname;
                while (true)
                {
                    while (null != (line = srAccessLog.ReadLine()))
                    {
                        line = Regex.Replace(line, @"\s+", " ");
                        Tokens = line.Split(' ');
                        if ((Tokens.Length > 6) && (0 == Tokens[5].CompareTo("CONNECT")))
                        {
                            domain = Tokens[6];
                            hostname = domain.Split(':', ' ');
                            if ('.' != hostname[0][0])
                                hostname[0] = "." + hostname[0];
                            if (!isKnown(hostname[0]))
                            {
                                if (!NewDomains.isKnown(hostname[0]))
                                {
                                    NewDomains.AddDomain(hostname[0]);
                                    Console.WriteLine("New domain: " + hostname[0]);
                                }
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {

            }
        }

        public DList GetNewDomains()
        {
            while (!isLoaded)
                Thread.Sleep(100);
            return NewDomains;
        }

        Thread Monitor;
        public SquidMonitor()
        {
            Monitor = new Thread(SqMonWorker);
            Monitor.Start();
        }

        public void AddNewDomain(string domain, string listType, string sectionName, string activeInactive)
        {
            try
            {
                bool isAdded = true;
                switch (listType.ToLower())
                {
                    case "trust":
                        BlockList.RemoveDomain(domain);
                        TrustList.AddNewDomain(domain, sectionName, activeInactive);
                        break;
                    case "block":
                        TrustList.RemoveDomain(domain);
                        BlockList.AddNewDomain(domain, sectionName, activeInactive);
                        break;
                    default:
                        isAdded = false;
                        break;
                }
                if (isAdded)
                {
                    NewDomains.RemoveDomain(domain);
                    SquidReconfigure();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SquidReconfigure()
        {
            Process reloadSquid = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    //RedirectStandardOutput = true,
                    //RedirectStandardError = true,
                    CreateNoWindow = true,
                    FileName = "c:\\squid\\sbin\\squid.exe",
                    Arguments = "-n squid -k reconfigure"
                }
            };
            reloadSquid.Start();
            //string output = reloadSquid.StandardOutput.ReadToEnd();
            reloadSquid.WaitForExit();
        }

        public DList GetTrustList()
        {
            return TrustList;
        }

        public DList GetBlockList()
        {
            return BlockList;
        }
    }
}
