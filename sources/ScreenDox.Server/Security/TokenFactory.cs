using System;
using System.Security.Cryptography;


namespace Web.Api.Infrastructure.Auth
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }

    public sealed class TokenFactory : ITokenFactory
    {
        public string GenerateToken(int size = 64)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
