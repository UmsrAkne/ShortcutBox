namespace ShortcutBox.Models
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Microsoft.Xaml.Behaviors;
    using ViewModels;

    public class DragAndDropBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            // ファイルをドラッグしてきて、コントロール上に乗せた際の処理
            AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;

            // ファイルをドロップした際の処理
            AssociatedObject.Drop += AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            AssociatedObject.Drop -= AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            // ファイルパスの一覧の配列
            List<string> filePaths = new List<string>((string[])e.Data.GetData(DataFormats.FileDrop));

            List<ExFileInfo> files = filePaths.Select(p =>
            {
                if (File.GetAttributes(p).HasFlag(FileAttributes.Directory))
                {
                    return new ExFileInfo(new DirectoryInfo(p));
                }
                else
                {
                    return new ExFileInfo(new FileInfo(p));
                }
            }).ToList();

            var vm = ((Window)sender).DataContext as MainWindowViewModel;
            files.ForEach(f => vm.Files.Add(f));
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
    }
}