namespace FrontDesk.Kiosk.Workflow
{
	public interface IScreeningFrequencySpecification
    {
        bool IsSkipRequiredForSection(string sectionId);
        void LoadPatientScreeningsStatistics();
    }
}
