using Frontdesk.Server.SmartExport.Services;

using FrontDesk;
using FrontDesk.Server.Screening;

using NConsole;

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ScreenDox.Rpms.SmartExport.Console
{
    [Description("Start export.")]
    public class RunCommand : IConsoleCommand
    {
        [Description("Enable or disable test mode")]
        [Argument(Name = "SimulationMode")]
        public bool SimulationMode { get; set; } = true;

        [Description("Get results from")]
        [Argument(Name = "StartDate")]
        public DateTime StartDate { get; set; }


        [Description("The max records to process.")]
        [Argument(Name = "BatchSize")]
        public int BatchSize { get; set; }

        public async Task<object> RunAsync(CommandLineProcessor processor, IConsoleHost host)
        {

            var service = new SmartExportService();

            int sqlBatchSize = 10000; // because SQL returns fresh records first
            var results = SimulationMode ? 
                service.TestGetScreeningResultIdForExport(StartDate, sqlBatchSize) :
                service.GetScreeningResultIdForExport(sqlBatchSize).Where(x => x.CreatedDate >= StartDate).ToList();

            results = results.OrderBy(x => x.CreatedDate).Take(BatchSize).ToList();

            var defaultColor = System.Console.ForegroundColor;

            decimal total = results.Count;
            decimal succeed = 0;
            decimal failed = 0;
            int index = 0;
            foreach (var result in results)
            {
                index++;
                System.Console.WriteLine(string.Format("{2}. Processing screening ID: {0}, Patient: {1}", result.ID, result.PatientName, index));
                var record = ScreeningResultHelper.GetScreeningResult(result.ID);
                bool status = service.ExecuteExport(record, SimulationMode);

                if (!status)
                {
                    failed++;
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    succeed++;
                    System.Console.ForegroundColor = ConsoleColor.Green;
                }

                System.Console.WriteLine(
                    string.Format("[{0}] Screening ID: {1}, Patient: {2}", status ? "SUCCEED" : "FAILED", result.ID, result.PatientName)
                    );

                System.Console.ForegroundColor = defaultColor;
            }


            System.Console.WriteLine("[Summary] Success Rate: {0:N2}| Succeed: {1}| Failed: {2}| Total: {3}".FormatWith(succeed / total * 100, succeed, failed, total));

            return new Task<object>(() => 1);
        }
    }

}
