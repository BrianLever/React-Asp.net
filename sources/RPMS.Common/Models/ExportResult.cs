using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ScreenDox.EHR.Common.Properties;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ExportResult", Namespace = "http://www.screendox.com")]
    public class ExportResult
    {
        [DataMember]
        public string ActionName { get; set; }
        [DataMember]
        public bool IsSuccessful { get; set; }

        [DataMember]
        public ExportFault Fault { get; set; }

        public ExportResult()
        {
            this.IsSuccessful = true;
            Fault = null;
        }
    }

    [DataContract(Name = "ExportFault", Namespace = "http://www.screendox.com")]
    public class ExportFault
    {
        [DataMember]
        public string InfoMessage { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public ExportFaultType FaultType { get; set; }

        public ExportFault()
        {
            this.FaultType = ExportFaultType.Unknown;
            this.InfoMessage = ExportFaultMessages.ResourceManager.GetString(ExportFaultType.Unknown.ToString());
        }

        public ExportFault(ExportFaultType faultType)
        {
            this.FaultType = faultType;
            this.InfoMessage = ExportFaultMessages.ResourceManager.GetString(faultType.ToString());
        }

        public ExportFault(ExportFaultType faultType, string errorMessage) : this(faultType)
        {
            this.ErrorMessage = errorMessage;
        }
    }

    [DataContract]
    public enum ExportFaultType
    {
        [EnumMember]
        Unknown = 1,
        [EnumMember]
        PatientRecordUpdateFault,
        [EnumMember]
        HealthFactorFault,
        [EnumMember]
        ExamFault,
        [EnumMember]
        CrisisAlertFault,
        [EnumMember]
        ExportTaskIsEmpty,
        [EnumMember]
        ScreeningDataExportFault
    }     
}
