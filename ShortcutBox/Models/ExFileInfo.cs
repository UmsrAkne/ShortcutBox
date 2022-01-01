namespace ShortcutBox.Models
{
    using System.IO;

    public class ExFileInfo
    {
        public ExFileInfo(FileSystemInfo fileInfo)
        {
            FileSystemInfo = fileInfo;
            IsDirectory = fileInfo is DirectoryInfo;
        }

        public FileSystemInfo FileSystemInfo { get; private set; }

        public bool IsDirectory { get; private set; }

        public string Name => FileSystemInfo.Name;

        public string FullName => FileSystemInfo.FullName;

        public bool Exists => FileSystemInfo.Exists;
    }
}
