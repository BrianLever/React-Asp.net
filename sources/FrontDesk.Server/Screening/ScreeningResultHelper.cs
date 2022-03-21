using Common.Logging;

using FrontDesk.Common.Debugging;
using FrontDesk.Common.Extensions;
using FrontDesk.Configuration;
using FrontDesk.Server.Data;
using FrontDesk.Server.Messages;
using FrontDesk.Server.Screening.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FrontDesk.Server.Screening
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed - need for datasources on pages
    public static class ScreeningResultHelper
    {
        private static ILog _logger = LogManager.GetLogger("ScreeningResultHelper");
        /// <summary>
        /// Get new database object
        /// </summary>
        private static Data.ScreeningResultDb DbObject { get { return new ScreeningResultDb(); } }

        #region Save or remove screening info

        /// <summary>
        /// Add new screening result and calculate BHI
        /// </summary>
        public static long InsertScreeningResult(ScreeningResult screeningResult)
        {
            screeningResult.CreatedDate = DateTimeOffset.Now;

            if (screeningResult == null) throw new ArgumentNullException("screeningResult");

            //validate
            string error = "";
            List<string> errors = null;
            if (!ScreeningResultHelper.Validate(screeningResult, out errors))
            {
                error = String.Join(", ", errors.ToArray());

                FrontDesk.Server.Logging.ErrorLog.Add(
                        Resources.TextMessages.InvalidScreeningTraceMsg + ": " + error,
                        String.Empty,
                        screeningResult.KioskID == 0 ? (short?)null : screeningResult.KioskID);

                screeningResult.WithErrors = true;
                ScreeningResultHelper.TryResolve(screeningResult);
            }

            List<string> fatalErrors;
            if (!Validate(screeningResult, out fatalErrors))
            {
                throw new Common.Entity.NonValidEntityException(error);
            }

            var info = ServerScreening.GetByID(screeningResult.ScreeningID);

            if (_logger.IsDebugEnabled)
            {
                _logger.DebugFormat("Received valid screening Result.\r\n{0}",
                    screeningResult.AsMasked().ToJson());
            }

            UpdateSectionScoringValues(screeningResult, info);

            DbObject.InsertScreeningResult(screeningResult);
            return screeningResult.ID;
        }

        /// <summary>
        /// calculate score and score level for section answers
        /// </summary>
        /// <param name="screeningResult"></param>
        private static void UpdateSectionScoringValues(ScreeningResult screeningResult, FrontDesk.Screening info)
        {
            //calculate score and score level for section
            foreach (var section in screeningResult.SectionAnswers)
            {
                var scoring = ScreeningScoringFactory.CreateScreeningScoring(section, info.FindSectionByID(section.ScreeningSectionID));
                section.Score = scoring.Score;

                var sectionLevel = scoring.ScoreLevel;
                if (sectionLevel != null)
                {
                    section.ScoreLevel = sectionLevel.ScoreLevel;
                }
                else
                {
                    section.ScoreLevel = null;
                }


            }
        }
        [Obsolete("Migrated to ScreenService")]
        /// <summary>
        /// Delete screening result
        /// </summary>
        public static void Delete(long screeningResultID)
        {
            var visitDb = new Data.BhsVisits.BhsVisitDb();

            //check that there is no completed visit
            var visitId = visitDb.FindByScreeningResultId(screeningResultID);
            if (visitId.HasValue)
            {
                var visit = visitDb.Get(visitId.Value);

                if (visit != null && visit.IsCompleted)
                {
                    Trace.TraceWarning(TextStrings.SCREENING_RESULT_DELETEFAIL_VISIT_EXISTS.FormatWith(screeningResultID));
                    throw new ApplicationException(TextStrings.SCREENING_RESULT_DELETEFAIL_VISIT_EXISTS.FormatWith(screeningResultID));
                }

            }
            if (visitId.HasValue)
            {
                // Bhs visit has not been completed, so we can remove it
                visitDb.Delete(visitId.Value);
            }

            DbObject.Delete(screeningResultID);
        }

        public static void Update(ScreeningResult screeningResult)
        {
            DbObject.Update(screeningResult);
        }

        /// <summary>
        /// Mark report as exported and update Export related fields
        /// </summary>
        /// <param name="screeningResult"></param>
        public static void UpdateExportInfo(ScreeningResult screeningResult)
        {
            //validate model
            if (!screeningResult.ExportDate.HasValue)
            {
                throw new FrontDesk.Common.Entity.NonValidEntityException("Export date field is missing");
            }
            if (!screeningResult.ExportedBy.HasValue)
            {
                throw new FrontDesk.Common.Entity.NonValidEntityException("Exported by user ID field is missing");
            }
            if (string.IsNullOrWhiteSpace(screeningResult.ExportedToHRN))
            {
                throw new FrontDesk.Common.Entity.NonValidEntityException("Linked HRN value is missing");
            }
            if (!screeningResult.ExportedToPatientID.HasValue)
            {
                throw new FrontDesk.Common.Entity.NonValidEntityException("EHR Patient's record id is missing");
            }
            if (!screeningResult.ExportedToVisitID.HasValue)
            {
                throw new FrontDesk.Common.Entity.NonValidEntityException("EHR Visit record id is missing");
            }
            if (!screeningResult.ExportedToVisitDate.HasValue)
            {
                throw new FrontDesk.Common.Entity.NonValidEntityException("EHR Visit's date is missing");
            }
            if (string.IsNullOrWhiteSpace(screeningResult.ExportedToVisitLocation))
            {
                throw new FrontDesk.Common.Entity.NonValidEntityException("EHR Visit's location is missing");
            }

            DbObject.UpdateExportInfo(screeningResult);

        }

        #endregion


        #region Get Screening Results
        [Obsolete("Migrated to ScreenService")]
        /// <summary>
        /// Get screening results
        /// </summary>
        public static ScreeningResult GetScreeningResult(long screeningResultID)
        {
            var result = DbObject.GetScreeningResult(screeningResultID);

            //DebugLogger.WriteObjectInformation("Screening result", result);

            return result;
        }

        #endregion


        #region Check-In data for UI bound
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        public static DataSet GetUniquePatientCheckIns(
            string firstNameFilter,
            string lastNameFilter,
            long? screeningResultIdFilter,
            int? locationFilter,
            DateTime? startDateFilter,
            DateTime? endDateFilter,
            int startRowIndex,
            int maximumRows,
            string orderBy)
        {
            var filter = new ScreeningResultFilterModel
            {
                FirstName = firstNameFilter,
                LastName = lastNameFilter,
                ScreeningResultID = screeningResultIdFilter,
                Location = locationFilter,
                StartDate = startDateFilter,
                EndDate = endDateFilter,
                ProblemScoreFilter = null
            };


            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }
            _logger.Trace("[ScreeningResult][GetUniquePatientCheckIns] Method called.");
            return DbObject.GetLatestCheckinsForDisplay(filter, startRowIndex, maximumRows, orderBy);

        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        public static DataSet GetUniquePatientCheckInsByProblem(ScreeningResultFilterModel filter, int startRowIndex,
            int maximumRows,
            string orderBy)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }
            _logger.Trace("[ScreeningResult][GetUniquePatientCheckInsByProblem] Method called.");
            return DbObject.GetUniquePatientCheckInsByProblem(filter, startRowIndex, maximumRows, orderBy);
        }

        /// <summary>
        /// Get number of total rows to display
        /// </summary>
        public static int GetUniquePatientCheckInsCount(
            string firstNameFilter,
            string lastNameFilter,
            long? screeningResultIdFilter,
            int? locationFilter,
            DateTime? startDateFilter,
            DateTime? endDateFilter)
        {
            var filter = new ScreeningResultFilterModel
            {
                FirstName = firstNameFilter,
                LastName = lastNameFilter,
                ScreeningResultID = screeningResultIdFilter,
                Location = locationFilter,
                StartDate = startDateFilter,
                EndDate = endDateFilter,
                ProblemScoreFilter = null
            };

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            _logger.Trace("[ScreeningResult][GetUniquePatientCheckInsCount] Method called.");
            return DbObject.GetLatestCheckinsForDisplayCount(filter);
        }


        /// <summary>
        /// Get number of total rows to display
        /// </summary>
        public static int GetUniquePatientCheckInsByProblemCount(ScreeningResultFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            _logger.Trace("[ScreeningResult][GetUniquePatientCheckInsByProblemCount] Method called.");
            return DbObject.GetUniquePatientCheckInsByProblemCount(filter);
        }


        /// <summary>
        /// Get total number of check-in records in the database
        /// </summary>
        public static long GetTotalRecordCount()
        {

            return DbObject.GetTotalCount();
        }

        public static List<PatientCheckInViewModel> GetRelatedPatientScreenings(long mainRowID, ScreeningResultFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            _logger.Trace("[ScreeningResult][GetRelatedPatientScreenings] Method called.");
            List<PatientCheckInViewModel> result = DbObject.GetRelatedPatientScreenings(mainRowID, filter);

            return result;
        }

        public static List<PatientCheckInViewModel> GetRelatedPatientScreenings(long mainRowID, int? locationFilter, DateTime? startDateFilter, DateTime? endDateFilter, long? screeningResultID)
        {
            return GetRelatedPatientScreenings(mainRowID, new ScreeningResultFilterModel
            {
                Location = locationFilter,
                StartDate = startDateFilter,
                EndDate = endDateFilter,
                ScreeningResultID = screeningResultID
            });
        }

        #endregion

        #region Validation

        public static bool Validate(ScreeningResult result, out List<string> errors)
        {
            errors = new List<string>();

            #region 1. Validate user details

            if (string.IsNullOrWhiteSpace(result.FirstName))
            {
                errors.Add(Resources.TextMessages.ScreeningValidation_FirstNameMissing);
            }

            if (string.IsNullOrWhiteSpace(result.LastName))
            {
                errors.Add(Resources.TextMessages.ScreeningValidation_LastNameMissing);
            }

            if (result.Birthday.Equals(DateTime.MinValue) || result.Birthday.Date > DateTime.Now.Date)
            {
                errors.Add(Resources.TextMessages.ScreeningValidation_IvalidDateOfBirth);
            }

            if (!String.IsNullOrEmpty(result.StateID) && State.GetByStateCode(result.StateID) == null)
            {
                errors.Add(Resources.TextMessages.ScreeningValidation_InvalidState);
            }

            #region Deprecated code
            //Because of Contact screening frequency feature contact info can be null
            //if (result.StreetAddress == null || result.StreetAddress.Trim() == String.Empty)
            //{
            //    errors.Add(Resources.TextMessages.ScreeningValidation_AddressMissing);
            //}

            //if (result.City == null || result.City.Trim() == String.Empty)
            //{
            //    errors.Add(Resources.TextMessages.ScreeningValidation_CityMissing);
            //}

            //if (String.IsNullOrEmpty(result.StateID))
            //{
            //    errors.Add(Resources.TextMessages.ScreeningValidation_StateMissing);
            //}
            //else if (State.GetByStateCode(result.StateID) == null)
            //{
            //    errors.Add(Resources.TextMessages.ScreeningValidation_InvalidState);
            //}

            //if (String.IsNullOrEmpty(result.ZipCode))
            //{
            //    errors.Add(Resources.TextMessages.ScreeningValidation_ZipCodeMissing);
            //}

            //if (String.IsNullOrEmpty(result.Phone))
            //{
            //    errors.Add(Resources.TextMessages.ScreeningValidation_PhoneMissing);
            //}
            #endregion

            #endregion

            #region 2.Validate screeing results

            if (result.SectionAnswers == null)
            {
                errors.Add(Resources.TextMessages.ScreeningValidation_InvalidScreeningResult);
            }

            foreach (var section in result.SectionAnswers)
            {
                if (!ScreeningSectionDescriptor.KnownScreeningSections.Contains(section.ScreeningSectionID))
                {
                    errors.Add(Resources.TextMessages.ScreeningValidation_UnknownScreeningTool.FormatWith(section.ScreeningSectionID));
                }
            }

            #endregion

            return (errors == null || errors.Count == 0);

        }

        public static void TryResolve(ScreeningResult result)
        {
            string emptyString = Resources.TextMessages.EmptyButRequiredFiledText;
            string defaultStateCode = "AL";
            DateTime defaultBirthday = new DateTime(1900, 1, 1);

            if (result.SectionAnswers == null || result.SectionAnswers.Count != 5)
            {
                return;
            }


            if (result.FirstName == null || result.FirstName.Trim() == String.Empty)
            {
                result.FirstName = emptyString;
            }

            if (result.LastName == null || result.LastName.Trim() == String.Empty)
            {
                result.LastName = emptyString;
            }

            if (result.Birthday.Equals(DateTime.MinValue) || result.Birthday.Date > DateTime.Now.Date)
            {
                result.Birthday = defaultBirthday;
            }

            if (result.StreetAddress == null || result.StreetAddress.Trim() == String.Empty)
            {
                result.StreetAddress = emptyString;
            }

            if (result.City == null || result.City.Trim() == String.Empty)
            {
                result.City = emptyString;
            }

            if (String.IsNullOrEmpty(result.StateID))
            {
                result.StateID = defaultStateCode;
            }
            else if (State.GetByStateCode(result.StateID) == null)
            {
                result.StateID = defaultStateCode;
            }

            if (String.IsNullOrEmpty(result.ZipCode))
            {
                result.ZipCode = emptyString;
            }

            if (String.IsNullOrEmpty(result.Phone))
            {
                result.Phone = emptyString;
            }
        }


        #endregion

        #region For Administrative and Testing Needs

        //[AspNetHostingPermissionAttribute(SecurityAction.Deny, Level = AspNetHostingPermissionLevel.Minimal)]
        public static void PerformUpdateScreeningResultScores(string screeningID)
        {
            ScreeningResult result = null;
            var db = DbObject;
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();

                var idList = db.GetAllScreeningResultIDsForScreening(screeningID);

                foreach (var id in idList)
                {
                    result = db.GetScreeningResult(id);
                    if (result != null)
                    {
                        var info = ServerScreening.GetByID(result.ScreeningID);

                        UpdateSectionScoringValues(result, info);

                        foreach (var section in result.SectionAnswers)
                        {
                            db.UpdateScreeningSectionScore(section);
                        }
                    }
                }

                db.StopConnectionSharing();
                db.CommitTransaction();
            }
            catch (Exception)
            {
                db.StopConnectionSharing();
                db.RollbackTransaction();
            }
            finally
            {
                db.Disconnect();
            }
        }

        #endregion

        #region Indicator Report

        public static IndicatorReportViewModel GetBhsIndicatorReport(FrontDesk.Screening screeningInfo, SimpleFilterModel filter, bool uniquePatients)
        {
            if (screeningInfo == null)
            {
                throw new ArgumentNullException("screeningInfo", "screeningInfo is required");
            }

            _logger.DebugFormat("GetBhsIndicatorReport: Unique patients flag: {0}", uniquePatients);


            var db = new IndicatorReportDb(); ;
            IndicatorReportViewModel model = new IndicatorReportViewModel();

            db.Connect();
            try
            {
                db.BeginTransaction();
                db.StartConnectionSharing();

                model.TotalPatientScreenings = db.GetBhsIndicatorReportTotalRecords(filter);
                model.ScreeningInfo = screeningInfo;

                //get brief questions section
                model.BriefQuestionsSectionItems.AddRange(
                        (uniquePatients ?
                            db.GetUniqueBhsIndicatorReport_v2(filter) :
                            db.GetBhsIndicatorReport_v2(filter))
                                .Where(x => x.ScreeningSectionID != ScreeningFrequencyDescriptor.ContactFrequencyID)
                        ); //tobacco questions first

                //get screening sections details
                model.SectionsScoreLevelItems.AddRange(
                    uniquePatients ?
                    db.GetUniqueBhsPositiveNagativeIndicatorReportForTCC(filter) :
                    db.GetBhsPositiveNagativeIndicatorReportForTCC(filter)
                ); //tobacco questions first

                model.SectionsScoreLevelItems.AddRange(
                    uniquePatients ?
                    db.GetUniqueBhsIndicatorReportByScoreLevel(filter) :
                    db.GetBhsIndicatorReportByScoreLevel(filter)
                );
                #region depression
                //depression phq-2 and phq-9

                var depressionScores = uniquePatients ?
                   db.GetUniqueBhsIndicatorReportByScoreLevelForDepressionLike(filter,
                    ScreeningSectionDescriptor.Depression, ScreeningSectionDescriptor.DepressionPhq2ID) :
                   db.GetBhsIndicatorReportByScoreLevelForDepressionLike(filter,
                    ScreeningSectionDescriptor.Depression, ScreeningSectionDescriptor.DepressionPhq2ID)
               ;

                //removing all positive scores for Phq-2 because it always MINIMAL-NEGATIVE
                depressionScores = depressionScores.Where(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Depression
                || (x.ScreeningSectionID == ScreeningSectionDescriptor.DepressionPhq2ID && x.QuestionId == 0)).ToList();


                model.SectionsScoreLevelItems.AddRange(depressionScores);

                //thinking of death
                model.SectionsScoreLevelItems.AddRange(
                    uniquePatients ?
                    db.GetUniqueBhsIndicatorReportByScoreLevelForThinkingOfDeath(filter) :
                    db.GetBhsIndicatorReportByScoreLevelForThinkingOfDeath(filter)
                );
                #endregion

                #region anxiety

                // anxiety gad-2 and gad-7
                var anxietyScores = uniquePatients ?
                  db.GetUniqueBhsIndicatorReportByScoreLevelForDepressionLike(filter, ScreeningSectionDescriptor.Anxiety, ScreeningSectionDescriptor.AnxietyGad2ID) :
                  db.GetBhsIndicatorReportByScoreLevelForDepressionLike(filter, ScreeningSectionDescriptor.Anxiety, ScreeningSectionDescriptor.AnxietyGad2ID)
                ;

                //removing all positive scores for gad-7 because it always MINIMAL-NEGATIVE
                anxietyScores = anxietyScores.Where(x => x.ScreeningSectionID == ScreeningSectionDescriptor.Anxiety
                || (x.ScreeningSectionID == ScreeningSectionDescriptor.AnxietyGad2ID && x.QuestionId == 0)).ToList();

                // adding GAD-2 record of all previous was full sections
                if (!anxietyScores.Any(x => x.ScreeningSectionID == ScreeningSectionDescriptor.AnxietyGad2ID))
                {
                    var minimalScoreLevel = anxietyScores.First(x => x.QuestionId == 0);
                    anxietyScores.Add(
                        new IndicatorReportItem(
                            ScreeningSectionDescriptor.AnxietyGad2ID,
                            minimalScoreLevel.QuestionId,
                            minimalScoreLevel.ScreeningSectionQuestion,
                            minimalScoreLevel.ScreeningSectionIndicates,
                            0,
                            0));
                }

                model.SectionsScoreLevelItems.AddRange(anxietyScores);

                #endregion

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

            DebugLogger.WriteObjectInformation("GetBhsIndicatorReport result", model);

            return model;
        }





        /// <summary>
        /// Get date list from minimum screen result date to next year
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        public static List<int> GetDateListForReport()
        {
            List<int> dateList = new List<int>();
            //DateTimeOffset startYear = DbObject.GetMinDate();
            DateTimeOffset? startYear = DbObject.GetMinDate();
            //if (startYear != DateTimeOffset.MinValue)
            if (startYear.HasValue)
            {
                for (int i = startYear.Value.Year; i <= DateTimeOffset.Now.Year; i++)
                {
                    dateList.Add(i);
                }
            }
            return dateList;
        }

        /// <summary>
        /// Get minimum date for all screening result
        /// </summary>
        public static DateTimeOffset? GetMinDate()
        {
            return DbObject.GetMinDate();
        }

        #endregion


    }
}
