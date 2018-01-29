using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingWallpapers.ViewModel
{
    partial class WizardViewModel
    {
        public BindingCommand BrowseCommand => new BindingCommand
        {
            ExecuteAction = async o =>
            {
                var dialog = new FolderBrowserDialog
                {
                    Description = this["Title"],
                    SelectedPath = Path,
                    ShowNewFolderButton = true,
                };
                var result = DialogResult.Cancel;
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
            },
        };
    }
}
