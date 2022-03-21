using CommandLine;
using FrontDesk.Common;
using ScreenDox.ServerInstallation.KioskRegistrationUtil.Options;
using System;

namespace ScreenDox.ServerInstallation.KioskRegistrationUtil
{
    class Program
    {
       

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<RegisterOptions>(args)
                  .MapResult(
                    (RegisterOptions opts) => RunRegisterAndReturnExitCode(opts),
                    errs => 1
                    );
        }

        private static int RunRegisterAndReturnExitCode(RegisterOptions opts)
        {
            var start = (short)(opts.Start + 1000);
            var count = (short)opts.Count;
            var secretGenerator = new SecretKeyGenerator(true, true, true, true);

            for (short index = 0; index < count; index++ )
            {
                short id = (short)(start + index);
                var key = TextFormatHelper.PackString(id);
                var secret = secretGenerator.GenerateSecret(64);

                Console.WriteLine($"ID:{id}\tKey:{key}\tSecret:{secret}");
            }

            return 0;
        }
    }
}
