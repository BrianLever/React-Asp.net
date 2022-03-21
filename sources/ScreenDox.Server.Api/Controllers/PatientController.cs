using AutoMapper;

using Common.Logging;

using Frontdesk.Server.SmartExport.EhrInterfaceService;

using FrontDesk;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Models.ViewModels.Validators;
using ScreenDox.Server.Security;
using ScreenDox.Server.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    public class PatientController : ApiController
    {
        private readonly IScreeningResultService _screeningResultService;
        private readonly IPatientService _patientService;
        private readonly IUserPrincipalService _userService;
        private readonly ISecurityLogService _securityLogService;
        private readonly IEhrInterfaceProxy _proxy;

        private readonly ILog _logger = LogManager.GetLogger<ScreenController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        public PatientController(IScreeningResultService screeningResultService,
                                 IUserPrincipalService userService,
                                 ISecurityLogService securityLogService,
                                 IEhrInterfaceProxy proxy,
                                 IPatientService patientService)
        {
            _screeningResultService = screeningResultService ?? throw new ArgumentNullException(nameof(screeningResultService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _proxy = proxy ?? throw new ArgumentNullException(nameof(proxy));
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        }


        /// <summary>
        /// Update patient info in screening
        /// </summary>
        /// <param name="id">Screendox Id (Screening Result ID).</param>
        /// <param name="patientInfo">Patient info</param>
        /// <returns>Returns ID of the created visit. If visit already exists, it returns it's Id.</returns>
        [HttpPost]
        [Route("api/patient/{id}")]
        public IHttpActionResult UpdatePatient([FromUri] long id, UpdatePatientRequest patientInfo)
        {
            id.ShouldNotBeDefault();
            patientInfo.ShouldNotBeNull();

            var principle = _userService.GetCurrent();
            var patient = _screeningResultService.Get(id);

            patient.ShouldNotBeNull(id);
            Validate(patientInfo);

            var updatedPatient = Mapper.Map<UpdatePatientRequest, ScreeningResult>(patientInfo, patient);

            try
            {
                //Update screening result
                _screeningResultService.Update(updatedPatient);
                _securityLogService.Add(new SecurityLog(SecurityEvents.EditPatientContactInformation,
                    updatedPatient.ID,
                    patient.LocationID));

                _proxy.ResetCache4GetMatchedPatients(patient);
            }
            catch (Exception ex)
            {
                _logger.Error("[Patient Update] Failed to update patient info.", ex);

                ResponseDataFactory.ThrowInvalidOperationError("Failed to update patient contact information.");
            }

            return Ok(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("Patient contact information"));
        }

        /// <summary>
        /// Search existing patient records in Screendox and EHR and return matched records from both sources
        /// EHR source is called only First name is 
        /// </summary>
        /// <param name="filter">Filter.
        /// Required fields:
        /// - Last name
        /// - Date of Birth
        /// Optional:
        ///  - First Name. If first name is provided and no matches in the Screendox database, the EHR patient database is called.
        /// </param>
        [HttpPost]
        [Route("api/patient/search")]
        public PatientSearchResponse Search([FromBody] PatientSearchFilter filter)
        {
            filter.ShouldNotBeNull();
            Validate(filter);

            var result = new PatientSearchResponse();

            var tasks = new List<Task<List<PatientSearchInfoMatch>>>();
            // find records in Screendox
            tasks.Add(FindMatchedPatientsInScreendox(filter));
            // find records in EHR
            tasks.Add(FindMatchedPatientsInEhr(filter));

            try
            {
                var searchResults = Task.WhenAll(tasks).Result.SelectMany(x => x).ToList();

                // agregate results
                // add all screendox items
                result.Screendox.Items = searchResults.Where(x => !x.IsEhrSource).ToList();
                result.Screendox.TotalCount = result.Screendox.Items.Count;

                // include only those EHR records that does not exist in Screendox
                var ehrResults = searchResults.Where(x => x.IsEhrSource).ToList();
                var screendoxResults = result.Screendox.Items;

                // birthday is always the same, do not check this field
                result.Ehr.Items = ehrResults.Where(ehr => !screendoxResults.Any(sdx =>
                    string.Compare(sdx.FullName, ehr.FullName, StringComparison.OrdinalIgnoreCase) == 0 &&
                    string.Compare(sdx.StreetAddress, ehr.StreetAddress, StringComparison.OrdinalIgnoreCase) == 0 &&
                    string.Compare(sdx.City, ehr.City, StringComparison.OrdinalIgnoreCase) == 0 &&
                    string.Compare(sdx.StateID, ehr.StateID, StringComparison.OrdinalIgnoreCase) == 0
                    )).ToList();
                result.Ehr.TotalCount = result.Ehr.Items.Count;
            }
            catch (AggregateException ex)
            {
                _logger.Error("Failed to search patients before adding new record.", ex.InnerException);

                ResponseDataFactory.DependencyError("EHR service is unavailable. Please try again later or contact Administrator.");
            }

            return result;
        }

        private Task<List<PatientSearchInfoMatch>> FindMatchedPatientsInScreendox(PatientSearchFilter filter)
        {
            return Task.Run(() => _patientService.FindPatient(filter)); ;
        }

        private Task<List<PatientSearchInfoMatch>> FindMatchedPatientsInEhr(PatientSearchFilter filter)
        {
            var patientInfo = new ScreeningResult
            {
                ID = 0,
                LastName = filter.LastName,
                FirstName = filter.FirstName,
                MiddleName = filter.MiddleName,
                Birthday = filter.Birthday
            };


            return Task.Run(() => _proxy.GetMatchedPatients(patientInfo, 0, 100)
            .Where( x =>
            {
                // filter additional fields 
                bool valid = true;

                if(!string.IsNullOrEmpty(filter.FirstName))
                {
                    if (string.IsNullOrEmpty(x.FirstName))
                    {
                        valid = false;
                    }
                    else
                    {
                        valid &= x.FirstName.StartsWith(filter.FirstName);
                    }
                }

                if (!string.IsNullOrEmpty(filter.MiddleName))
                {
                    if (string.IsNullOrEmpty(x.MiddleName))
                    {
                        valid = false;
                    }
                    else
                    {
                        valid &= x.MiddleName.StartsWith(filter.MiddleName);
                    }
                }

                return valid;
            })     
            .Select(x =>
                 {
                     var item = Mapper.Map<PatientSearchInfoMatch>(x);
                     item.SetNotMatchesFields(patientInfo);
                     return item;
                 }
                 ).ToList());
        }

        protected void Validate(UpdatePatientRequest request)
        {

            var result = new UpdatePatientRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }

        protected void Validate(PatientSearchFilter request)
        {

            var result = new PatientSearchFilterValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }


    }
}