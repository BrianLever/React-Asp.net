namespace RPMS.Common.GlobalConfiguration
{
    public interface IGlobalSettingsService
    {
        string ExternalEhrSystem { get; }
        bool IsRpmsMode { get;}
        bool IsNextGenMode { get; }
    }


   
}