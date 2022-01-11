namespace ShortcutBox.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using Prism.Commands;
    using Prism.Mvvm;
    using ShortcutBox.Models;
    using ShortcutBox.Models.DBs;

    public class FileHistoryViewModel : BindableBase
    {
        private ObservableCollection<FileHistory> fileHistories;
        private FileHistory selectedItem;
        private DelegateCommand copyFullPathCommand;
        private DelegateCommand copyParentDirectoryPathCommand;
        private DelegateCommand openFileCommand;

        public ObservableCollection<FileHistory> FileHistories
        {
            get => fileHistories;
            set => SetProperty(ref fileHistories, value);
        }


        public FileHistory SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        public DelegateCommand CopyFullPathCommand
        {
            get => copyFullPathCommand ?? (copyFullPathCommand = new DelegateCommand(() =>
            {
                if (SelectedItem != null)
                {
                    Clipboard.SetText(SelectedItem.FullPath);
                }
            }));
        }

        public DelegateCommand CopyParentDirectoryPathCommand
        {
            get => copyParentDirectoryPathCommand ?? (copyParentDirectoryPathCommand = new DelegateCommand(() =>
            {
                if (SelectedItem != null)
                {
                    Clipboard.SetText(Path.GetDirectoryName(SelectedItem.FullPath));
                }
            }));
        }

        public DelegateCommand OpenFileCommand
        {
            get => openFileCommand ?? (openFileCommand = new DelegateCommand(() =>
            {
                if (SelectedItem != null && new ExFileInfo(SelectedItem.FullPath).Exists)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = SelectedItem.FullPath;
                    p.Start();
                }
            }));
        }
    }
}
