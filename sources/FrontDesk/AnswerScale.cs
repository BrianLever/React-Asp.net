using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk
{
    /// <summary>
    /// Set of allowed options to answer the questio
    /// </summary>
    public class AnswerScale
    {
        public const int DefaultYesNoScaleID = 1;
        #region properties

        public int AnswerScaleID { get; set; }
        public string Description { get; set; }

        #endregion
        /// <summary>
        /// Amswer options
        /// </summary>
        public List<AnswerScaleOption> Options = new List<AnswerScaleOption>();

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AnswerScale() { }
        /// <summary>
        /// Constructor with scale id
        /// </summary>
        /// <param name="answerScaleID"></param>
        public AnswerScale(int answerScaleID) { AnswerScaleID = answerScaleID; }

        #endregion


        #region Static Methods
        /// <summary>
        /// Get new database object
        /// </summary>
        private static Data.IScreeningAnswersDb DbObject
        {
            get
            {
                if (!_useLocalDatabaseConnection)
                {
                    return new Data.ScreeningDb();
                }
                else
                {
                    return new Data.ScreeningKioskDb();
                }
            }
        }

        public static Dictionary<int, AnswerScale> GetAnswerOptions()
        {
            return DbObject.GetAnswerOptions();
        }

        public static List<AnswerScaleOption> GetAnswerOptions(int answerScaleID)
        {
            return DbObject.GetAnswerOptions(answerScaleID);
        }

        /// <summary>
        /// Get Yes/No options
        /// </summary>
        /// <returns></returns>
        public static List<AnswerScaleOption> GetYesNoAnswerOptions()
        {
            return DbObject.GetAnswerOptions(DefaultYesNoScaleID);
        }

        private static bool _useLocalDatabaseConnection = false;
        /// <summary>
        /// If true user SQL Compact database provider. False by default
        /// </summary>
        public static bool UseLocalDatabaseConnection
        {
            get { return _useLocalDatabaseConnection; }
            set { _useLocalDatabaseConnection = value; }
        }


        #endregion
    }
}
