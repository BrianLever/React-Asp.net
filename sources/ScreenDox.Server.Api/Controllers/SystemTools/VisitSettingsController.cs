
using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.SystemTools
{
    /// <summary>
    /// Manages Visit creation system settings
    /// </summary>
    public class VisitSettingsController : ApiControllerWithValidationBase<VisitSettingItemRequest>
    {
        private readonly IVisitSettingsService _service;

        private readonly IScreeningScoreLevelService _screeningScoreLevelService;
        private readonly ITimeService _timeService;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<VisitSettingsController>();

        private Lazy<List<ScreeningSectionScoreHint>> ScoreLevelHints;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="screeningScoreLevelService"></param>
        /// <param name="securityLogService"></param>
        /// <param name="timeService"></param>
        public VisitSettingsController(
            IVisitSettingsService service,
            IScreeningScoreLevelService screeningScoreLevelService,
            ISecurityLogService securityLogService,
            ITimeService timeService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _screeningScoreLevelService = screeningScoreLevelService ?? throw new ArgumentNullException(nameof(screeningScoreLevelService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

            // init lazy initializer for Score Hints
            ScoreLevelHints = new Lazy<List<ScreeningSectionScoreHint>>(() => GetScoreLevelHints(), true);
        }


        /// <summary>
        /// Get current state of visit settings
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        public IEnumerable<VisitSettingItemViewModel> Get()
        {
            //return results
            var result = _service.GetAll()
                 .Select(x =>
                 {
                     var model = AutoMapper.Mapper.Map<VisitSettingItemViewModel>(x);
                     model.SectionScoreHint = GetScreeningSectionScoreHint(x.Id);

                     return model;
                 })
                 .ToList();

            return result;
        }

        /// <summary>
        /// Get score hint text for Measure tool id
        /// </summary>
        /// <param name="visitSectionId"></param>
        /// <returns></returns>
        protected string GetScreeningSectionScoreHint(string visitSectionId)
        {
            if (visitSectionId == VisitSettingsDescriptor.Depression)
            {
                visitSectionId = ScreeningSectionDescriptor.Depression;
            }


            var scoreLevel = ScoreLevelHints.Value
                .FirstOrDefault(x => x.ScreeningSectionID == visitSectionId);
            return scoreLevel != null ? scoreLevel.ScoreHint : string.Empty;
        }

        private List<ScreeningSectionScoreHint> GetScoreLevelHints()
        {
            const string key = "VisitSettingsForm_ScoreLevelHints";

            var cache = MemoryCache.Default;

            var cacheEntry = cache.Get(key);
            List<ScreeningSectionScoreHint> result;

            if (cacheEntry == null)
            {
                result = _screeningScoreLevelService.GetAllScoreHints();

                var cachePolicy = new CacheItemPolicy();
                cachePolicy.AbsoluteExpiration = _timeService.GetDateTimeOffsetNow().AddMinutes(5);
                cache.Add(key, result, cachePolicy);
            }
            else
            {
                result = (List<ScreeningSectionScoreHint>)cacheEntry ?? new List<ScreeningSectionScoreHint>();
            }

            return result;
        }

        /// <summary>
        /// Update or create screening profile frequency settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPut]
        public IHttpActionResult Add(VisitSettingItemRequest requestModel)
        {
            requestModel.ShouldNotBeNull();

            // validation
            Validate(requestModel);

            return AcceptedContentResult(Save(requestModel));
        }

        /// <summary>
        /// Apply changes
        /// </summary>
        /// <param name="updateItems"></param>
        /// <returns></returns>
        private IEnumerable<VisitSettingItemViewModel> Save(VisitSettingItemRequest updateItems)
        {

            _service.Update(updateItems.Items);

            _logger.InfoFormat("Visit Settings have been updated. Data: {0}.", updateItems.ToJson());
            return Get();

        }



        protected override AbstractValidator<VisitSettingItemRequest> GetValidator()
        {
            return new VisitSettingItemRequestValidator();
        }
    }
}
