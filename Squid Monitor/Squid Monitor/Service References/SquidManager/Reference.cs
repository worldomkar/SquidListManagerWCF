﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.36366
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Squid_Monitor.SquidManager {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DList", Namespace="http://schemas.datacontract.org/2004/07/sqmgr_wcf_svc")]
    [System.SerializableAttribute()]
    public partial class DList : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Squid_Monitor.SquidManager.Section[] SectionsField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Squid_Monitor.SquidManager.Section[] Sections {
            get {
                return this.SectionsField;
            }
            set {
                if ((object.ReferenceEquals(this.SectionsField, value) != true)) {
                    this.SectionsField = value;
                    this.RaisePropertyChanged("Sections");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Section", Namespace="http://schemas.datacontract.org/2004/07/sqmgr_wcf_svc")]
    [System.SerializableAttribute()]
    public partial class Section : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] ActiveDomainField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] InactiveDomainField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] ActiveDomain {
            get {
                return this.ActiveDomainField;
            }
            set {
                if ((object.ReferenceEquals(this.ActiveDomainField, value) != true)) {
                    this.ActiveDomainField = value;
                    this.RaisePropertyChanged("ActiveDomain");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] InactiveDomain {
            get {
                return this.InactiveDomainField;
            }
            set {
                if ((object.ReferenceEquals(this.InactiveDomainField, value) != true)) {
                    this.InactiveDomainField = value;
                    this.RaisePropertyChanged("InactiveDomain");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SquidManager.ISqMgr")]
    public interface ISqMgr {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/GetNewDomains", ReplyAction="http://tempuri.org/ISqMgr/GetNewDomainsResponse")]
        Squid_Monitor.SquidManager.DList GetNewDomains();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/GetNewDomains", ReplyAction="http://tempuri.org/ISqMgr/GetNewDomainsResponse")]
        System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DList> GetNewDomainsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/AddNewDomain", ReplyAction="http://tempuri.org/ISqMgr/AddNewDomainResponse")]
        void AddNewDomain(string domain, string listType, string sectionName, string activeInactive);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/AddNewDomain", ReplyAction="http://tempuri.org/ISqMgr/AddNewDomainResponse")]
        System.Threading.Tasks.Task AddNewDomainAsync(string domain, string listType, string sectionName, string activeInactive);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/GetTrustList", ReplyAction="http://tempuri.org/ISqMgr/GetTrustListResponse")]
        Squid_Monitor.SquidManager.DList GetTrustList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/GetTrustList", ReplyAction="http://tempuri.org/ISqMgr/GetTrustListResponse")]
        System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DList> GetTrustListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/GetBlockList", ReplyAction="http://tempuri.org/ISqMgr/GetBlockListResponse")]
        Squid_Monitor.SquidManager.DList GetBlockList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/GetBlockList", ReplyAction="http://tempuri.org/ISqMgr/GetBlockListResponse")]
        System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DList> GetBlockListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/ReloadLists", ReplyAction="http://tempuri.org/ISqMgr/ReloadListsResponse")]
        void ReloadLists();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISqMgr/ReloadLists", ReplyAction="http://tempuri.org/ISqMgr/ReloadListsResponse")]
        System.Threading.Tasks.Task ReloadListsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISqMgrChannel : Squid_Monitor.SquidManager.ISqMgr, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SqMgrClient : System.ServiceModel.ClientBase<Squid_Monitor.SquidManager.ISqMgr>, Squid_Monitor.SquidManager.ISqMgr {
        
        public SqMgrClient() {
        }
        
        public SqMgrClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SqMgrClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SqMgrClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SqMgrClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Squid_Monitor.SquidManager.DList GetNewDomains() {
            return base.Channel.GetNewDomains();
        }
        
        public System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DList> GetNewDomainsAsync() {
            return base.Channel.GetNewDomainsAsync();
        }
        
        public void AddNewDomain(string domain, string listType, string sectionName, string activeInactive) {
            base.Channel.AddNewDomain(domain, listType, sectionName, activeInactive);
        }
        
        public System.Threading.Tasks.Task AddNewDomainAsync(string domain, string listType, string sectionName, string activeInactive) {
            return base.Channel.AddNewDomainAsync(domain, listType, sectionName, activeInactive);
        }
        
        public Squid_Monitor.SquidManager.DList GetTrustList() {
            return base.Channel.GetTrustList();
        }
        
        public System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DList> GetTrustListAsync() {
            return base.Channel.GetTrustListAsync();
        }
        
        public Squid_Monitor.SquidManager.DList GetBlockList() {
            return base.Channel.GetBlockList();
        }
        
        public System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DList> GetBlockListAsync() {
            return base.Channel.GetBlockListAsync();
        }
        
        public void ReloadLists() {
            base.Channel.ReloadLists();
        }
        
        public System.Threading.Tasks.Task ReloadListsAsync() {
            return base.Channel.ReloadListsAsync();
        }
    }
}
