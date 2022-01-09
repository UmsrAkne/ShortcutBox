namespace ShortcutBox.ViewModels
{
    using System.Collections.Generic;
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
        private ObservableCollection<ExFileInfo> files = new ObservableCollection<ExFileInfo>();
        private ExFileInfo selectedFileInfo;
        private FileHistoryDbContext databaseContext = new FileHistoryDbContext();
        private SortingPropertyName sortingPropertyName = SortingPropertyName.Index;
        private bool orderReverse;

        private DelegateCommand copyFullPathCommand;
        private DelegateCommand copyParentDirectoryPathCommand;
        private DelegateCommand openFileCommand;
        private DelegateCommand clearFileListCommand;
        private DelegateCommand saveStatusCommand;
        private DelegateCommand restoreFilesCommand;
        private DelegateCommand<object> sortCommand;
        private DelegateCommand reverseOrderCommand;

        public MainWindowViewModel()
        {
            databaseContext.CreateDatabase();
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ObservableCollection<ExFileInfo> Files
        {
            get => files;
            private set => SetProperty(ref files, value);
        }

        public ExFileInfo SelectedFileInfo { get => selectedFileInfo; set => SetProperty(ref selectedFileInfo, value); }

        public bool OrderReverse { get => orderReverse; set => SetProperty(ref orderReverse, value); }

        public DelegateCommand CopyFullPathCommand
        {
            get => copyFullPathCommand ?? (copyFullPathCommand = new DelegateCommand(() =>
            {
                if (SelectedFileInfo != null)
                {
                    Clipboard.SetText(SelectedFileInfo.FullName);
                    databaseContext.SaveCopiedText(SelectedFileInfo.FullName);
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
                    databaseContext.SaveCopiedText(Path.GetDirectoryName(SelectedFileInfo.FullName));
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
                AddFiles(fileHistories.Select(h => new ExFileInfo(h.FullPath)).ToList());
            }));
        }

        public DelegateCommand<object> SortCommand
        {
            get => sortCommand ?? (sortCommand = new DelegateCommand<object>((object propName) =>
            {
                sortingPropertyName = (SortingPropertyName)propName;

                switch (sortingPropertyName)
                {
                    case SortingPropertyName.FileName:
                        Files = new ObservableCollection<ExFileInfo>(
                            OrderReverse ?
                                Files.OrderByDescending(f => f.Name) :
                                Files.OrderBy(f => f.Name));
                        break;

                    case SortingPropertyName.Index:
                        Files = new ObservableCollection<ExFileInfo>(
                            OrderReverse ?
                                Files.OrderByDescending(f => f.Index) :
                                Files.OrderBy(f => f.Index));
                        break;

                    case SortingPropertyName Kind:
                        Files = new ObservableCollection<ExFileInfo>(
                            OrderReverse ?
                                Files.OrderByDescending(f => f.IsDirectory).ThenByDescending(f => f.Name) :
                                Files.OrderBy(f => f.IsDirectory).ThenBy(f => f.Name));
                        break;
                }
            }));
        }

        public DelegateCommand ReverseOrderCommand
        {
            get => reverseOrderCommand ?? (reverseOrderCommand = new DelegateCommand(() =>
            {
                OrderReverse = !OrderReverse;
                SortCommand.Execute(sortingPropertyName);
            }));
        }

        public void SaveHistory(ExFileInfo fileInfo)
        {
            databaseContext.AddHistory(fileInfo);
        }

        public void AddFiles(List<ExFileInfo> files)
        {
            Enumerable.Range(0, files.Count() - 1).ToList().ForEach(i => files[i].Index = i);
            Files.AddRange(files);
        }
    }
}
