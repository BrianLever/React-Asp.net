using Common.Logging;
using FrontDesk;
using NConsole;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontdesk.Kiosk.Simulator.Utils
{
    public class KioskEndpointClient
    {
        private readonly ILog _logger = LogManager.GetLogger<KioskEndpointClient>();

        public void Send(ICollection<ScreeningResultTestData> testData, IConsoleHost host)
        {
            var client = new KioskEndpointWebService.KioskEndpointClient();

            _logger.DebugFormat("KioskEndpointClient preparing sending test data. Pattern count: {0}", testData.Count);

            try
            {
                foreach(var testCase in testData)
                {
                    _logger.DebugFormat("Sending result [{0}]", testCase.TestCaseName);

                    try
                    {
                        client.SaveScreeningResult(testCase.Data);

                    }
                    catch (Exception ex)
                    {
                        _logger.ErrorFormat("Failed sending result [{0}].", ex, testCase.TestCaseName);

                        throw;
                    }
                    _logger.InfoFormat("Result [{0}] sent succeed.", testCase.TestCaseName);

                    host.WriteMessage($"Test case {testCase.TestCaseName} executed.\r\n");
                }
            }
            finally
            {
                client.Close();
            }


        }
    }
}
