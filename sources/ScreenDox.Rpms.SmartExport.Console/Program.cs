using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NConsole;

namespace ScreenDox.Rpms.SmartExport.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            HandleSelfSignedSSlCertificate();

            var processor = new CommandLineProcessor(new ConsoleHost());
            processor.RegisterCommand<RunCommand >("run");
            try
            {
                processor.Process(args);
            }
            catch (Exception ex)
            {
                var color = System.Console.ForegroundColor;
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(ex.ToString());

                System.Console.ForegroundColor = color;

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
