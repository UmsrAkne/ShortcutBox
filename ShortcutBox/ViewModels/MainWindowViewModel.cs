namespace ShortcutBox.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Prism.Commands;
    using Prism.Mvvm;
    using ShortcutBox.Models;
    using ShortcutBox.Models.DBs;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private ExFileInfo selectedFileInfo;
        private FileHistoryDbContext databaseContext = new FileHistoryDbContext();

        private DelegateCommand copyFullPathCommand;
        private DelegateCommand copyParentDirectoryPathCommand;
        private DelegateCommand openFileCommand;
        private DelegateCommand clearFileListCommand;
        private DelegateCommand saveStatusCommand;
        private DelegateCommand restoreFilesCommand;

        public MainWindowViewModel()
        {
            databaseContext.CreateDatabase();
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ObservableCollection<ExFileInfo> Files { get; private set; } = new ObservableCollection<ExFileInfo>();

        public ExFileInfo SelectedFileInfo { get => selectedFileInfo; set => SetProperty(ref selectedFileInfo, value); }

        public DelegateCommand CopyFullPathCommand
        {
            get => copyFullPathCommand ?? (copyFullPathCommand = new DelegateCommand(() =>
            {
                if (SelectedFileInfo != null)
                {
                    Clipboard.SetText(SelectedFileInfo.FullName);
                }
            }));
        }

        public DelegateCommand CopyParentDirectoryPathCommand
        {
            get => copyParentDirectoryPathCommand ?? (copyParentDirectoryPathCommand = new DelegateCommand(() =>
            {
                if (SelectedFileInfo != null)
                {
                    Clipboard.SetText(Path.GetDirectoryName(SelectedFileInfo.FullName));
                }
            }));
        }

        public DelegateCommand OpenFileCommand
        {
            get => openFileCommand ?? (openFileCommand = new DelegateCommand(() =>
            {
                if (SelectedFileInfo != null && SelectedFileInfo.Exists)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = SelectedFileInfo.FullName;
                    p.Start();
                }
            }));
        }

        public DelegateCommand ClearFileListCommand
        {
            get => clearFileListCommand ?? (clearFileListCommand = new DelegateCommand(() =>
            {
                Files.Clear();
            }));
        }

        public DelegateCommand SaveStatusCommand
        {
            get => saveStatusCommand ?? (saveStatusCommand = new DelegateCommand(() =>
            {
                if (Files.Count == 0)
                {
                    return;
                }

                databaseContext.FileHistories.ToList().ForEach(file => file.UsedLastTime = false);

                Files.ToList().ForEach(f =>
                {
                    var fileInfo = databaseContext.FileHistories.Where(file => file.FullPath == f.FullName).FirstOrDefault();
                    if (fileInfo != null)
                    {
                        fileInfo.UsedLastTime = true;
                    }
                });

                databaseContext.SaveChanges();
            }));
        }

        public DelegateCommand RestoreFilesCommand
        {
            get => restoreFilesCommand ?? (restoreFilesCommand = new DelegateCommand(() =>
            {
                var fileHistories = databaseContext.FileHistories.Where(f => f.UsedLastTime);
                Files.AddRange(fileHistories.Select(h => new ExFileInfo(h.FullPath)));
            }));
        }

        public void SaveHistory(ExFileInfo fileInfo)
        {
            databaseContext.AddHistory(fileInfo);
        }
    }
}
