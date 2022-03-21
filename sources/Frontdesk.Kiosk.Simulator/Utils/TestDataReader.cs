using FrontDesk;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Frontdesk.Kiosk.Simulator.Utils
{
    public class TestDataReader
    {
        public List<ScreeningResultTestData> GetTestSequence(string fileOrDirectoryPath)
        {
            List<ScreeningResultTestData> results = new List<ScreeningResultTestData>();

            string[] filenames = FindFilesFromPath(fileOrDirectoryPath);
            foreach (var filename in filenames)
            {
                results.Add(new ScreeningResultTestData
                {
                    Data = JsonConvert.DeserializeObject<ScreeningResult>(File.ReadAllText(filename)),
                    TestCaseName = Path.GetFileNameWithoutExtension(filename)
                });
            }
            return results;
        }

        private string[] FindFilesFromPath(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                return Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly).OrderBy(f => f).ToArray();
            }
            else
            {
                return new string[] { System.IO.Path.GetFullPath(path) };
            }
        }
    }
}
