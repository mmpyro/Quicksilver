using System;
using System.Collections.Generic;
using System.Linq;
using FileNotifier;

namespace FileWatcher
{
    public class FileNotifierManager : IFileNotifierManager
    {
        private readonly IFileNotifier[] _fileNotifier;
        private readonly Dictionary<ObserveFileDto, IFileObserver> _observedFiles;
        private List<IFileNotifier> notifiersList;

        public FileNotifierManager(params IFileNotifier[] fileNotifier)
        {
            _fileNotifier = fileNotifier;
            _observedFiles = new Dictionary<ObserveFileDto, IFileObserver>();
        }

        public void Set(ObserveFileDto fileToObserve)
        {
            IFileObserver adapter = FileObserver.Create(fileToObserve, _fileNotifier);
            AddToObserverList(fileToObserve,adapter);
        }

        private void AddToObserverList(ObserveFileDto fileToObserve, IFileObserver adapter)
        {
            if (!_observedFiles.ContainsKey(fileToObserve))
            {
                adapter.Start();
                _observedFiles.Add(fileToObserve, adapter);
            }
            else
                throw new ArgumentException("This path was added to observable list before.");
        }

        public void Remove(string filePath)
        {
            try
            {
                var fileDto =
                    _observedFiles.Single(t => t.Key.DirectoryPath.Equals(filePath, StringComparison.OrdinalIgnoreCase)).Key;
                var adapter = _observedFiles[fileDto];
                adapter.Stop();
                _observedFiles.Remove(fileDto);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<ObserveFileDto> PerformFileList()
        {
            return _observedFiles.Select(t => t.Key).ToList();
        }
    }
}