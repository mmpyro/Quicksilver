using System;
using Newtonsoft.Json;
using System.IO;

namespace ConfigReader
{
    public class JSONReader : IConfigurationReader
    {
        public Configuration LoadConfiguration()
        {
            string configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "quicksilver.js");
            if (File.Exists(configurationFilePath))
            {
                return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configurationFilePath));
            }
            else
            {
                throw new ArgumentException($"File {configurationFilePath} does not exist");
            }
        }

        public Configuration LoadConfiguration(string directoryFilePath)
        {
            string configurationFilePath = Path.Combine(directoryFilePath, "quicksilver.js");
            if (File.Exists(configurationFilePath))
            {
                return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configurationFilePath));
            }
            else
            {
                throw new ArgumentException($"File {configurationFilePath} does not exist");
            }
        }
    }
}
