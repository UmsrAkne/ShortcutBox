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
    }
}
