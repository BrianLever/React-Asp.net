using RPMS.Common.Security;

namespace RPMS.Data.CareBridge
{
    public interface IApiCredentialsService
    {
        BasicAuthCredentials GetCredentials();
        BasicAuthCredentials GetCredentialsCached();
    }
}