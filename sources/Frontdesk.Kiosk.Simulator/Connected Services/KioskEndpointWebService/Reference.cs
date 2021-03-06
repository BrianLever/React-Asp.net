//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Frontdesk.Kiosk.Simulator.KioskEndpointWebService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="KioskPingMessage", Namespace="http://schemas.datacontract.org/2004/07/FrontDesk.Server.Services")]
    [System.SerializableAttribute()]
    public partial class KioskPingMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
        public string IpAddress {
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
        public string KioskAppVersion {
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
        public short KioskID {
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
    public partial class KioskPingMessage1 : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
        public string IpAddress {
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
        public string KioskAppVersion {
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
        public short KioskID {
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
        public string SecretKey {
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
    public partial class ScreeningSectionAgeView : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
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
        public bool IsEnabled {
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
        public System.DateTime LastModifiedDateUTC {
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
        public byte MinimalAge {
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
        public string ScreeningSectionID {
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PatientSearch", Namespace="http://www.screendox.com")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.Patient))]
    public partial class PatientSearch : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DateOfBirthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MiddleNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int matchRankField;
        
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
        public System.DateTime DateOfBirth {
            get {
                return this.DateOfBirthField;
            }
            set {
                if ((this.DateOfBirthField.Equals(value) != true)) {
                    this.DateOfBirthField = value;
                    this.RaisePropertyChanged("DateOfBirth");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FirstName {
            get {
                return this.FirstNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstNameField, value) != true)) {
                    this.FirstNameField = value;
                    this.RaisePropertyChanged("FirstName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ID {
            get {
                return this.IDField;
            }
            set {
                if ((this.IDField.Equals(value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastName {
            get {
                return this.LastNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LastNameField, value) != true)) {
                    this.LastNameField = value;
                    this.RaisePropertyChanged("LastName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MiddleName {
            get {
                return this.MiddleNameField;
            }
            set {
                if ((object.ReferenceEquals(this.MiddleNameField, value) != true)) {
                    this.MiddleNameField = value;
                    this.RaisePropertyChanged("MiddleName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int matchRank {
            get {
                return this.matchRankField;
            }
            set {
                if ((this.matchRankField.Equals(value) != true)) {
                    this.matchRankField = value;
                    this.RaisePropertyChanged("matchRank");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="Patient", Namespace="http://www.screendox.com")]
    [System.SerializableAttribute()]
    public partial class Patient : Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CityField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EHRField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhoneHomeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhoneOfficeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StateIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StreetAddressField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ZipCodeField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string City {
            get {
                return this.CityField;
            }
            set {
                if ((object.ReferenceEquals(this.CityField, value) != true)) {
                    this.CityField = value;
                    this.RaisePropertyChanged("City");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EHR {
            get {
                return this.EHRField;
            }
            set {
                if ((object.ReferenceEquals(this.EHRField, value) != true)) {
                    this.EHRField = value;
                    this.RaisePropertyChanged("EHR");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhoneHome {
            get {
                return this.PhoneHomeField;
            }
            set {
                if ((object.ReferenceEquals(this.PhoneHomeField, value) != true)) {
                    this.PhoneHomeField = value;
                    this.RaisePropertyChanged("PhoneHome");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhoneOffice {
            get {
                return this.PhoneOfficeField;
            }
            set {
                if ((object.ReferenceEquals(this.PhoneOfficeField, value) != true)) {
                    this.PhoneOfficeField = value;
                    this.RaisePropertyChanged("PhoneOffice");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StateID {
            get {
                return this.StateIDField;
            }
            set {
                if ((object.ReferenceEquals(this.StateIDField, value) != true)) {
                    this.StateIDField = value;
                    this.RaisePropertyChanged("StateID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StreetAddress {
            get {
                return this.StreetAddressField;
            }
            set {
                if ((object.ReferenceEquals(this.StreetAddressField, value) != true)) {
                    this.StreetAddressField = value;
                    this.RaisePropertyChanged("StreetAddress");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ZipCode {
            get {
                return this.ZipCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ZipCodeField, value) != true)) {
                    this.ZipCodeField = value;
                    this.RaisePropertyChanged("ZipCode");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.frontdeskhealth.com", ConfigurationName="KioskEndpointWebService.IKioskEndpoint")]
    public interface IKioskEndpoint {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResultResponse")]
        System.Nullable<bool> SaveScreeningResult(FrontDesk.ScreeningResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResultResponse")]
        System.Threading.Tasks.Task<System.Nullable<bool>> SaveScreeningResultAsync(FrontDesk.ScreeningResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult_v2Response")]
        System.Nullable<bool> SaveScreeningResult_v2(FrontDesk.ScreeningResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveScreeningResult_v2Response")]
        System.Threading.Tasks.Task<System.Nullable<bool>> SaveScreeningResult_v2Async(FrontDesk.ScreeningResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveDemographicsResults", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveDemographicsResultsResponse")]
        bool SaveDemographicsResults(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/SaveDemographicsResults", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/SaveDemographicsResultsResponse")]
        System.Threading.Tasks.Task<bool> SaveDemographicsResultsAsync(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/PingResponse")]
        bool Ping(short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/PingResponse")]
        System.Threading.Tasks.Task<bool> PingAsync(short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v2Response")]
        bool Ping_v2(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v2Response")]
        System.Threading.Tasks.Task<bool> Ping_v2Async(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v3", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v3Response")]
        bool Ping_v3(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage1 message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v3", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/Ping_v3Response")]
        System.Threading.Tasks.Task<bool> Ping_v3Async(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage1 message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettingsResponse")]
        Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[] GetModifiedAgeSettings(System.DateTime lastModifiedDateUTC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettingsResponse")]
        System.Threading.Tasks.Task<Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[]> GetModifiedAgeSettingsAsync(System.DateTime lastModifiedDateUTC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings_v2Response")]
        Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[] GetModifiedAgeSettings_v2(short kioskId, System.DateTime lastModifiedDateUTC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedAgeSettings_v2Response")]
        System.Threading.Tasks.Task<Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[]> GetModifiedAgeSettings_v2Async(short kioskId, System.DateTime lastModifiedDateUTC);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/TestKioskInstallation", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/TestKioskInstallationResponse")]
        bool TestKioskInstallation(string kioskKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/TestKioskInstallation", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/TestKioskInstallationResponse")]
        System.Threading.Tasks.Task<bool> TestKioskInstallationAsync(string kioskKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "icsResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningPatientIdentityWithAddress))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningResult))]
        System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics(FrontDesk.ScreeningPatientIdentity patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "icsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> GetPatientScreeningFrequencyStatisticsAsync(FrontDesk.ScreeningPatientIdentity patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics_v2Response")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningPatientIdentityWithAddress))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(FrontDesk.ScreeningResult))]
        System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics_v2(FrontDesk.ScreeningPatientIdentity patient, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics_v2", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetPatientScreeningFrequencyStatist" +
            "ics_v2Response")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> GetPatientScreeningFrequencyStatistics_v2Async(FrontDesk.ScreeningPatientIdentity patient, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedLookupValues", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedLookupValuesResponse")]
        System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetModifiedLookupValues(System.DateTime lastModifiedDateUTC, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedLookupValues", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetModifiedLookupValuesResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]>> GetModifiedLookupValuesAsync(System.DateTime lastModifiedDateUTC, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetLookupValuesDeleteLog", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetLookupValuesDeleteLogResponse")]
        System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetLookupValuesDeleteLog(System.DateTime lastModifiedDateUTC, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/GetLookupValuesDeleteLog", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/GetLookupValuesDeleteLogResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]>> GetLookupValuesDeleteLogAsync(System.DateTime lastModifiedDateUTC, short kioskID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/ValidatePatient", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/ValidatePatientResponse")]
        Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch ValidatePatient(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch patient);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.frontdeskhealth.com/IKioskEndpoint/ValidatePatient", ReplyAction="http://www.frontdeskhealth.com/IKioskEndpoint/ValidatePatientResponse")]
        System.Threading.Tasks.Task<Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch> ValidatePatientAsync(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch patient);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IKioskEndpointChannel : Frontdesk.Kiosk.Simulator.KioskEndpointWebService.IKioskEndpoint, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KioskEndpointClient : System.ServiceModel.ClientBase<Frontdesk.Kiosk.Simulator.KioskEndpointWebService.IKioskEndpoint>, Frontdesk.Kiosk.Simulator.KioskEndpointWebService.IKioskEndpoint {
        
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
        
        public System.Threading.Tasks.Task<System.Nullable<bool>> SaveScreeningResultAsync(FrontDesk.ScreeningResult result) {
            return base.Channel.SaveScreeningResultAsync(result);
        }
        
        public System.Nullable<bool> SaveScreeningResult_v2(FrontDesk.ScreeningResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog) {
            return base.Channel.SaveScreeningResult_v2(result, timeLog);
        }
        
        public System.Threading.Tasks.Task<System.Nullable<bool>> SaveScreeningResult_v2Async(FrontDesk.ScreeningResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog) {
            return base.Channel.SaveScreeningResult_v2Async(result, timeLog);
        }
        
        public bool SaveDemographicsResults(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog) {
            return base.Channel.SaveDemographicsResults(result, timeLog);
        }
        
        public System.Threading.Tasks.Task<bool> SaveDemographicsResultsAsync(FrontDesk.Common.Bhservice.Import.PatientDemographicsKioskResult result, FrontDesk.Common.Screening.ScreeningTimeLogRecord[] timeLog) {
            return base.Channel.SaveDemographicsResultsAsync(result, timeLog);
        }
        
        public bool Ping(short kioskID) {
            return base.Channel.Ping(kioskID);
        }
        
        public System.Threading.Tasks.Task<bool> PingAsync(short kioskID) {
            return base.Channel.PingAsync(kioskID);
        }
        
        public bool Ping_v2(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage message) {
            return base.Channel.Ping_v2(message);
        }
        
        public System.Threading.Tasks.Task<bool> Ping_v2Async(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage message) {
            return base.Channel.Ping_v2Async(message);
        }
        
        public bool Ping_v3(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage1 message) {
            return base.Channel.Ping_v3(message);
        }
        
        public System.Threading.Tasks.Task<bool> Ping_v3Async(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.KioskPingMessage1 message) {
            return base.Channel.Ping_v3Async(message);
        }
        
        public Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[] GetModifiedAgeSettings(System.DateTime lastModifiedDateUTC) {
            return base.Channel.GetModifiedAgeSettings(lastModifiedDateUTC);
        }
        
        public System.Threading.Tasks.Task<Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[]> GetModifiedAgeSettingsAsync(System.DateTime lastModifiedDateUTC) {
            return base.Channel.GetModifiedAgeSettingsAsync(lastModifiedDateUTC);
        }
        
        public Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[] GetModifiedAgeSettings_v2(short kioskId, System.DateTime lastModifiedDateUTC) {
            return base.Channel.GetModifiedAgeSettings_v2(kioskId, lastModifiedDateUTC);
        }
        
        public System.Threading.Tasks.Task<Frontdesk.Kiosk.Simulator.KioskEndpointWebService.ScreeningSectionAgeView[]> GetModifiedAgeSettings_v2Async(short kioskId, System.DateTime lastModifiedDateUTC) {
            return base.Channel.GetModifiedAgeSettings_v2Async(kioskId, lastModifiedDateUTC);
        }
        
        public bool TestKioskInstallation(string kioskKey) {
            return base.Channel.TestKioskInstallation(kioskKey);
        }
        
        public System.Threading.Tasks.Task<bool> TestKioskInstallationAsync(string kioskKey) {
            return base.Channel.TestKioskInstallationAsync(kioskKey);
        }
        
        public System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics(FrontDesk.ScreeningPatientIdentity patient) {
            return base.Channel.GetPatientScreeningFrequencyStatistics(patient);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> GetPatientScreeningFrequencyStatisticsAsync(FrontDesk.ScreeningPatientIdentity patient) {
            return base.Channel.GetPatientScreeningFrequencyStatisticsAsync(patient);
        }
        
        public System.Collections.Generic.Dictionary<string, int> GetPatientScreeningFrequencyStatistics_v2(FrontDesk.ScreeningPatientIdentity patient, short kioskID) {
            return base.Channel.GetPatientScreeningFrequencyStatistics_v2(patient, kioskID);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, int>> GetPatientScreeningFrequencyStatistics_v2Async(FrontDesk.ScreeningPatientIdentity patient, short kioskID) {
            return base.Channel.GetPatientScreeningFrequencyStatistics_v2Async(patient, kioskID);
        }
        
        public System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetModifiedLookupValues(System.DateTime lastModifiedDateUTC, short kioskID) {
            return base.Channel.GetModifiedLookupValues(lastModifiedDateUTC, kioskID);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]>> GetModifiedLookupValuesAsync(System.DateTime lastModifiedDateUTC, short kioskID) {
            return base.Channel.GetModifiedLookupValuesAsync(lastModifiedDateUTC, kioskID);
        }
        
        public System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]> GetLookupValuesDeleteLog(System.DateTime lastModifiedDateUTC, short kioskID) {
            return base.Channel.GetLookupValuesDeleteLog(lastModifiedDateUTC, kioskID);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, FrontDesk.Common.LookupValue[]>> GetLookupValuesDeleteLogAsync(System.DateTime lastModifiedDateUTC, short kioskID) {
            return base.Channel.GetLookupValuesDeleteLogAsync(lastModifiedDateUTC, kioskID);
        }
        
        public Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch ValidatePatient(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch patient) {
            return base.Channel.ValidatePatient(patient);
        }
        
        public System.Threading.Tasks.Task<Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch> ValidatePatientAsync(Frontdesk.Kiosk.Simulator.KioskEndpointWebService.PatientSearch patient) {
            return base.Channel.ValidatePatientAsync(patient);
        }
    }
}
