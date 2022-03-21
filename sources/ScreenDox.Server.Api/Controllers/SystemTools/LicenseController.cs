
using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Security;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.SystemTools
{
    /// <summary>
    /// Manages License keys
    /// </summary>
    [RestrictAccessToRoles(UserRoles.SuperAdministrator)]
    public class LicenseController : ApiControllerWithValidationBase<LicenseKeyRequest>
    {
        private readonly ILicenseService _service;

        private readonly ITimeService _timeService;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<LicenseController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityLogService"></param>
        /// <param name="timeService"></param>
        public LicenseController(
            ILicenseService service,
            ISecurityLogService securityLogService,
            ITimeService timeService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }


        /// <summary>
        /// Get current state of visit settings
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/systemtools/license")]
        public IEnumerable<LicenseCertificateResponse> GetAll()
        {
            //return results
            var result = _service.GetAllProductLicenses()
                 .Select(x =>
                 {
                     return AutoMapper.Mapper.Map<LicenseCertificateResponse>(x);
                 })
                 .ToList();

            // set the only one active license
            bool hasActivatedLicense = false;

            // the assumption is that the list or sorted by date in desc order
            result.ForEach(x =>
            {
                if (x.ActivatedDate == null)
                {
                    x.IsLicenseExpired = false; // not activated - render as normal
                }
                else if (hasActivatedLicense)
                {
                    x.IsLicenseExpired = true;
                }
                else if (x.ExpirationDate > _timeService.GetLocalNow())
                {
                    x.IsLicenseExpired = false;
                    hasActivatedLicense = true;
                }
            });

            return result;
        }


        /// <summary>
        /// Registers new license key and activate license key
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/systemtools/license")]
        public IHttpActionResult Add(LicenseKeyRequest requestModel)
        {
            requestModel.ShouldNotBeNull();

            // validation
            Validate(requestModel);

            if (!string.IsNullOrEmpty(requestModel.LicenseActivationKey))
            {
                // activate key flow
                var license = _service.GetLicenseCertificate(requestModel.LicenseKey);
                if (license == null)
                {
                    ResponseDataFactory.ThrowBadRequestMessage(new string[]{
                        Resources.TextMessages.LicenseKey_InvalidKeyMessage
                    });
                }

                return ActivateLicenseKey(license.License.LicenseString, requestModel.LicenseActivationKey);
            }

            return RegisterLicenseKey(requestModel.LicenseKey);
        }


        private IHttpActionResult RegisterLicenseKey(string key)
        {
            var result = new ActivationLicenseCertificateResponse
            {
                LicenseString = key
            };

            var licenseProductResult = _service.RegisterProductLicense(key);

            if (licenseProductResult == LicenseService.RegisterProductLicenseResult.RegisteredNewLicenseKey)
            {
                result.ActivationRequestString = GenerateNewActivationRequestCode(key);
            }
            else if (licenseProductResult == LicenseService.RegisterProductLicenseResult.DuplicateLicenseKey)
            {
                // license key is in database.
                // resume activation if it was broken

                // check if it was activated
                var license = _service.GetLicenseCertificate(key);
                if (license.ActivationKey == null)
                {
                    result.ActivationRequestString = GenerateNewActivationRequestCode(key);
                }
                else
                {
                    // key already activated. Return an error 
                    ResponseDataFactory.ThrowBadRequestMessage(new string[]{
                    Resources.TextMessages.LicenseKey_DuplicateKeyMessage
                    });
                }
            }
            else if (licenseProductResult == LicenseService.RegisterProductLicenseResult.InvalidLicenseKey)
            {
                ResponseDataFactory.ThrowBadRequestMessage(new string[]{
                    Resources.TextMessages.LicenseKey_InvalidKeyMessage
                    });
            }

            return AcceptedContentResult(result);

        }

        private string GenerateNewActivationRequestCode(string licenseKey)
        {

            var activationRequestKey = _service.UpdateActivationRequestKey(licenseKey);
            if (string.IsNullOrEmpty(activationRequestKey))
            {
                throw new ApplicationException(Resources.TextMessages.LicenseKey_CreateActivationREquestKeyFailedMessage);
            }

            return activationRequestKey;
        }

        private IHttpActionResult ActivateLicenseKey(string licenseKey, string activationKey)
        {

            LicenseService.ActivateProductLicenseResult result = _service.ActivateLicense(licenseKey, activationKey);


            if (result == LicenseService.ActivateProductLicenseResult.InvalidActivationKey ||
                result == LicenseService.ActivateProductLicenseResult.None)
            {
                ResponseDataFactory.ThrowBadRequestMessage(new string[]{
                   Resources.TextMessages.Activation_InvalidKeyMessage
                    });
            }
            // result == LicenseService.ActivateProductLicenseResult.Activated


            return AcceptedContentResult(GetAll());
        }


        /// <summary>
        /// Delete license key
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/systemtools/license")]
        public IHttpActionResult Delete(LicenseKeyRequest requestModel)
        {
            requestModel.ShouldNotBeNull();

            // validation
            Validate(requestModel);

            try
            {
                _service.RemoveLicense(requestModel.LicenseKey);
            }
            catch(Exception ex)
            {
                _logger.Error("DELETE License key", ex);
                throw;

            }

            return Ok(GetAll());
        }



        protected override AbstractValidator<LicenseKeyRequest> GetValidator()
        {
            return new LicenseKeyRequestItemRequestValidator();
        }
    }
}
