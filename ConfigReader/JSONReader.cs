using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ConfigManager
{
    public class JSONReader : IConfigurationReader
    {
        public Configuration LoadConfiguration()
        {
            string configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "quicksilver.json");
            if (File.Exists(configurationFilePath))
            {
                return JsonConvert.DeserializeObject<Configuration>(ReadJson(configurationFilePath));
            }
            else
            {
                throw new ArgumentException($"File {configurationFilePath} does not exist");
            }
        }

        public Configuration LoadConfiguration(string directoryFilePath)
        {
            string configurationFilePath = Path.Combine(directoryFilePath, "quicksilver.json");
            if (File.Exists(configurationFilePath))
            {
                return JsonConvert.DeserializeObject<Configuration>(ReadJson(configurationFilePath));
            }
            else
            {
                throw new ArgumentException($"File {configurationFilePath} does not exist");
            }
        }

        private string ReadJson(string path)
        {
            string json = File.ReadAllText(path);
            json = JToken.Parse(json).ToString().Replace("\\","\\\\");
            return json;
        }
    }
}
