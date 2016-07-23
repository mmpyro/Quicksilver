using Quicksilver.Logger;
using System;
using System.IO;
using System.Linq;

namespace Quicksilver.Filters
{
    public class FileFilter : IFileFilter
    {
        private readonly ILogger _logger;

        public FileFilter(ILogger logger)
        {
            _logger = logger;
            Filters = new string[] { };
        }

        public string[] Filters { get; set; }

        public void Filter(RenamedEventArgs arg, Action restart)
        {
            if (Filters.Any(t => arg.Name.Contains("."+t)))
            {
                _logger.Info($"File {arg.OldName} was renamed to: {arg.Name}");
                restart();
            }
        }

        public void Filter(FileSystemEventArgs arg, Action restart)
        {
            if (Filters.Any(t => arg.Name.Contains("." + t)))
            {
                _logger.Info($"File {arg.Name} was {arg.ChangeType.ToString()} ");
                restart();
            }
        }
    }
}
