using Frontdesk.Kiosk.Simulator.Commands;
using NConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Kiosk.Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            HandleSelfSignedSSlCertificate();

            var processor = new CommandLineProcessor(new ConsoleHost());
            processor.RegisterCommand<SendScreeningResultCommand>("send");

            try
            {
                processor.ProcessAsync(args);
            }
            catch (Exception ex)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());

                Console.ForegroundColor = color;

            }
        }

        private static void HandleSelfSignedSSlCertificate()
        {
            // validate cert by calling a function
            ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, cert, chain, errors) => true);


        }
    }
}
