using System;

namespace ScreenDox.ServerInstallation.KioskRegistrationUtil
{
    internal class SecretKeyGenerator
    {
        const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string NUMBERS = "123456789";
        const string SPECIALS = @"@£-#";

        private readonly bool useLowercase;
        private readonly bool useUppercase;
        private readonly bool useNumbers;
        private readonly bool useSpecial;

        public SecretKeyGenerator(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial)
        {
            this.useLowercase = useLowercase;
            this.useUppercase = useUppercase;
            this.useNumbers = useNumbers;
            this.useSpecial = useSpecial;
        }

        public string GenerateSecret(int passwordSize)
        {
            char[] _password = new char[passwordSize];
            string charSet = ""; // Initialise to blank
            Random _random = new Random();

            // Build up the character set to choose from
            if (useUppercase) charSet += UPPER_CAES;
            if (useLowercase) charSet += LOWER_CASE;
            if (useSpecial) charSet += SPECIALS;
            if (useNumbers) charSet += NUMBERS;

            for (var index = 0; index < passwordSize; index++)
            {
                _password[index] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
    }

}
