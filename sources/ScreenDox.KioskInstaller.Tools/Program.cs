using System;
using System.Collections.Generic;
using CommandLine;
using FrontDesk.Common;

namespace ScreenDox.KioskInstaller.Tools
{



    class Program
    {

        class Options
        {
            [Option('c', "count", Required = true, HelpText = "Number of ids generated")]
            public int Count { get; set; }

            [Option('s', "skip", Required = false, HelpText = "Skip some numbers to generate new sequence", Default = 0)]
            public int Skip { get; set; }

            [Option('l', "last-used", Required = true, HelpText = "Last used kiosk id.")]
            public string LastUsed { get; set; }
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
              .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts));
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            string kioskKey = opts.LastUsed;
            var count = opts.Count;
            var skip = opts.Skip;



            try
            {
                var kioskId = TextFormatHelper.UnpackStringInt16(kioskKey);

                var startSequence = kioskId + 1 + skip;

                WriteMsg($"Last used kiosk key: {kioskId}");
                WriteMsg($"First number in sequence: {startSequence}");


                WriteMsg($"Sequence for new registrations ({count}):");

                for (var i = 0; i < count; i++)
                {
                    var key = TextFormatHelper.PackString((Int16)(startSequence + i));
                    WriteMsg($"{key}");
                }
            }
            catch
            {
                WriteError("Key has not been recognized as correct Kiosk Key.");
            }

            WriteMsg("Done.");
        }

        private static void WriteError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void WriteMsg(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
