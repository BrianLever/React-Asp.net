namespace FrontDesk.Common.Screening
{
    public interface IScreeningAgesSettingsProvider
    {
        int[] AgeGroups { get; }
        string[] AgeGroupsLabels { get; }

        /// <summary>
        /// Reload data
        /// </summary>
        void Refresh();
    }
}