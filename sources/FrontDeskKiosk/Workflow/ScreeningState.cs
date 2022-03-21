namespace FrontDesk.Kiosk.Workflow
{
	public class ScreeningState
    {
        /// <summary>
        /// current screening section
        /// </summary>
        public ScreeningSection Section {get;set;}

        /// <summary>
        /// current screening section
        /// </summary>
        public ScreeningSectionQuestion SectionQuestion { get; set; }


        public void Reset()
        {
            Section = null;
            SectionQuestion = null;
        }
    }
}
