using FileNotifier;
using FileWatcher;
using Ploeh.AutoFixture;
using System.IO;

namespace QuickSilverSpecyfication
{
    internal class FakeWatchDog : IFileObserver
    {
        private readonly ObserveFileDto dto;
        private readonly IFileNotifier[] notifier;
        private readonly Fixture _any = new Fixture();

        public FakeWatchDog(ObserveFileDto dto, IFileNotifier[] notifier)
        {
            this.dto = dto;
            this.notifier = notifier;
        }

        public void NotifyOnCreate()
        {
            foreach (var item in notifier)
            {
                item.OnCreated(_any.Create<FileSystemEventArgs>());
            }
        }

        public void NotifyOnRename()
        {
            foreach (var item in notifier)
            {
                item.OnRename(_any.Create<RenamedEventArgs>());
            }
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}