using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using FileNotifier;

namespace FileWatcher
{
    public class FileWatchDog : IFileObserver
    {
        private readonly ObserveFileDto _dto;
        private readonly List<IFileNotifier> _notifier;
        private  FileSystemWatcher _fileSystemWatcher;

        public FileWatchDog(ObserveFileDto dto, params IFileNotifier[] notifier)
        {
            _dto = dto;
            _notifier = new List<IFileNotifier>(notifier);
        }

        private void FileSystemWatcherOnRenamed(object sender, RenamedEventArgs renamedEventArgs)
        {
            _notifier.ForEach(t => t.OnRename(renamedEventArgs));
        }


        private void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            _notifier.ForEach(t => t.OnCreated(fileSystemEventArgs));
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Start()
        {
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = _dto.DirectoryPath,
                Filter = _dto.Filter,
                NotifyFilter = NotifyFilters.CreationTime|NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.DirectoryName
            };

            _fileSystemWatcher.Created += FileSystemWatcherOnChanged;
            _fileSystemWatcher.Changed += FileSystemWatcherOnChanged;
            _fileSystemWatcher.Renamed += FileSystemWatcherOnRenamed;
            _fileSystemWatcher.Deleted += FileSystemWatcherOnChanged;

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
        }
    }
}