using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BingWallpapers.Model
{
    sealed class BindingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private void onCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public Func<object, bool> CanExecuteFunc { get; set; }
        public Action<object> ExecuteAction { get; set; }
        public bool CanExecute(object parameter) => CanExecuteFunc?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => ExecuteAction?.Invoke(parameter);
    }
}
