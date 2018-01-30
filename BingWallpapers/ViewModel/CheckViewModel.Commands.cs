using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingWallpapers.ViewModel
{
    partial class CheckViewModel
    {
        public BindingCommand CancelCommand => new BindingCommand
        {
            ExecuteAction = o => cancelCheck(),
        };
    }
}
