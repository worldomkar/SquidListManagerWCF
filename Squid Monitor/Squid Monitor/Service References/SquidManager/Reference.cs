﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
    [System.Runtime.Serialization.DataContractAttribute(Name="DomainsList", Namespace="http://schemas.datacontract.org/2004/07/SquidManager")]
    [System.SerializableAttribute()]
    public partial class DomainsList : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Squid_Monitor.SquidManager.Section[] sectionsField;
        
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
        public Squid_Monitor.SquidManager.Section[] sections {
            get {
                return this.sectionsField;
            }
            set {
                if ((object.ReferenceEquals(this.sectionsField, value) != true)) {
                    this.sectionsField = value;
                    this.RaisePropertyChanged("sections");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="Section", Namespace="http://schemas.datacontract.org/2004/07/SquidManager")]
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SquidManager.ISquidManager")]
    public interface ISquidManager {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/GetNewDomains", ReplyAction="http://tempuri.org/ISquidManager/GetNewDomainsResponse")]
        Squid_Monitor.SquidManager.DomainsList GetNewDomains();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/GetNewDomains", ReplyAction="http://tempuri.org/ISquidManager/GetNewDomainsResponse")]
        System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DomainsList> GetNewDomainsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/AddNewDomain", ReplyAction="http://tempuri.org/ISquidManager/AddNewDomainResponse")]
        void AddNewDomain(string domain, string listType, string sectionName, string activeInactive);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/AddNewDomain", ReplyAction="http://tempuri.org/ISquidManager/AddNewDomainResponse")]
        System.Threading.Tasks.Task AddNewDomainAsync(string domain, string listType, string sectionName, string activeInactive);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/GetTrustList", ReplyAction="http://tempuri.org/ISquidManager/GetTrustListResponse")]
        Squid_Monitor.SquidManager.DomainsList GetTrustList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/GetTrustList", ReplyAction="http://tempuri.org/ISquidManager/GetTrustListResponse")]
        System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DomainsList> GetTrustListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/GetBlockList", ReplyAction="http://tempuri.org/ISquidManager/GetBlockListResponse")]
        Squid_Monitor.SquidManager.DomainsList GetBlockList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/GetBlockList", ReplyAction="http://tempuri.org/ISquidManager/GetBlockListResponse")]
        System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DomainsList> GetBlockListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/ReloadLists", ReplyAction="http://tempuri.org/ISquidManager/ReloadListsResponse")]
        void ReloadLists();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISquidManager/ReloadLists", ReplyAction="http://tempuri.org/ISquidManager/ReloadListsResponse")]
        System.Threading.Tasks.Task ReloadListsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISquidManagerChannel : Squid_Monitor.SquidManager.ISquidManager, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SquidManagerClient : System.ServiceModel.ClientBase<Squid_Monitor.SquidManager.ISquidManager>, Squid_Monitor.SquidManager.ISquidManager {
        
        public SquidManagerClient() {
        }
        
        public SquidManagerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SquidManagerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SquidManagerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SquidManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Squid_Monitor.SquidManager.DomainsList GetNewDomains() {
            return base.Channel.GetNewDomains();
        }
        
        public System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DomainsList> GetNewDomainsAsync() {
            return base.Channel.GetNewDomainsAsync();
        }
        
        public void AddNewDomain(string domain, string listType, string sectionName, string activeInactive) {
            base.Channel.AddNewDomain(domain, listType, sectionName, activeInactive);
        }
        
        public System.Threading.Tasks.Task AddNewDomainAsync(string domain, string listType, string sectionName, string activeInactive) {
            return base.Channel.AddNewDomainAsync(domain, listType, sectionName, activeInactive);
        }
        
        public Squid_Monitor.SquidManager.DomainsList GetTrustList() {
            return base.Channel.GetTrustList();
        }
        
        public System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DomainsList> GetTrustListAsync() {
            return base.Channel.GetTrustListAsync();
        }
        
        public Squid_Monitor.SquidManager.DomainsList GetBlockList() {
            return base.Channel.GetBlockList();
        }
        
        public System.Threading.Tasks.Task<Squid_Monitor.SquidManager.DomainsList> GetBlockListAsync() {
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
