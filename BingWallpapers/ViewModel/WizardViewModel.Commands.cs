using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ace.Files.Json;
using Ace.Wpf.Mvvm;
using BingWallpapers.Model;
using BingWallpapers.View;

namespace BingWallpapers.ViewModel
{
    partial class WizardViewModel
    {
        public BindingCommand BrowseCommand => new BindingCommand
        {
            ExecuteAction = async o =>
            {
                IsButtonEnabled = false;
                var dialog = new FolderBrowserDialog
                {
                    Description = this["Title"],
                    SelectedPath = Path,
                    ShowNewFolderButton = true,
                };
                var result = DialogResult.Cancel;
                await Task.Delay(500);
                await Task.Run(() =>
                {
                    View.Dispatcher.Invoke(() =>
                    {
                        result = dialog.ShowDialog();
                    });
                });
                if (result == DialogResult.OK)
                {
                    Path = dialog.SelectedPath;
                }
                IsButtonEnabled = true;
            },
        };
        public BindingCommand OKCommand => new BindingCommand
        {
            ExecuteAction = async o =>
            {
                IsButtonEnabled = false;
                await Task.Run(() =>
                {
                    if (Directory.Exists(Path))
                    {
                        Settings.DownloadPath = Path;
                        Task.Delay(500).Wait();
                        var uri = new Uri("../View/CheckView.xaml", UriKind.Relative);
                        View.Dispatcher.Invoke(() =>
                        {
                            MainViewModel.Current.Navigate(uri);
                        });
                    }
                    else
                    {
                        MessageBox.Show(this["PathNotExist"], this["Error"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
                IsButtonEnabled = true;
            },
        };
    }
}
