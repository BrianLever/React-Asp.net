using Common.Logging;
using Frontdesk.Kiosk.Simulator.Utils;
using NConsole;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Frontdesk.Kiosk.Simulator.Commands
{
    [Description("Read screening data from json file in folder and send to the server.")]
    public class SendScreeningResultCommand : IConsoleCommand
    {
        private readonly ILog _logger = LogManager.GetLogger<SendScreeningResultCommand>();

        [Description("The array if kiosk ids to be used when sending results.")]
        [Argument(Name = "Kiosks", IsRequired = false)]
        public string Kiosks { get; set; } = "1000";


        [Description("Path to json file or folder with file.")]
        [Argument(Name = "Path", IsRequired = false)]
        public string Path { get; set; }



        public Task<object> RunAsync(CommandLineProcessor processor, IConsoleHost host)
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                Path = ".\\Scenarios";
            }

            var testData = new TestDataReader().GetTestSequence(Path);

            if (testData.Count == 0)
            {
                host.WriteError("No test cases found.");
            }

            var kioskList = Kiosks.Split(new[] { ' ', ';', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Int16.Parse(x)).ToArray();

            _logger.InfoFormat("Kiosks list: {0}", String.Join(", ", kioskList));
            //modify data

            try
            {
                for (var index = 0; index < testData.Count; index++)
                {
                    testData[index].Data.KioskID = (kioskList.Length > 0 ? kioskList[index % kioskList.Length] : kioskList[0]);

                    _logger.InfoFormat("Pattern {0}, Kiosk: {1}", index, testData[index].Data.KioskID);
                }

                (new KioskEndpointClient()).Send(testData, host);
                _logger.InfoFormat("Test completed.");

                host.WriteMessage("Test completed.\r\n");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed send the date", ex);

                host.WriteMessage("Test failed.");
            }
            return new Task<object>(() => 1);
        }



    }
}
