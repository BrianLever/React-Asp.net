using FrontDesk.Configuration;
using FrontDesk.Server.Data;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Server.Screening.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Services
{
    public class IndicatorReportService : IIndicatorReportService
    {
        private readonly IIndicatorReportRepository _indicatorReportRepository;

        public IndicatorReportService(IIndicatorReportRepository indicatorReportRepository)
        {
            if (indicatorReportRepository == null)
            {
                throw new ArgumentNullException("indicatorReportRepository");
            }

            _indicatorReportRepository = indicatorReportRepository;
        }


        public IndicatorReportService() : this(new IndicatorReportDb()) { }

        public IndicatorReportByAgeViewModel GetBhsIndicatorReportByAge(FrontDesk.Screening screeningInfo, SimpleFilterModel filter, int[] ageGroups, bool uniquePatientsMode)
        {
            if (screeningInfo == null)
            {
                throw new ArgumentNullException("screeningInfo", "screeningInfo is required");
            }

            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (ageGroups is null)
            {
                throw new ArgumentNullException(nameof(ageGroups));
            }

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date;
            }
            if (filter.StartDate.HasValue)
            {
                filter.StartDate = filter.StartDate.Value.Date;
            }


            var db = _indicatorReportRepository;
            var model = new IndicatorReportByAgeViewModel();

            db.Connect();
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();


                //get brief questions section

                var items =
                    (uniquePatientsMode ?
                        db.GetIndicatorReportUniquePatientsPositiveByAges(filter) :
                        db.GetIndicatorReportPositiveByAges(filter))
                    .Where(x => x.ScreeningSectionID != ScreeningFrequencyDescriptor.ContactFrequencyID &&
                        x.ScreeningSectionID != ScreeningSectionDescriptor.DrugOfChoice);



                model.BriefQuestionsSectionItems.AddRange(items.ToViewModel(ageGroups));
                model.ScreeningInfo = screeningInfo;


                //get screening sections details
                //tobacco questions first
                model.SectionsScoreLevelItems.AddRange(
                    (uniquePatientsMode ?
                        db.GetBhsIndicatorReportUniquePatientsByAgeForTCC(filter) :
                        db.GetBhsIndicatorReportByAgeForTCC(filter))
                    .ToViewModel(ageGroups));

                items = uniquePatientsMode ?
                    db.GetBhsIndicatorReportUniquePatientsByScoreLevelAndAge(filter) :
                    db.GetBhsIndicatorReportByScoreLevelAndAge(filter);

                model.SectionsScoreLevelItems.AddRange(items.ToViewModel(ageGroups));

                //depression phq-2 and phq-9

                var depressionScores = GetDepressionLikeItems(filter, uniquePatientsMode,
                    ScreeningSectionDescriptor.Depression, ScreeningSectionDescriptor.DepressionPhq2ID);

                model.SectionsScoreLevelItems.AddRange(depressionScores.ToViewModel(ageGroups));


                //thinking of death


                model.SectionsScoreLevelItems.AddRange((
                    uniquePatientsMode ?
                    db.GetBhsIndicatorReportUniquePatientsByScoreLevelAndAgeForThinkingOfDeath(filter) :
                    db.GetBhsIndicatorReportByScoreLevelAndAgeForThinkingOfDeath(filter)
                    ).ToViewModel(ageGroups));



                // anxiety GAD-2/7

                var anxietyScores = GetDepressionLikeItems(filter, uniquePatientsMode,
                    ScreeningSectionDescriptor.Anxiety, ScreeningSectionDescriptor.AnxietyGad2ID);

                model.SectionsScoreLevelItems.AddRange(anxietyScores.ToViewModel(ageGroups));


                db.StopConnectionSharing();
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
                throw new Exception("Failed to create BHI report", ex);
            }
            finally
            {
                db.Disconnect();
            }


            return model;
        }
        /// <summary>
        /// Handle GAD 2/7 and DAST 2/9 heading
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="uniquePatientsMode"></param>
        /// <param name="mainSectionID"></param>
        /// <param name="shortSectionID"></param>
        /// <returns></returns>
        private List<IndicatorReportByAgeItem> GetDepressionLikeItems(SimpleFilterModel filter,
            bool uniquePatientsMode, string mainSectionID, string shortSectionID)
        {
            var result = uniquePatientsMode ?
                    _indicatorReportRepository.GetBhsIndicatorReportUniquePatientsByScoreLevelAndAgeForDepressionLike(filter, mainSectionID, shortSectionID) :
                    _indicatorReportRepository.GetBhsIndicatorReportByScoreLevelAndAgeForDepressionLike(filter, mainSectionID, shortSectionID)
                    ;

            //removing all positive scores for Phq-2 because it always MINIMAL-NEGATIVE
            result = result.Where(x => x.ScreeningSectionID == mainSectionID
            || (x.ScreeningSectionID == shortSectionID && x.QuestionID == 0)).ToList();



            // adding XYZ-2 record of all previous was XXX-7/9 sections
            if (!result.Any(x => x.ScreeningSectionID == shortSectionID))
            {
                var minimalScoreLevel = result.First(x => x.QuestionID == 0);
                result.Add(
                    new IndicatorReportByAgeItem
                    {
                        ScreeningSectionID = ScreeningSectionDescriptor.AnxietyGad2ID,
                        QuestionID = minimalScoreLevel.QuestionID,
                        ScreeningSectionQuestion = minimalScoreLevel.ScreeningSectionQuestion,
                        ScreeningSectionIndicates = minimalScoreLevel.ScreeningSectionIndicates,
                        Age = 0,
                        PositiveCount = 0
                    });
            }

            return result;

        }

        /// <summary>
        /// Get BHI by Problem
        /// </summary>
        /// <param name="screeningInfo"></param>
        /// <param name="filter"></param>
        /// <param name="uniquePatientsMode"></param>
        /// <returns></returns>
        public IndicatorReportViewModel GetBhsIndicatorReportByProblem(FrontDesk.Screening screeningInfo, SimpleFilterModel filter, bool uniquePatientsMode)
        {
            if (screeningInfo == null)
            {
                throw new ArgumentNullException("screeningInfo", "screeningInfo is required");
            }

            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }


            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date;
            }
            if (filter.StartDate.HasValue)
            {
                filter.StartDate = filter.StartDate.Value.Date;
            }


            var db = _indicatorReportRepository;
            var model = new IndicatorReportViewModel();

            db.Connect();
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();


                //get brief questions section

                var items =
                    (uniquePatientsMode ?
                        db.GetUniqueBhsIndicatorReport_v2(filter) :
                        db.GetBhsIndicatorReport_v2(filter))
                    .Where(x => x.ScreeningSectionID != ScreeningFrequencyDescriptor.ContactFrequencyID);



                model.BriefQuestionsSectionItems.AddRange(items);
                model.ScreeningInfo = screeningInfo;


                //get screening sections details
                //tobacco questions first
                model.SectionsScoreLevelItems.AddRange(
                    (uniquePatientsMode ?
                        db.GetUniqueBhsPositiveNagativeIndicatorReportForTCC(filter) :
                        db.GetBhsPositiveNagativeIndicatorReportForTCC(filter)));

                items = uniquePatientsMode ?
                    db.GetUniqueBhsIndicatorReportByScoreLevel(filter) :
                    db.GetBhsIndicatorReportByScoreLevel(filter);

                model.SectionsScoreLevelItems.AddRange(items);

                //depression phq-2 and phq-9

                var depressionScores = GetDepressionLikeItemsByScoreLevel(filter, uniquePatientsMode,
                    ScreeningSectionDescriptor.Depression, ScreeningSectionDescriptor.DepressionPhq2ID);

                model.SectionsScoreLevelItems.AddRange(depressionScores);


                //thinking of death


                model.SectionsScoreLevelItems.AddRange(
                    uniquePatientsMode ?
                    db.GetUniqueBhsIndicatorReportByScoreLevelForThinkingOfDeath(filter) :
                    db.GetBhsIndicatorReportByScoreLevelForThinkingOfDeath(filter)
                    );



                // anxiety GAD-2/7

                var anxietyScores = GetDepressionLikeItemsByScoreLevel(filter, uniquePatientsMode,
                    ScreeningSectionDescriptor.Anxiety, ScreeningSectionDescriptor.AnxietyGad2ID);

                model.SectionsScoreLevelItems.AddRange(anxietyScores);


                db.StopConnectionSharing();
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
                throw new Exception("Failed to create BHI report", ex);
            }
            finally
            {
                db.Disconnect();
            }


            return model;
        }

        public List<IndicatorReportItem> GetDepressionLikeItemsByScoreLevel(SimpleFilterModel filter,
            bool uniquePatientsMode, string mainSectionID, string shortSectionID)
        {
            var result = uniquePatientsMode ?
                    _indicatorReportRepository.GetUniqueBhsIndicatorReportByScoreLevelForDepressionLike(filter, mainSectionID, shortSectionID) :
                    _indicatorReportRepository.GetBhsIndicatorReportByScoreLevelForDepressionLike(filter, mainSectionID, shortSectionID)
                    ;

            //removing all positive scores for Phq-2 because it always MINIMAL-NEGATIVE
            result = result.Where(x => x.ScreeningSectionID == mainSectionID
            || (x.ScreeningSectionID == shortSectionID && x.QuestionId == 0)).ToList();



            // adding XYZ-2 record of all previous was XXX-7/9 sections
            if (!result.Any(x => x.ScreeningSectionID == shortSectionID))
            {
                var minimalScoreLevel = result.First(x => x.QuestionId == 0);
                result.Add(
                    new IndicatorReportItem
                    {
                        ScreeningSectionID = ScreeningSectionDescriptor.AnxietyGad2ID,
                        QuestionId = minimalScoreLevel.QuestionId,
                        ScreeningSectionQuestion = minimalScoreLevel.ScreeningSectionQuestion,
                        ScreeningSectionIndicates = minimalScoreLevel.ScreeningSectionIndicates,
                        PositiveCount = 0
                    });
            }

            return result;

        }



        public IndicatorReportByAgeViewModel GetDrugsOfChoiceByAge(FrontDesk.Screening screeningInfo, SimpleFilterModel filter, int[] ageGroups, bool uniquePatientsMode)
        {
            if (screeningInfo == null)
            {
                throw new ArgumentNullException("screeningInfo", "screeningInfo is required");
            }

            if (filter is null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (ageGroups is null)
            {
                throw new ArgumentNullException(nameof(ageGroups));
            }

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date;
            }
            if (filter.StartDate.HasValue)
            {
                filter.StartDate = filter.StartDate.Value.Date;
            }


            var db = _indicatorReportRepository;
            var model = new IndicatorReportByAgeViewModel();

            db.Connect();
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();


                //get brief questions section

                var items =
                    (uniquePatientsMode ?
                        db.GetBhsDrugsofChoiceUniquePatientsByAge(filter) :
                        db.GetBhsDrugsofChoiceByAge(filter));

                model.ScreeningInfo = screeningInfo;
                model.SectionsScoreLevelItems.AddRange(items.ToViewModel(ageGroups));

                db.StopConnectionSharing();
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
                throw new Exception("Failed to create BHI report", ex);
            }
            finally
            {
                db.Disconnect();
            }


            return model;
        }



    }

}
