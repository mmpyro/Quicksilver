using System.IO;

namespace ConfigManager
{
    public class Configuration
    {
        private string _directoryPath;
        public string[] Filters { get; set; }
        public string StartUpFilePath { get; set; }
        public string Program { get; set; }
        public string DirectoryPath
        {
            get
            {
                return _directoryPath ?? Directory.GetCurrentDirectory(); 
            }

            set
            {
                if (value.Contains("/"))
                {
                    _directoryPath = value.Replace("/", @"\");
                }
                else
                {
                    _directoryPath = value;
                }
            }
        }
    }
}