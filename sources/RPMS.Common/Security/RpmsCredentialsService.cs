using System;
using System.Collections.Generic;
using Common.Logging;
using CuttingEdge.Conditions;

namespace RPMS.Common.Security
{
    public interface IRpmsCredentialsService
    {
        RpmsCredentials GetCredentials();
        RpmsCredentials GetCredentialsCached();
        List<RpmsCredentials> GetAllCredentials();
        void AddCredentials(RpmsCredentials openTextCredentials);
        void DeleteCredentials(Guid id);
    }

    public class RpmsCredentialsService : IRpmsCredentialsService
    {
        private readonly IRpmsCredentialsRepository _rpmsCredentialsRepository;
        private readonly ICryptographyService _cryptographyService;
        private readonly ILog _logger = LogManager.GetLogger<IRpmsCredentialsService>();

        private Lazy<RpmsCredentials> _credentialsCached; 

        public RpmsCredentialsService(IRpmsCredentialsRepository rpmsCredentialsRepository, ICryptographyService cryptographyService)
        {
            Condition.Requires(rpmsCredentialsRepository, "rpmsCredentialsRepository").IsNotNull();
            Condition.Requires(cryptographyService, "cryptographyService").IsNotNull();

            _rpmsCredentialsRepository = rpmsCredentialsRepository;
            _cryptographyService = cryptographyService;

            _credentialsCached = new Lazy<RpmsCredentials>(GetCredentials);
        }

        public List<RpmsCredentials> GetAllCredentials()
        {
            var credentials = _rpmsCredentialsRepository.GetAll();
            if (credentials == null)
            {
                return null;
            }
            foreach (var credential in credentials)
            {
                try
                {
                    credential.AccessCode = _cryptographyService.Decrypt(credential.AccessCode);
                    credential.VerifyCode = _cryptographyService.Decrypt(credential.VerifyCode);
                }
                catch(Exception ex)
                {
                    _logger.ErrorFormat("Failed to decrypt EHR credentials", ex);

                    credential.AccessCode = "Unable to decrypt";
                    credential.VerifyCode = "";
                }
            }

            return credentials;
        }

        public RpmsCredentials GetCredentialsCached()
        {
            return _credentialsCached.Value;
        }

        public RpmsCredentials GetCredentials()
        {
            var credentials = _rpmsCredentialsRepository.Get();
            if (credentials == null)
            {
                return null;
            }

            credentials.AccessCode = _cryptographyService.Decrypt(credentials.AccessCode);
            credentials.VerifyCode = _cryptographyService.Decrypt(credentials.VerifyCode);

            return credentials;
        }

        public void AddCredentials(RpmsCredentials openTextCredentials)
        {
            if (openTextCredentials == null)
            {
                return;
            }

            var encryptedCredentials = new RpmsCredentials
            {
                AccessCode = _cryptographyService.Encrypt(openTextCredentials.AccessCode),
                VerifyCode = _cryptographyService.Encrypt(openTextCredentials.VerifyCode),
                ExpireAt = openTextCredentials.ExpireAt,
                Id = Guid.NewGuid()
            };

            openTextCredentials.Id = encryptedCredentials.Id;

            _rpmsCredentialsRepository.Add(encryptedCredentials);

        }

        public void DeleteCredentials(Guid id)
        {
            _rpmsCredentialsRepository.Delete(id);
        }
    }
}
