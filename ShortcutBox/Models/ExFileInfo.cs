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

        public ExFileInfo(string path)
        {
            if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                FileSystemInfo = new DirectoryInfo(path);
            }
            else
            {
                FileSystemInfo = new FileInfo(path);
            }

            IsDirectory = FileSystemInfo is DirectoryInfo;
        }

        public FileSystemInfo FileSystemInfo { get; private set; }

        public bool IsDirectory { get; private set; }

        public string Name => FileSystemInfo.Name;

        public string FullName => FileSystemInfo.FullName;

        public bool Exists => FileSystemInfo.Exists;

        public int Index { get; set; }
    }
}
