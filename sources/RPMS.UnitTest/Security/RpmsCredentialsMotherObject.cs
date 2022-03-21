using System;
using RPMS.Common.Security;

namespace RPMS.UnitTest.Security
{
    public static class RpmsCredentialsMotherObject
    {
        public static RpmsCredentials CreateEncyptedCredentials()
        {
            return new RpmsCredentials
            {
                Id = new Guid("083A4382-6722-4215-9711-34479175CBC4"),
                AccessCode = "AccessCodeEncrypted",
                VerifyCode = "VerifyCodeEncrypted",
                ExpireAt = new DateTime(2014, 10, 31)
            };
        }


        public static RpmsCredentials CreateOpenTextCredentials()
        {
            return new RpmsCredentials
            {
                Id = new Guid("083A4382-6722-4215-9711-34479175CBC4"),
                AccessCode = "AccessCodeOpenText",
                VerifyCode = "VerifyCodeOpenText",
                ExpireAt = new DateTime(2014, 10, 31)
            };
        }
    }
}
