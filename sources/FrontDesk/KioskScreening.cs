using FrontDesk.Data;

namespace FrontDesk
{
    /// <summary>
    /// Screening questions for Kiosk
    /// </summary>
    public class KioskScreening : Screening
    {

        #region static members

        private static ScreeningKioskDb DbObject { get { return new ScreeningKioskDb(); } }


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
            string id = DbObject.GetAvailableScreening();
            if (!string.IsNullOrEmpty(id))
            {

                screening.ScreeningID = id;

                screening.Sections = DbObject.GetScreeningSections(id);
            }
            return screening;
        }

        
        #endregion
    }
}
