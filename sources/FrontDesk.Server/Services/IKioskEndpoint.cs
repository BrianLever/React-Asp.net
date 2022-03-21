using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;

using FrontDesk.Common;
using FrontDesk.Common.Bhservice.Import;
using FrontDesk.Common.Screening;

using RPMS.Common.Models;

namespace FrontDesk.Server.Services
{
    [Obsolete]
    // NOTE: If you change the interface name "IKioskEndpoint" here, you must also update the reference to "IKioskEndpoint" in Web.config.
    [ServiceContract(Namespace = "http://www.frontdeskhealth.com")]
    public interface IKioskEndpoint
    {
        [OperationContract]
        bool? SaveScreeningResult_v2(ScreeningResult result, ScreeningTimeLogRecord[] timeLog);

        [OperationContract]
        bool SaveDemographicsResults(PatientDemographicsKioskResult result, ScreeningTimeLogRecord[] timeLog);

        [OperationContract]
        bool Ping_v3(ScreenDox.Server.Common.Models.KioskPingMessage message);

        [OperationContract]
        List<ScreeningSectionAgeView> GetModifiedAgeSettings_v2(short kioskId, DateTime lastModifiedDateUTC);

        [OperationContract]
        bool TestKioskInstallation(string kioskKey);

        [OperationContract]
        Dictionary<string, int> GetPatientScreeningFrequencyStatistics_v2(ScreeningPatientIdentity patient, short kioskID);

        [OperationContract]
        Dictionary<string, List<LookupValue>> GetModifiedLookupValues(DateTime lastModifiedDateUTC, short kioskID);

        [OperationContract]
        Dictionary<string, List<LookupValue>> GetLookupValuesDeleteLog(DateTime lastModifiedDateUTC, short kioskID);


        [OperationContract]
        PatientSearch ValidatePatient(PatientSearch patient);
    }
}
