using System;
using System.Runtime.Serialization;

namespace FileWatcher
{
    public class ObserveFileDto
    {
        public ObserveFileDto() : this("","*.*",true)
        {
            
        }

        public ObserveFileDto(string fileDirectoryPath, bool withSubDirectories = true) : this(fileDirectoryPath, "*.*", withSubDirectories)
        {
        }

        public ObserveFileDto(string directoryPath, string filter, bool withSubDirectories = true)
        {
            DirectoryPath = directoryPath;
            Filter = filter;
            WithSubDirectories = withSubDirectories;
        }

        public string DirectoryPath { get; set; }
        public string Filter { get; set; }
        public bool WithSubDirectories { get; set; }

        public override int GetHashCode()
        {
            return DirectoryPath.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var dto = obj as ObserveFileDto;
            if (obj != null)
            {
                return dto.DirectoryPath.Equals(DirectoryPath, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}