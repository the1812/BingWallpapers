using BingWallpapers.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace BingWallpapers
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
            {
                var dllName = new AssemblyName(eventArgs.Name).Name + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(name => name.EndsWith(dllName));
                if (resourceName == null)
                {
                    return null;
                }
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    var assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            Settings.Load();
            if (e.Args.Length > 0 && e.Args[0].ToLower() == "--silent")
            {
                SilentChecker.Start();
                Shutdown();
            }
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            Settings.Save();
            base.OnExit(e);
        }
    }
}
