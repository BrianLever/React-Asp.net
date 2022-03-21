using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using FrontDesk.Common.Data;
using System.Reflection.Emit;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Server.Screening
{
    public class ScreeningScoreLevel : IScreeningScoreLevelRepository
    {
        #region Properties
        /// <summary>
        /// Screening section ID
        /// </summary>
        public string ScreeningSectionID { get; set; }

        /// <summary>
        /// Score level value
        /// </summary>
        public int ScoreLevel { get; set; }
        /// <summary>
        /// Score Level string
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Score Level label
        /// </summary>
        public string Label { get; set; }

        #endregion

        #region constructors

        public ScreeningScoreLevel() { }

        public ScreeningScoreLevel(IDataReader reader)
        {
            ScreeningSectionID = Convert.ToString(reader["ScreeningSectionID"]).TrimEnd();
            ScoreLevel = Convert.ToInt32(reader["ScoreLevel"]);
            Name = Convert.ToString(reader["Name"]);
            Label = reader.Get<string>("Label", string.Empty);
        }

        #endregion
        #region Static members
        /// <summary>
        /// Get new database object
        /// </summary>
        private static Data.ScreeningScoreDb DbObject { get { return new FrontDesk.Server.Data.ScreeningScoreDb(); } }
        /// <summary>
        /// get list of score levels for particular sections
        /// </summary>
        public IList<ScreeningScoreLevel> GetScoreLevelsBySectionID(string sectionID)
        {
            return DbObject.GetScoreLevelsBySectionID(sectionID);
        }
        /// <summary>
        /// Get all score levels as DataView for all sections in the screening
        /// </summary>
        /// <param name="screeningID"></param>
        /// <returns></returns>
        public static DataView GetAllScoreLevelsByScreeningID(string screeningID)
        {
            DataView dv = null;
            var ds = DbObject.GetAllScoreLevelsByScreeningID(screeningID);
            if(DBDatabase.IsHasOneTable(ds))
            {
                dv = new DataView(ds.Tables[0]);
            }
            return dv;
        }

        #endregion

    }
}
