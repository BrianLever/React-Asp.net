using Common.Logging;
using Frontdesk.Server.SmartExport.SmartExtentions;
using FrontDesk;
using RPMS.Common.Models;
using ScreenDox.EHR.Common.SmartExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Server.SmartExport.Services.Testing
{
    public class ExportTestResultComparer 
    {
        ILog _logger;
        public ExportTestResultComparer(ScreeningResult original, Patient patient, SmartLookupResult<Visit> visit, ILog logger)
        {
            Original = original;
            Patient = patient;
            Visit = visit;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public ScreeningResult Original { get; }
        public Patient Patient { get; }
        public SmartLookupResult<Visit> Visit { get; }

        public string Result { get; set; }

        public bool Compare()
        {

            if(Original.ExportedToPatientID != Patient.ID)
            {
                Result = "Patient ID does not match (Original {0}; Exported: {1}; HRN: {2}; Name: {3})"
                    .FormatWith(
                        Original.ExportedToPatientID,
                        Patient.ID,
                        Patient.EHR,
                        Patient.FullName);
                return false;
            }

            if (Original.ExportedToHRN != Patient.EHR)
            {
                Result = "Patient EHR does not match (Original {0}; Exported: {1}; Name: {2})"
                    .FormatWith(
                        Original.ExportedToHRN,
                        Patient.EHR,
                        Patient.FullName);
                return false;
            }

            if (Original.ExportedToVisitID != Visit.BestResult.ID)
            {
                Result = "Visit ID does not match (Original {0}; Exported: {1}; Original Date: {2}; Exported Date: {3}; Category: {4}, Name: {5}, All Options: [{6}])"
                    .FormatWith(
                        Original.ExportedToVisitID,
                        Visit.BestResult.ID,
                        Original.ExportedToVisitDate,
                        Visit.BestResult.Date,
                        Visit.BestResult.ServiceCategory,
                        Patient.FullName.AsMaskedFullName(),
                        string.Join("|", Visit.AllResuls.Select(x => "{0}, {1}, {2}".FormatWith(x.ID, x.Date, x.ServiceCategory)))
                        );
                return false;
            }

            if (Original.ExportedToVisitDate != Visit.BestResult.Date)
            {
                Result = "Visit Date does not match (Original {0}; Exported: {1}; Date: {2}; Category: {3}, Name: {5})"
                   .FormatWith(
                       Original.ExportedToVisitDate,
                       Visit.BestResult.Date,
                       Visit.BestResult.ServiceCategory,
                       Patient.FullName);
                return false;
            }

            Result = "Matched (Patient ID: {0}; Name: {1}; Visit ID: {2})".FormatWith(Patient.ID, Patient.FullName.AsMaskedFullName(), Visit.BestResult.ID);
            return true;
        }
    }
}
