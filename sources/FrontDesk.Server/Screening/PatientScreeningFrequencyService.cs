using Common.Logging;

using FrontDesk.Server;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.ScreenngProfile;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FrontDesk.Configuration
{
    public interface IPatientScreeningFrequencyService
    {
        Dictionary<string, int> GetGPRAScreeningCount(ScreeningPatientIdentity patient, DateTimeOffset currentDate, int screeningProfileId);
        Dictionary<string, int> GetGPRAScreeningCount(ScreeningPatientIdentity patient, IEnumerable<ScreeningFrequencyItem> screeningFrequencyConfiguration, DateTimeOffset currentDate);
        Dictionary<string, int> GetGPRAScreeningCount(ScreeningPatientIdentity patient, int screeningProfileId);
    }

    /// <summary>
    /// Get the number of screenings per GPRA period
    /// </summary>
    public class PatientScreeningFrequencyService : IPatientScreeningFrequencyService
    {
        private readonly ILog _logger = LogManager.GetLogger<PatientScreeningFrequencyService>();

        private Func<IPatientScreeningFrequencyRepository> _resultsRepositoryFactoryMethod;
        private IScreeningProfileFrequencyRepository _settingsRepository;


        public PatientScreeningFrequencyService(
            Func<IPatientScreeningFrequencyRepository> resultsRepositoryFactoryMethod,
            IScreeningProfileFrequencyRepository settingsRepository
            )
        {
            if (resultsRepositoryFactoryMethod == null)
                throw new ArgumentNullException("resultsRepositoryFactoryMethod");

            if (settingsRepository == null)
                throw new ArgumentNullException("settingsRepository");


            _resultsRepositoryFactoryMethod = resultsRepositoryFactoryMethod;
            _settingsRepository = settingsRepository;
        }

        public Dictionary<string, int> GetGPRAScreeningCount(ScreeningPatientIdentity patient, int screeningProfileId)
        {
            return GetGPRAScreeningCount(patient, DateTimeOffset.Now, screeningProfileId);
        }

        public Dictionary<string, int> GetGPRAScreeningCount(ScreeningPatientIdentity patient, DateTimeOffset currentDate, int screeningProfileId)
        {

            IEnumerable<ScreeningFrequencyItem> frequencySettings = _settingsRepository.GetAll(screeningProfileId);

            return GetGPRAScreeningCount(patient, frequencySettings, currentDate);
        }

        public static DateTimeOffset GetStartTime(int frequencyInterval, DateTimeOffset currentDate)
        {
            if (frequencyInterval <= 0)
            {
                throw new ArgumentException("Should be greater than zero", nameof(frequencyInterval));
            }

            DateTimeOffset startTime;
            DateTime today = currentDate.Date;


            //get months
            int months = frequencyInterval / 100;

            if (months > 0)
            {

                if (months > 12)
                {
                    //if Once a time
                    //we search over all database

                    startTime = currentDate.AddMonths(-1 * months);
                }

                else
                {


                    //we count within GPRA period
                    startTime = new DateTimeOffset(
                        GPRAReportingTime.GetCurrent(today).GetGPRAFrequencyEffectiveDateInMonths(today, months), currentDate.Offset);
                }
            }
            else // days
            {
                int days = frequencyInterval % 100;

                startTime = new DateTimeOffset(
                    GPRAReportingTime.GetCurrent(today).GetGPRAFrequencyEffectiveDateInDays(today, days), currentDate.Offset);
            }

            return startTime;

        }

        public Dictionary<string, int> GetGPRAScreeningCount(ScreeningPatientIdentity patient, IEnumerable<ScreeningFrequencyItem> screeningFrequencyConfiguration, DateTimeOffset currentDate)
        {
            Dictionary<string, int> patientScreeningStatistics = new Dictionary<string, int>();
            DateTime today = currentDate.DateTime;
            Task<KeyValuePair<string, int>> task;

            List<Task<KeyValuePair<string, int>>> tasks = new List<Task<KeyValuePair<string, int>>>();
            foreach (var setting in screeningFrequencyConfiguration)
            {
                //if not Every Visit
                if (setting.Frequency > 0)
                {
                    string sectionId = setting.ID;
                    DateTimeOffset startTime = GetStartTime(setting.Frequency, currentDate);

                    if (sectionId == ScreeningFrequencyDescriptor.ContactFrequencyID)
                    {

                        task = new Task<KeyValuePair<string, int>>(() =>
                        {
                            return new KeyValuePair<string, int>(sectionId, _resultsRepositoryFactoryMethod().GetPatientContactInfoScreeningCount(patient, startTime));
                        });
                    }
                    else if (sectionId == ScreeningSectionDescriptor.Demographics)
                    {

                        task = new Task<KeyValuePair<string, int>>(() =>
                        {
                            return new KeyValuePair<string, int>(sectionId, _resultsRepositoryFactoryMethod().GetPatientDemographicsScreeningCount(patient, startTime));
                        });
                    }
                    else
                    {

                        task = new Task<KeyValuePair<string, int>>(() =>
                        {
                            return new KeyValuePair<string, int>(sectionId, _resultsRepositoryFactoryMethod().GetPatientSectionScreeningCount(patient, sectionId, startTime));
                        });
                    }
                    tasks.Add(task);
                    task.Start();
                }
                else
                {
                    patientScreeningStatistics.Add(setting.ID, 0);
                }
            }

            try
            {
                Task.WaitAll(tasks.ToArray());

                foreach (var t in tasks)
                {
                    patientScreeningStatistics.Add(t.Result.Key, t.Result.Value);
                }
            }
            catch (AggregateException ex)
            {

                foreach (var exc in ex.InnerExceptions)
                {
                    _logger.Warn("Failed to get patient screening statistics.", exc);
                }

                throw ex.Flatten();
            }

            // copy values from PHQ and GAD short to all questions sections
            var pairs = ScreeningSectionDescriptor.AlternativeOptionalMandatorySections;

            foreach (var pair in pairs)
            {
                int tmpStatValue;
                if (patientScreeningStatistics.TryGetValue(pair.Primary, out tmpStatValue))
                {
                    patientScreeningStatistics.Add(pair.AllQuestions, tmpStatValue);
                }
            }

            return patientScreeningStatistics;
        }

    }
}
