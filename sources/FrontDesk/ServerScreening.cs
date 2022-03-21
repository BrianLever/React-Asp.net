using System.Collections.Generic;

namespace FrontDesk
{
    /// <summary>
    /// Screening questions for Server report
    /// </summary>
    public class ServerScreening : Screening
    {
        #region static members

        private static Data.ScreeningDb DbObject { get { return new FrontDesk.Data.ScreeningDb(); } }

        public static string DefaultScreeningID = "BHS";

        public static Screening GetByID(string screeningID)
        {
            Screening screening = new KioskScreening();
            screening.ScreeningID = screeningID;

            screening.Sections = DbObject.GetScreeningSections(screeningID);

            return screening;
        }

        public static Screening Get()
        {
            Screening screening = new KioskScreening();
           
            screening.ScreeningID = DefaultScreeningID;
            screening.Sections = DbObject.GetScreeningSections(screening.ScreeningID);

            var answers = DbObject.GetAnswerOptions();

            
            foreach(var section in screening.Sections)
            {
                foreach (var question in section.Questions)
                {

                    if (answers.TryGetValue(question.AnswerScaleID, out var questionAnswerOptions))
                    {
                        question.AnswerOptions = questionAnswerOptions.Options;
                    }
                    else
                    {
                        question.AnswerOptions = new List<AnswerScaleOption>();
                    }
                }
            }


            return screening;
        }

        public static List<ScreeningSection> GetSections()
        {
            return DbObject.GetScreeningSections(DefaultScreeningID);
        }
        #endregion
    }
}
