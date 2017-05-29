using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using sqmgr;

namespace sqmgr_wcf_svc
{
    [DataContract]
    internal class Section
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<string> ActiveDomain;

        [DataMember]
        public List<string> InactiveDomain;
    }

    [DataContract]
    public class DList
    {
        [DataMember]
        List<Section> Sections;

        public DList()
        {
            Sections = new List<Section>();
        }

        public DList(sqmgr.DList dl)
        {
            Sections = new List<Section>();
            foreach (sqmgr.Section s in dl.GetSections())
            {
                Section S = new Section();
                S.ActiveDomain = s.ActiveDomain;
                S.InactiveDomain = s.InactiveDomain;
                S.Name = s.Name;
                Sections.Add(S);
            }
        }
    }

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class SqMgrSvc : ISqMgr
    {
        SquidMonitor sm;

        SqMgrSvc()
        {
            sm = new SquidMonitor();
        }

        public DList GetNewDomains()
        {
            sqmgr.DList dl = sm.GetNewDomains();
            DList retDl = new DList(dl);
            return retDl;
        }

        public DList GetTrustList()
        {
            DList TrustList = new DList(sm.GetTrustList());
            return TrustList;
        }

        public DList GetBlockList()
        {
            DList BlockList = new DList(sm.GetBlockList());
            return BlockList;
        }

        public void AddNewDomain(string domain, string listType, string sectionName, string activeInactive)
        {
            sm.AddNewDomain(domain, listType, sectionName, activeInactive);
        }

        public void ReloadLists()
        {
            sm.Reload();
        }
    }
}
