using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.Win32;

namespace BingWallpapers
{
    static class InstanceChecker
    {
        public static bool IsUniqueInstance => Api.GetSameRunningProgram() == null;
        public static void SwitchToRunningInstance()
        {
            var process = Api.GetSameRunningProgram();
            if (process is null)
            {
                return;
            }
            Api.BringToFront(process);
        }
    }
}
