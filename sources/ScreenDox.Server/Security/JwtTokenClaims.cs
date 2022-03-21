using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Security
{
    /// <summary>
    /// Known token names
    /// </summary>
    public class JwtTokenClaims
    {
        public const string Subject = "sub";
        public const string SessionId = "sid";
   
        public const string NotBefore = "nbf";

        public const string Nonce = "nonce";
 
        public const string JwtId = "jti";

        public const string Issuer = "iss";
        public const string IssuedAt = "iat";

        public const string FullName = "name";
        public const string FirstName = "given_name";
        public const string LastName = "family_name";
        public const string ExpirationTime = "exp";
      
        public const string Email = "email";
      
        
        public const string AuthTime = "auth_time";

        public const string Audience = "aud";

        public const string UniqueName = "unique_name";
        public const string UserId = "user_id";

        public const string Website = "website";

        public const string Roles = "roles";

        public const string BranchLocationId = "branchlocationid";

    }
}