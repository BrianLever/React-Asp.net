
using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Membership;

using RPMS.Common.Security;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Security;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels.Validators;

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
    public class RpmsCredentialsController : ApiControllerWithValidationBase<RpmsCredentialsRequest>
    {
        private readonly IRpmsCredentialsService _service;

        private readonly ITimeService _timeService;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<RpmsCredentialsController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityLogService"></param>
        /// <param name="timeService"></param>
        public RpmsCredentialsController(
            IRpmsCredentialsService service,
            ISecurityLogService securityLogService,
            ITimeService timeService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }


        /// <summary>
        /// Get the list of EHR (RPMS) Credentials
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/systemtools/rpmscredentials")]
        public IEnumerable<RpmsCredentialsResponse> GetAll()
        {
            //return results
            var result = _service.GetAllCredentials().Select(x =>
            {
                x.VerifyCode = x.VerifyCode.AsMaskedPassword();

                return Mapper.Map<RpmsCredentialsResponse>(x);
            }).ToArray() ;

            if (result.Any())
            {
                var activeCredentials = result.FirstOrDefault(x => x.ExpireAt >= _timeService.GetLocalNow());
                if(activeCredentials != null)
                {
                    activeCredentials.IsActive = true;
                }

            }
            return result;
        }


        /// <summary>
        /// Registers new EHR (RPMS) credentials
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/systemtools/rpmscredentials")]
        public IHttpActionResult Add(RpmsCredentialsRequest request)
        {
            request.ShouldNotBeNull();

            // validation
            Validate(request);

            _service.AddCredentials(new RpmsCredentials
            {
                AccessCode = request.AccessCode.Trim(),
                VerifyCode = request.VerifyCode.Trim(),
                ExpireAt = request.ExpireAt.Value
            });

            return Ok();
        }

        /// <summary>
        /// Delete EHR(RPMS) credential
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/systemtools/rpmscredentials/{id}")]
        public IHttpActionResult Delete(Guid id)
        {
            id.ShouldNotBeDefault();



            var credentials = _service.GetAllCredentials().FirstOrDefault(x => id == x.Id);

            credentials.ShouldNotBeNull();

            try
            {
                _service.DeleteCredentials(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to delete EHR Credentials", ex);
                throw;
            }

            return Ok(GetAll());
        }



        protected override AbstractValidator<RpmsCredentialsRequest> GetValidator()
        {
            return new RpmsCredentialsRequestValidator();
        }
    }
}
