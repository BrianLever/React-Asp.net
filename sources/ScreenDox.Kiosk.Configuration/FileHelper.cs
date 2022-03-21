using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScreenDox.Kiosk.Configuration
{
    public static class FileHelper
    {
        public static void ReplaceParametersInFIle(string sourceFilePath, string targetFilePath, Dictionary<string, string> parameters)
        {
            if (!File.Exists(sourceFilePath))
            {
                throw new Exception($"Failed to find application configuration file to replace. Path: {sourceFilePath}");
            }

            // replacing parameters in file and writing to target app config file
            var contentLines = File.ReadAllLines(sourceFilePath);
            using (var fs = File.OpenWrite(targetFilePath))
            {
                using (var outputWriter = new StreamWriter(fs, Encoding.UTF8))
                {
                    foreach (var line in contentLines)
                    {
                        var tempLineValue = line;
                        foreach (var param in parameters)
                        {
                            tempLineValue = tempLineValue.Replace(param.Key, param.Value);
                        }
                        outputWriter.WriteLine(tempLineValue);
                    }
                }
            }
        }
    }
}
