using System.IO;

namespace FileNotifier
{
    public interface IFileNotifier
    {
        void OnCreated(FileSystemEventArgs arg);
        void OnRename(RenamedEventArgs arg);
    }


}