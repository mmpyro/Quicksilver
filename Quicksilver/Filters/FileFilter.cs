using Quicksilver.Logger;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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

        public void Filter(RenamedEventArgs arg, Subject<string> subject)
        {
            if (Filters.Any(t => arg.Name.Contains("."+t)))
            {
                _logger.Info($"File {arg.OldName} was renamed to: {arg.Name}");
                subject.OnNext(arg.Name);
            }
        }

        public void Filter(FileSystemEventArgs arg, Subject<string> subject)
        {
            if (Filters.Any(t => arg.Name.Contains("." + t)))
            {
                _logger.Info($"File {arg.Name} was {arg.ChangeType.ToString()} ");
                subject.OnNext(arg.Name);
            }
        }

    }
}
