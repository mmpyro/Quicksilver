using ConfigReader;
using System.IO;
using FileWatcher;
using Quicksilver.Logger;
using Quicksilver.ProcessWorker;

namespace Quicksilver
{
    public class QuickSilver
    {
        private readonly IProcessWorker _processWorker;
        private readonly ILogger _logger;
        private readonly FileNotifierManager _fileManager;
        private readonly Configuration _configuration;

        public QuickSilver(Configuration configuration, IProcessWorker processWorker, FileNotifierManager fileManager, ILogger logger)
        {
            _configuration = configuration;
            _processWorker = processWorker;
            _logger = logger;
            _fileManager = fileManager;
        }

        public void Run()
        {
            _logger.Info("Quicksiler started.");
            _logger.Info("Configuration was loaded.");
            _fileManager.Set(new ObserveFileDto
            {
                DirectoryPath = _configuration.DirectoryPath,
                Filter = "*.*",
                WithSubDirectories = true
            });
            _processWorker.Start();
        }

        public void Kill()
        {
            _processWorker.Kill();
            _fileManager.Remove(_configuration.DirectoryPath);
            _logger.Warn("Quicksiler will be closed");
        }

        public void Restart()
        {
            _processWorker.Restart();
        }
    }
}
