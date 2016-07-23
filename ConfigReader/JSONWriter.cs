using Newtonsoft.Json;
using System.IO;

namespace ConfigManager
{
    public class JSONWriter : IConfigurationWriter
    {
        public void Save(string path, Configuration configuration)
        {
            string json = JsonConvert.SerializeObject(configuration);
            File.WriteAllText(path, json);
        }
    }
}
