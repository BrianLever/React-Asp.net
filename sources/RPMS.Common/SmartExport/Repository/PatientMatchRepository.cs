using Common.Logging;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

using RPMS.Common.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace ScreenDox.EHR.Common.SmartExport.Repository
{
    public class PatientMatchRepository : DBDatabase, IPatientMatchRepository
    {
        private ILog _logger = LogManager.GetLogger<PatientMatchRepository>();
        public PatientMatchRepository() : base()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
        }

        public PatientMatchRepository(DbConnection sharedConnection)
        : base(sharedConnection)
        {
        }

        public List<Patient> ValidatePatientInfo(PatientSearch patientSearch)
        {
            List<Patient> result = new List<Patient>();

            const string sql = "[dbo].[uspFindMatchedPatientForExport]";

            ClearParameters();

            AddParameter("@LastName", DbType.String, 128).Value = SqlParameterSafe(patientSearch.LastName);
            AddParameter("@FirstName", DbType.String, 128).Value = SqlParameterSafe(patientSearch.FirstName);
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(patientSearch.MiddleName);
            AddParameter("@Birthday", DbType.Date).Value = patientSearch.DateOfBirth;


            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new Patient
                        {
                            LastName = reader.Get<String>("LastName"),
                            FirstName = reader.Get<String>("FirstName"),
                            MiddleName = reader.Get<String>("MiddleName"),
                            DateOfBirth = reader.Get<DateTime>("Birthday"),
                            ID = reader.Get<int>("ExportedToPatientID"),
                        });
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        /// <summary>
        /// Get name correction map
        /// </summary>
        /// <returns>Dictionary, Source is a key, Destination is a value</returns>
        public Dictionary<string, string> GetNameCorrectionMap()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string sql = "SELECT [Source], [Destination] FROM export.PatientNameMap";

            ClearParameters();

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())

                    {
                        try
                        {
                            result.Add(
                                reader.GetString(0),
                                reader.GetString(1)
                                );
                        }

                        catch (ArgumentException)
                        {
                            _logger.Warn($"Dublicate [Source] value in export.PatientNameMap table. Value: {reader.GetString(0)}");
                        }
                    }
                }

            }
            finally
            {
                Disconnect();
            }

            return result;
        }
    }
}
