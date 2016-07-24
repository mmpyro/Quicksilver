using System.IO;

namespace ConfigManager
{
    public class Configuration
    {
        private string _directoryPath;
        private string _program;
        public string[] Filters { get; set; }
        public string Arguments { get; set; }
        public string Program
        {
            get
            {
                return _program;
            }
            set
            {
                if (value.Contains("/"))
                {
                    _program = value.Replace("/", @"\");
                }
                else
                {
                    _program = value;
                }
            }
        }

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