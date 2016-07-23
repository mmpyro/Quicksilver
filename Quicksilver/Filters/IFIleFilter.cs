using System.IO;
using System.Reactive.Subjects;

namespace Quicksilver.Filters
{
    public interface IFileFilter
    {
        string[] Filters { get; set; }
        void Filter(FileSystemEventArgs arg, Subject<string> subject);
        void Filter(RenamedEventArgs arg, Subject<string> subject);
    }
}
