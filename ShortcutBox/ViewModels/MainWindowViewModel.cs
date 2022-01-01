﻿namespace ShortcutBox.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using Prism.Commands;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private FileSystemInfo selectedFileInfo;

        private DelegateCommand copyFullPathCommand;
        private DelegateCommand copyParentDirectoryPathCommand;

        public MainWindowViewModel()
        {
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ObservableCollection<FileSystemInfo> Files { get; private set; } = new ObservableCollection<FileSystemInfo>();

        public FileSystemInfo SelectedFileInfo { get => selectedFileInfo; set => SetProperty(ref selectedFileInfo, value); }

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
    }
}
