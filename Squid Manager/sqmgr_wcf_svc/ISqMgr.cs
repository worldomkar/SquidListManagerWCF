using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Services;

namespace sqmgr_wcf_svc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISqMgr
    {
        [OperationContract]
        DList GetNewDomains();

        [OperationContract]
        void AddNewDomain(string domain, string listType, string sectionName, string activeInactive);

        [OperationContract]
        DList GetTrustList();

        [OperationContract]
        DList GetBlockList();

        [OperationContract]
        void ReloadLists();
    }
}
