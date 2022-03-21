using ScreenDoxKioskLauncher.Infrastructure;

using System;
using System.Diagnostics;
using System.IO;

namespace ScreenDoxKioskLauncher.Commands
{
    public class StartKioskApplicationCommand : BaseCommand<bool>
    {
        public StartKioskApplicationCommand(IEnvironmentProvider environmentProvider) : base(environmentProvider)
        {
        }

        /// <summary>
        /// Start the command
        /// </summary>
        /// <returns>True when operation was successful</returns>

        public override bool Run()
        {
            var result = false;

            var exePath = GetKioskExePath();
            var processName = GetKioskProcessName();

            try
            {
                StartApplication(processName, exePath);

            }
            catch (Exception ex)
            {
                _logger.WarnFormat($"Failed to start kiosk application. Path: {exePath}", ex);
            }

            return result;
        }

        private bool StartApplication(string processName, string exeFulPath, bool isHighPriority = false)
        {
            Process[] processes = Process.GetProcessesByName(GetKioskProcessName());
            if (processes.Length == 0)
            {

                string fileToStart = exeFulPath;

                _logger.InfoFormat("Starting application: '{0}'", fileToStart);

                ProcessStartInfo processInfo = new ProcessStartInfo(fileToStart);

                var process = Process.Start(processInfo);
                if (isHighPriority)
                {
                    process.PriorityClass = ProcessPriorityClass.AboveNormal;
                    process.PriorityBoostEnabled = true;
                }
                else
                {
                    process.PriorityClass = ProcessPriorityClass.Normal; //
                }

                return true;
            }
            else
            {
                //close another instance and halt application
                for (int i = 0; i < processes.Length; i++)
                {
                    if (!processes[i].Responding || i != 0)
                    {
                        processes[i].CloseMainWindow();
                        processes[i].WaitForExit(3000);
                        processes[i].Close();

                        _logger.WarnFormat("Killing app process during application start. Process ID: {0}", i);
                    }
                }
                return false;
            }
        }
    }
}
