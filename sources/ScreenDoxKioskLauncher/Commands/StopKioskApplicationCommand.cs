using ScreenDoxKioskLauncher.Infrastructure;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ScreenDoxKioskLauncher.Commands
{

    public class StopKioskApplicationCommand : BaseCommand<bool>
    {
        /// <summary>
        /// Default consructor
        /// </summary>
        /// <param name="environmentProvider"></param>
        public StopKioskApplicationCommand(IEnvironmentProvider environmentProvider) : base(environmentProvider)
        {
        }

        /// <summary>
        /// Run the command
        /// </summary>
        /// <returns></returns>
        public override bool Run()
        {
            var result = false;
            var processName = GetKioskProcessName();

            try
            {
                StopApplication(processName);
                result = true;

            }
            catch (Exception ex)
            {
                _logger.WarnFormat($"Failed to stop kiosk application.", ex);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Close player application
        /// </summary>
        private void StopApplication(string processName)
        {

            bool allProcessedClosed = false;
            int maxAttempts = 10;
            int attemptIndex = 0;

            try
            {
                //wait white all processes closed
                while (!allProcessedClosed && attemptIndex < maxAttempts)
                {
                    _logger.Info($"Closing kiosk process. Attempt {attemptIndex}.");

                    //find all processes by name
                    var processes = Process.GetProcessesByName(processName);

                    if (!processes.Any())
                    {
                        allProcessedClosed = true;
                        _logger.Info($"All kiosk processed closed. Attempt {attemptIndex}.");
                        continue;
                    }

                    //closing all found processes
                    for (int i = 0; i < processes.Length; i++)
                    {
                        var process = processes[i];
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {

                        }
                    }

                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    attemptIndex++;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to close the kiosk app.", ex);
            }
        }

    }
}
