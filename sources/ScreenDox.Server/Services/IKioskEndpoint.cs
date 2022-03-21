using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;

using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Bhservice.Import;
using FrontDesk.Common.Screening;
using FrontDesk.Server;

using RPMS.Common.Models;

namespace ScreenDox.Server.Services
{
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
