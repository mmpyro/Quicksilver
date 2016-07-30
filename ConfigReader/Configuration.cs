using System.IO;
using ConfigManager.Helpers;

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
                _program = value.ConvertPath();
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
                _directoryPath = value.ConvertPath();
            }
        }
    }
}