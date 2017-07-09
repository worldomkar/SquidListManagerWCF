//-----------------------------------------------------------------------
// <copyright file="ISquidManager.cs" company="_">
// SquidManager
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
// <copyright file="ISquidManager.cs" company="_">
// ISquidManager
// </copyright>
// <author>Omkar Patekar</author>
//-----------------------------------------------------------------------
namespace SquidManager
{
    using System.ServiceModel;

    [ServiceContract]
    public interface ISquidManager
    {
        [OperationContract]
        DomainsList GetNewDomains();

        [OperationContract]
        void AddNewDomain(string domain, string listType, string sectionName, string activeInactive);

        [OperationContract]
        DomainsList GetTrustList();

        [OperationContract]
        DomainsList GetBlockList();

        [OperationContract]
        void ReloadLists();
    }
}
