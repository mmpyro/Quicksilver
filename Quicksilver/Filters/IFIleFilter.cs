using System;
using System.IO;

namespace Quicksilver.Filters
{
    public interface IFileFilter
    {
        string[] Filters { get; set; }
        void Filter(FileSystemEventArgs arg, Action restart);
        void Filter(RenamedEventArgs arg, Action restart);
    }
}
