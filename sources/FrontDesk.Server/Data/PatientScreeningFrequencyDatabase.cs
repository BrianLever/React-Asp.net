using System;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;

namespace FrontDesk.Server.Data
{
    /// <summary>
    /// Class cimplements methods for Screening Frequency feature when patient is allowed to not be screenned every visit but once a month, bi-monthly, quaterly, annually.
    /// Class methods retutn number of visits in the last gpra year period starting from certain date
    /// </summary>
    public class PatientScreeningFrequencyDatabase : DBDatabase, IPatientScreeningFrequencyRepository
    {
        public PatientScreeningFrequencyDatabase() : base(0) { }

        internal PatientScreeningFrequencyDatabase(DbConnection sharedConnection) : base(sharedConnection) { }

        /// <summary>
        /// Get number of actual contact information screenings in current gpra interval period
        /// </summary>
        /// <param name="patient">Patient identity</param>
        /// <param name="currentGpraPeriodStartDate">Start date of the current screening interval</param>
        /// <returns>Number of actual screenings. Screenings with omitted contact address questions are ignored.</returns>
        public int GetPatientContactInfoScreeningCount(ScreeningPatientIdentity patient, DateTimeOffset currentGpraPeriodStartDate)
        {
            int recordCount = 0;

            string sqlText = @"
SELECT count(ScreeningResultID)
FROM dbo.ScreeningResult
WHERE CreatedDate >= @StartDate 
    AND FirstName = @FirstName 
    AND LastName = @LastName 
    AND ISNULL(MiddleName,'') = ISNULL(@MiddleName, '')
    AND Birthday = @Birthday
    AND StreetAddress IS NOT NULL
";

            CommandObject.Parameters.Clear();
            AddParameter("@FirstName", DbType.String, 128).Value = patient.FirstName;
            AddParameter("@LastName", DbType.String, 128).Value = patient.LastName;
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patient.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patient.Birthday;
            AddParameter("@StartDate", DbType.DateTimeOffset).Value = currentGpraPeriodStartDate;

            try
            {

                Connect();

                recordCount = (int)RunScalarQuery(sqlText);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return recordCount;
        }

        public int GetPatientDemographicsScreeningCount(ScreeningPatientIdentity patient, DateTimeOffset currentGpraPeriodStartDate)
        {
            int recordCount = 0;

            string sqlText = @"
SELECT count(DISTINCT tl.ScreeningResultID)
FROM dbo.ScreeningTimeLog tl 
    INNER JOIN dbo.ScreeningResult r ON tl.ScreeningResultID = r.ScreeningResultID
WHERE tl.ScreeningSectionID = @ScreeningSectionID 
	AND tl.StartDate >= @StartDate 
    AND r.PatientName = dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName)
    AND r.Birthday = @Birthday
";

            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningSectionID", DbType.String, 128).Value = ScreeningSectionDescriptor.Demographics;
            AddParameter("@FirstName", DbType.String, 128).Value = patient.FirstName;
            AddParameter("@LastName", DbType.String, 128).Value = patient.LastName;
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patient.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patient.Birthday;
            AddParameter("@StartDate", DbType.DateTimeOffset).Value = currentGpraPeriodStartDate;

            try
            {

                Connect();

                recordCount = (int)RunScalarQuery(sqlText);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return recordCount;
        }

        /// <summary>
        /// Get the number of screenings passed by patient for certains measure tool in the current period (starting from certain date)
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="screeningSectionID"></param>
        /// <param name="currentGpraPeriodStartDate"></param>
        /// <returns></returns>
        public int GetPatientSectionScreeningCount(ScreeningPatientIdentity patient, string screeningSectionID, DateTimeOffset currentGpraPeriodStartDate)
        {
            int recordCount = 0;

            string sqlText = @"
SELECT count(sr.ScreeningSectionID)
FROM dbo.ScreeningResult r INNER JOIN dbo.ScreeningSectionResult sr ON r.ScreeningResultID = sr.ScreeningResultID
WHERE CreatedDate >= @StartDate 
    AND FirstName = @FirstName 
    AND LastName = @LastName 
    AND ISNULL(MiddleName,'') = ISNULL(@MiddleName, '')
    AND Birthday = @Birthday
    AND sr.ScreeningSectionID = @ScreeningSectionID
";

            CommandObject.Parameters.Clear();
            AddParameter("@ScreeningSectionID", DbType.AnsiString, 5).Value = screeningSectionID;
            AddParameter("@FirstName", DbType.String, 128).Value = patient.FirstName;
            AddParameter("@LastName", DbType.String, 128).Value = patient.LastName;
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patient.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patient.Birthday;
            AddParameter("@StartDate", DbType.DateTimeOffset).Value = currentGpraPeriodStartDate;

            try
            {

                Connect();

                recordCount = (int)RunScalarQuery(sqlText);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                base.Disconnect();
            }
            return recordCount;
        }
    }
}
