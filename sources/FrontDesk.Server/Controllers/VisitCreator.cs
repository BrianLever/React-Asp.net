using FrontDesk.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Configuration;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Common.Bhservice;
using System.Data;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Server.Controllers
{
    public class VisitCreator : IVisitCreator
    {
        private readonly IBhsVisitFactory _visitFactory;
        private readonly IVisitSettingsService _visitSettingsService;
        protected Lazy<VisitSettingItem[]> _enabledVisitSettingsConfigurationCache;

        public VisitCreator() : this(new VisitSettingsService(), new BhsVisitFactory())
        { }


        public VisitCreator(IVisitSettingsService visitSettingsService, IBhsVisitFactory visitFactory)
        {

            if (visitSettingsService == null) throw new ArgumentNullException(nameof(visitSettingsService));
            if (visitFactory == null) throw new ArgumentNullException(nameof(visitFactory));

            _visitSettingsService = visitSettingsService;
            _visitFactory = visitFactory;

            _enabledVisitSettingsConfigurationCache = new Lazy<VisitSettingItem[]>(() => _visitSettingsService.GetAll().Where(x => x.IsEnabled).ToArray());
        }

        public BhsVisit Create(ScreeningResult result, FrontDesk.Screening screeningInfo)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            var sectionResult = result.SectionAnswers;

            var simpleSectionRulesMatching = new List<Tuple<string, string>>(){
               Tuple.Create(ScreeningSectionDescriptor.SmokerInHome, VisitSettingsDescriptor.SmokerInHome),
               Tuple.Create(ScreeningSectionDescriptor.Alcohol, VisitSettingsDescriptor.Alcohol),
               Tuple.Create(ScreeningSectionDescriptor.SubstanceAbuse, VisitSettingsDescriptor.SubstanceAbuse),
               Tuple.Create(ScreeningSectionDescriptor.PartnerViolence, VisitSettingsDescriptor.PartnerViolence),
               Tuple.Create(ScreeningSectionDescriptor.Depression, VisitSettingsDescriptor.Depression),
               Tuple.Create(ScreeningSectionDescriptor.Anxiety, VisitSettingsDescriptor.Anxiety),
               Tuple.Create(ScreeningSectionDescriptor.ProblemGambling, VisitSettingsDescriptor.ProblemGambling),
            };

            if (simpleSectionRulesMatching.Any(x => CheckRuleSectionPositiveAndTriggerEnabled(x.Item1, x.Item2, result)))
            {
                return _visitFactory.Create(result, screeningInfo);
            }

            //check prticular sections answers
            //tobacco

            var simpleQuestionRulesMatching = new List<Tuple<string, int, string>>(){
               Tuple.Create(ScreeningSectionDescriptor.Tobacco, TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID, VisitSettingsDescriptor.TobaccoUseCeremony),
               Tuple.Create(ScreeningSectionDescriptor.Tobacco, TobaccoQuestionsDescriptor.DoYouSmokeQuestionID, VisitSettingsDescriptor.TobaccoUseSmoking),
               Tuple.Create(ScreeningSectionDescriptor.Tobacco, TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID, VisitSettingsDescriptor.TobaccoUseSmokeless),
               Tuple.Create(ScreeningSectionDescriptor.Depression, ScreeningSectionDescriptor.DepressionThinkOfDeathQuestionID, VisitSettingsDescriptor.DepressionThinkOfDeath),

            };

            if (simpleQuestionRulesMatching.Any(x => CheckRuleAnswerPositiveAndTriggerEnabled(x.Item1, x.Item2, x.Item3, result)))
            {
                return _visitFactory.Create(result, screeningInfo);
            }

            return null;
        }

        private bool CheckRuleSectionPositiveAndTriggerEnabled(string sectionId, string visitTriggerSettingId, ScreeningResult result)
        {

            var visitSetting = _enabledVisitSettingsConfigurationCache.Value.FirstOrDefault(x => x.Id == visitTriggerSettingId);

            bool triggerEnabled = visitSetting != null;

            int scoreThreshold = visitSetting != null ? visitSetting.CutScore : 1;


            var target = result.FindSectionByID(sectionId);
            
            if (triggerEnabled // when enabled
                && (target?.ScoreLevel ?? 0) > 0 // when positive
                && target?.Score >= scoreThreshold // when equal or greate than cut score threshold 
            )
            {
                return true;
            }

            return false;
        }

        private int? GetQuestionAnswer(string sectionId, int questionId, ScreeningResult result)
        {
            var target = result.FindSectionByID(sectionId);
            if (target == null) return null;

            return target.FindQuestionByID(questionId)?.AnswerValue;

        }

        private bool CheckRuleAnswerPositiveAndTriggerEnabled(string sectionId, int questionId, string visitTriggerSettingId, ScreeningResult result)
        {
            var questionValue = GetQuestionAnswer(sectionId, questionId, result) ?? 0;

            var visitSetting = _enabledVisitSettingsConfigurationCache.Value.FirstOrDefault(x => x.Id == visitTriggerSettingId);

            bool triggerEnabled = visitSetting != null;
            int scoreThreshold = visitSetting != null ? visitSetting.CutScore : 1;



            if (triggerEnabled // enabled
                && questionValue > 0 // positive
                && questionValue >= scoreThreshold
            )
            {
                return true;
            }

            return false;
        }

        

    }
}