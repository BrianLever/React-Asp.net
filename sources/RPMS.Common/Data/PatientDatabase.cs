using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using InterSystems.Data.CacheClient;

namespace RPMS.Common.Data
{
    public class PatientDatabase: CacheDatabase
    {

        public DataSet GetPatientMatches(string name)
        {
            string sql = @"
SELECT p.RowId,
vp.NAME,
vp.SEX,
vp.DOB,
st.ABBREVIATION as StateID,
vp.MAILING_ADDRESSCITY as City,
vp.MAILING_ADDRESSZIP as ZipCode,
vp.MAILING_ADDRESSSTREET + ' ' + vp.STREET_ADDRESS_LINE_2 + ' ' + vp.STREET_ADDRESS_LINE_3 AS StreetAddress,
vp.HOME_PHONE,
vp.OFFICE_PHONE,
vp.PHONE_3,
vp.PHONE_4
FROM IHS.PATIENT p 
INNER JOIN IHS.VA_PATIENT vp ON p.NAME = vp.RowID
INNER JOIN IHS.STATE st ON st.RowId= vp.MAILING_ADDRESSSTATE
WHERE vp.Name = 'ANDERSON,TERESA'
";

            #region Paging parameters

            CommandObject.Parameters.Clear();
            //AddParameter("@Name", DbType.String, 386).Value = name;

            #endregion

            try
            {
                Connect();
                return GetDataSet(sql);
            }
            catch
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
