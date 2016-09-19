using System.IO;
using ConfigManager;
using System.Diagnostics;
using Quicksilver.Filters;
using Quicksilver.Logger;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System;
using System.ComponentModel;

namespace Quicksilver.ProcessWorker
{

    public class ProcessNotifier : IProcessNotifier, IDisposable
    {
        private readonly IFileFilter _fileFilter;
        private readonly Configuration _configuration;
        private readonly ILogger _logger;
        private Process _process;
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
                if (!_process.HasExited)
                    _process.Kill();
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
            try
            {
                if (_isRunning == false)
                {
                    _process = CreateProcess();
                    _process.Start();
                    _logger.Warn($"Start process: {_configuration.Program} pid: {_process.Id}");
                    RedirectOutput();
                    _isRunning = true;
                }
            }
            catch(Win32Exception ex)
            {
                throw new ArgumentException("Specify full path for program in quicksilver.json file.", ex);
            }
        }

        private void RedirectOutput()
        {
            _process.BeginErrorReadLine();
            _process.BeginOutputReadLine();
            _process.ErrorDataReceived += (s, e) =>
            {
                DisplayOnOutput(e);
            };
            _process.OutputDataReceived += (s, e) =>
            {
                DisplayOnOutput(e);
            };
            _process.Exited += (s, e) =>
            {
                int exitCode = _process.ExitCode;
                string message = $"Process exit code: {exitCode}";
                if (exitCode == 0)
                    _logger.Log(message);
                else
                    _logger.Error(message);
            };
        }

        private void DisplayOnOutput(DataReceivedEventArgs e)
        {
            string data = e?.Data?.Trim();
            if (!string.IsNullOrEmpty(data))
                _logger.Debug(data);
        }

        private Process CreateProcess()
        {
            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = _configuration.Program,
                Arguments= _configuration.Arguments,
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
