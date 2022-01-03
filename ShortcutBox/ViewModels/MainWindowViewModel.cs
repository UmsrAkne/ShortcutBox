namespace ShortcutBox.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using Prism.Commands;
    using Prism.Mvvm;
    using ShortcutBox.Models;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private ExFileInfo selectedFileInfo;

        private DelegateCommand copyFullPathCommand;
        private DelegateCommand copyParentDirectoryPathCommand;
        private DelegateCommand openFileCommand;
        private DelegateCommand clearFileListCommand;

        public MainWindowViewModel()
        {
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
    }
}
