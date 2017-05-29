using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace sqmgr
{
    public class Section
    {
        public string Name { get; set; }
        public List<string> ActiveDomain;
        public List<string> InactiveDomain;
        public Section()
        {
            ActiveDomain = new List<string>();
            InactiveDomain = new List<string>();
        }
    }

    public class DList
    {   
        List <Section> Sections;
        string FileName;

        public List<Section> GetSections()
        {
            return Sections;
        }

        public DList()
        {
            Sections = new List<Section>();
        }

        public DList(string fileName)
        {
            Sections = new List<Section>();
            Load(fileName);
        }

        public void Reload()
        {
            Sections = null;
            Sections = new List<Section>();
            if (null != FileName)
                Load(FileName);
        }

        public void Load (string fileName)
        {
            FileStream fsDomainList = null;
            try
            {
                fsDomainList = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileName = fileName;
            }
            catch (Exception e)
            {
            }
            StreamReader srDomainList = new StreamReader(fsDomainList);
            string line;
            Section s = new Section();
            s.Name = "Global";
            Sections.Add(s);
            try
            {
                while (null != (line = srDomainList.ReadLine()))
                {
                    line.Trim();
                    if (0 == line.Length)
                        continue;
                    if ("#####" == line.Substring(0, 5))
                    {
                        if ((0 != s.ActiveDomain.Count) || (0 != s.InactiveDomain.Count))
                        {
                            s = new Section();
                            Sections.Add(s);
                        }
                        s.Name = line.Substring(5).Trim();
                    }
                    else
                        if ('#' == line[0])
                        {
                            s.InactiveDomain.Add(line.Substring(1).Trim());
                        }
                        else
                        {
                            s.ActiveDomain.Add(line);
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        bool isSubDomain(string domain, string subDomainUnderTest)
        {
            if ((null != domain) && (domain[0] != '.'))     // domain is not in expected format (starting with a '.'),
                return false;                               // probably is a comment, ignore.
            string[] dDomains = domain.Split('.');
            string[] sdDomains = subDomainUnderTest.Split('.');
            if (dDomains.Length > sdDomains.Length)     // subDomainUnderTest May be a parent domain or is another domain.
                return false;
            for (int i = dDomains.Length-1, j = sdDomains.Length-1; i > 0; --i, --j)
                if (dDomains[i] != sdDomains[j])
                    return false;

            return true;
        }

        public bool isKnown(string domain)
        {
            foreach (Section s in Sections)
            {
                try
                {
                    if ((null != s.ActiveDomain.Find(d => (d == domain)?true: isSubDomain(d, domain)) ||
                        (null != s.InactiveDomain.Find(d => (d == domain)?true: isSubDomain(d, domain)))))
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                }
            }
            return false;
        }

        public void AddDomain (string domain)
        {
            if (0 == Sections.Count)
            {
                Sections.Add(new Section());
                Sections[0].Name = "Global";
            }
            Sections[0].InactiveDomain.Add(domain);
        }

        public void RemoveDomain(string domain)
        {
            foreach (Section s in Sections)
            {
                if (null != s.ActiveDomain.Find(x => (x == domain)))
                {
                    s.ActiveDomain.Remove(domain);
                    break;
                }
                if (null != s.InactiveDomain.Find(x => (x == domain)))
                {
                    s.InactiveDomain.Remove(domain);
                    break;
                }
            }
        }
        
        void Save()
        {
            FileStream fsDomainList = new FileStream(FileName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter swDomainList = new StreamWriter(fsDomainList);
            foreach (Section s in Sections)
            {
                swDomainList.WriteLine("##### " + s.Name);
                foreach (string domain in s.ActiveDomain)
                {
                    swDomainList.WriteLine(domain);
                }
                foreach (string domain in s.InactiveDomain)
                {
                    string d = "#" + domain;
                    swDomainList.WriteLine(d);
                }
                swDomainList.WriteLine("");
            }
            swDomainList.Flush();
            swDomainList.Close();
        }

        public void AddNewDomain (string domain, string sectionName, string activeInactive)
        {
            try
            {
                if (isKnown(domain))        // Skip known domain/sub-domains addition.
                    return;

                Section s = Sections.Find(x => x.Name == sectionName);
                if (null == s)
                {
                    s = new Section();
                    s.Name = sectionName;
                    Sections.Add(s);
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
                Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
