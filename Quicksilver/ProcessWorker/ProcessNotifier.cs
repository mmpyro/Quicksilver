using System.IO;
using ConfigManager;
using System.Diagnostics;
using Quicksilver.Filters;
using Quicksilver.Logger;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System;

namespace Quicksilver.ProcessWorker
{

    public class ProcessNotifier : IProcessNotifier, IDisposable
    {
        private readonly IFileFilter _fileFilter;
        private readonly Configuration _configuration;
        private readonly ILogger _logger;
        private Process _process;
        private Task _soutReader;
        private CancellationTokenSource _cancelationToken = new CancellationTokenSource();
        private CancellationToken _token;
        private bool _isRunning = false;
        private readonly Subject<string> _subject;
        private readonly IDisposable _subscryption;

        public ProcessNotifier(Configuration configuration, IFileFilter fileFilter, ILogger logger)
        {
            _fileFilter = fileFilter;
            _configuration = configuration;
            _fileFilter.Filters = _configuration.Filters;
            _logger = logger;
            _subject = new Subject<string>();
            _subscryption = _subject.Throttle(TimeSpan.FromSeconds(1)).Subscribe(_ => Restart());
        }

        public void Kill()
        {
            if (_isRunning == true)
            {
                _logger.Warn($"Stop prcess with pid: {_process.Id}");
                if(!_process.HasExited)
                    _process.Kill();
                _cancelationToken.Cancel();
                _cancelationToken = new CancellationTokenSource();
                _isRunning = false;
            }
        }

        public void OnCreated(FileSystemEventArgs arg)
        {
            _fileFilter.Filter(arg, _subject);
            
        }

        public void OnRename(RenamedEventArgs arg)
        {
            _fileFilter.Filter(arg, _subject);
        }

        public void Restart()
        {
            if (_isRunning == true)
            {
                Kill();
                Start();
            }
        }

        public void Start()
        {
            if (_isRunning == false)
            {
                _token = _cancelationToken.Token;
                _process = CreateProcess();
                _process.Start();
                _logger.Warn($"Start process: {_configuration.Program} pid: {_process.Id}");
                _soutReader = Task.Run(() =>
                {
                    using (StreamReader sr = new StreamReader(_process.StandardOutput.BaseStream, Encoding.UTF8))
                    {
                        while (!_cancelationToken.IsCancellationRequested && !sr.EndOfStream)
                        {
                            _logger.Debug(sr.ReadLine());
                        }
                    }
                });
                _isRunning = true;
            }
        }

        private Process CreateProcess()
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = _configuration.Program,
                Arguments= _configuration.StartUpFilePath,
                WorkingDirectory = _configuration.DirectoryPath,
                StandardOutputEncoding = Encoding.UTF8,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            return process;
        }

        public void Dispose()
        {
            _subscryption?.Dispose();
        }
    }
}
