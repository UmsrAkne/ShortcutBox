namespace ShortcutBox.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private FileSystemInfo selectedFileInfo;

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
    }
}
