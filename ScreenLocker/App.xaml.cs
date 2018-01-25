using System.Diagnostics;
using System.IO;
using System.Windows;

namespace ScreenLocker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (File.Exists("run.bat"))
            {
                Process.Start("cmd.exe", $"/c run.bat");
            }
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}