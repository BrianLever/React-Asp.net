﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FrontDesk.Kiosk.KioskEndpointService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DateTimeOffset", Namespace="http://schemas.datacontract.org/2004/07/System")]
    [System.SerializableAttribute()]
    internal partial struct DateTimeOffset : System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.DateTime DateTimeField;
        
        private short OffsetMinutesField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        internal System.DateTime DateTime {
            get {
                return this.DateTimeField;
            }
            set {
                if ((this.DateTimeField.Equals(value) != true)) {
                    this.DateTimeField = value;
                    this.RaisePropertyChanged("DateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        internal short OffsetMinutes {
            get {
                return this.OffsetMinutesField;
            }
            set {
                if ((this.OffsetMinutesField.Equals(value) != true)) {
                    this.OffsetMinutesField = value;
                    this.RaisePropertyChanged("OffsetMinutes");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="KioskPingMessage", Namespace="http://schemas.datacontract.org/2004/07/FrontDesk.Server.Services")]
    [System.SerializableAttribute()]
    internal partial class KioskPingMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IpAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string KioskAppVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short KioskIDField;
        
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
        internal string IpAddress {
            get {
                return this.IpAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.IpAddressField, value) != true)) {
                    this.IpAddressField = value;
                    this.RaisePropertyChanged("IpAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string KioskAppVersion {
            get {
                return this.KioskAppVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.KioskAppVersionField, value) != true)) {
                    this.KioskAppVersionField = value;
                    this.RaisePropertyChanged("KioskAppVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal short KioskID {
            get {
                return this.KioskIDField;
            }
            set {
                if ((this.KioskIDField.Equals(value) != true)) {
                    this.KioskIDField = value;
                    this.RaisePropertyChanged("KioskID");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="KioskPingMessage", Namespace="http://schemas.datacontract.org/2004/07/ScreenDox.Server.Common.Models")]
    [System.SerializableAttribute()]
    internal partial class KioskPingMessage1 : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IpAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string KioskAppVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short KioskIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SecretKeyField;
        
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
        internal string IpAddress {
            get {
                return this.IpAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.IpAddressField, value) != true)) {
                    this.IpAddressField = value;
                    this.RaisePropertyChanged("IpAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string KioskAppVersion {
            get {
                return this.KioskAppVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.KioskAppVersionField, value) != true)) {
                    this.KioskAppVersionField = value;
                    this.RaisePropertyChanged("KioskAppVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal short KioskID {
            get {
                return this.KioskIDField;
            }
            set {
                if ((this.KioskIDField.Equals(value) != true)) {
                    this.KioskIDField = value;
                    this.RaisePropertyChanged("KioskID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string SecretKey {
            get {
                return this.SecretKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.SecretKeyField, value) != true)) {
                    this.SecretKeyField = value;
                    this.RaisePropertyChanged("SecretKey");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ScreeningSectionAgeView", Namespace="http://www.frontdeskhealth.com")]
    [System.SerializableAttribute()]
    internal partial class ScreeningSectionAgeView : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsEnabledField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime LastModifiedDateUTCField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte MinimalAgeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ScreeningSectionIDField;
        
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
        internal bool IsEnabled {
            get {
                return this.IsEnabledField;
            }
            set {
                if ((this.IsEnabledField.Equals(value) != true)) {
                    this.IsEnabledField = value;
                    this.RaisePropertyChanged("IsEnabled");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal System.DateTime LastModifiedDateUTC {
            get {
                return this.LastModifiedDateUTCField;
            }
            set {
                if ((this.LastModifiedDateUTCField.Equals(value) != true)) {
                    this.LastModifiedDateUTCField = value;
                    this.RaisePropertyChanged("LastModifiedDateUTC");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal byte MinimalAge {
            get {
                return this.MinimalAgeField;
            }
            set {
                if ((this.MinimalAgeField.Equals(value) != true)) {
                    this.MinimalAgeField = value;
                    this.RaisePropertyChanged("MinimalAge");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        internal string ScreeningSectionID {
            get {
                return this.ScreeningSectionIDField;
            }
            set {
                if ((object.ReferenceEquals(this.ScreeningSectionIDField, value) != true)) {
                    this.ScreeningSectionIDField = value;
                    this.RaisePropertyChanged("ScreeningSectionID");
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
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.frontdeskhealth.com", ConfigurationName="KioskEndpointService.IKioskEndpoint")]
    internal interface IKioskEndpoint {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResultResponse")]
        System.Nullable<bool> SaveScreeningResult(FrontDesk.ScreeningResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult_v2Response")]
        System.Nullable<bool> SaveScreeningResult_v2(FrontDesk.ScreeningResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveDemographicsResults", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveDemographicsResultsResponse")]
        bool SaveDemographicsResults(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/PingResponse")]
        bool Ping(short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v2Response")]
        bool Ping_v2(FrontDesk.Kiosk.KioskEndpointService.KioskPingMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v3", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v3Response")]
        bool Ping_v3(FrontDesk.Kiosk.KioskEndpointService.KioskPingMessage1 message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettingsResponse")]
        FrontDesk.Kiosk.KioskEndpointService.ScreeningSectionAgeView[] GetModifiedAgeSettings(System.DateTime lastModifiedDateUTC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings_v2Response")]
        FrontDesk.Kiosk.KioskEndpointService.ScreeningSectionAgeView[] GetModifiedAgeSettings_v2(short kioskId, System.DateTime lastModifiedDateUTC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/TestKioskInstallation", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/TestKioskInstallationResponse")]
        bool TestKioskInstallation(string kioskKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "icsResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningPatientIdentityWithAddress))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningResult))]
        System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics(FrontDesk.ScreeningPatientIdentity patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics_v2Response")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningPatientIdentityWithAddress))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningResult))]
        System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics_v2(FrontDesk.ScreeningPatientIdentity patient, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedLookupValues", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedLookupValuesResponse")]
        System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetModifiedLookupValues(System.DateTime lastModifiedDateUTC, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetLookupValuesDeleteLog", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetLookupValuesDeleteLogResponse")]
        System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetLookupValuesDeleteLog(System.DateTime lastModifiedDateUTC, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/ValidatePatient", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/ValidatePatientResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(RPMS.Common.Models.Patient))]
        RPMS.Common.Models.PatientSearch ValidatePatient(RPMS.Common.Models.PatientSearch patient);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal interface IKioskEndpointChannel : FrontDesk.Kiosk.KioskEndpointService.IKioskEndpoint, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    internal partial class KioskEndpointClient : System.ServiceModel.ClientBase<FrontDesk.Kiosk.KioskEndpointService.IKioskEndpoint>, FrontDesk.Kiosk.KioskEndpointService.IKioskEndpoint {
        
        public KioskEndpointClient() {
        }
        
        public KioskEndpointClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KioskEndpointClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KioskEndpointClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KioskEndpointClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Nullable<bool> SaveScreeningResult(FrontDesk.ScreeningResult result) {
            return base.Channel.SaveScreeningResult(result);
        }
        
        public System.Nullable<bool> SaveScreeningResult_v2(FrontDesk.ScreeningResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog) {
            return base.Channel.SaveScreeningResult_v2(result, timeLog);
        }
        
        public bool SaveDemographicsResults(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog) {
            return base.Channel.SaveDemographicsResults(result, timeLog);
        }
        
        public bool Ping(short kioskID) {
            return base.Channel.Ping(kioskID);
        }
        
        public bool Ping_v2(FrontDesk.Kiosk.KioskEndpointService.KioskPingMessage message) {
            return base.Channel.Ping_v2(message);
        }
        
        public bool Ping_v3(FrontDesk.Kiosk.KioskEndpointService.KioskPingMessage1 message) {
            return base.Channel.Ping_v3(message);
        }
        
        public FrontDesk.Kiosk.KioskEndpointService.ScreeningSectionAgeView[] GetModifiedAgeSettings(System.DateTime lastModifiedDateUTC) {
            return base.Channel.GetModifiedAgeSettings(lastModifiedDateUTC);
        }
        
        public FrontDesk.Kiosk.KioskEndpointService.ScreeningSectionAgeView[] GetModifiedAgeSettings_v2(short kioskId, System.DateTime lastModifiedDateUTC) {
            return base.Channel.GetModifiedAgeSettings_v2(kioskId, lastModifiedDateUTC);
        }
        
        public bool TestKioskInstallation(string kioskKey) {
            return base.Channel.TestKioskInstallation(kioskKey);
        }
        
        public System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics(FrontDesk.ScreeningPatientIdentity patient) {
            return base.Channel.GetPatientScreeningFrequencyStatistics(patient);
        }
        
        public System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics_v2(FrontDesk.ScreeningPatientIdentity patient, short kioskID) {
            return base.Channel.GetPatientScreeningFrequencyStatistics_v2(patient, kioskID);
        }
        
        public System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetModifiedLookupValues(System.DateTime lastModifiedDateUTC, short kioskID) {
            return base.Channel.GetModifiedLookupValues(lastModifiedDateUTC, kioskID);
        }
        
        public System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetLookupValuesDeleteLog(System.DateTime lastModifiedDateUTC, short kioskID) {
            return base.Channel.GetLookupValuesDeleteLog(lastModifiedDateUTC, kioskID);
        }
        
        public RPMS.Common.Models.PatientSearch ValidatePatient(RPMS.Common.Models.PatientSearch patient) {
            return base.Channel.ValidatePatient(patient);
        }
    }
}