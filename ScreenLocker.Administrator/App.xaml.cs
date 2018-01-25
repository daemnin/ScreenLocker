using Ninject;
using Prism.Regions;
using System.Windows;

namespace ScreenLocker.Administrator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootstrapper = new Bootstrapper();

            bootstrapper.Run();
            var regionManager = bootstrapper.Kernel.Get<IRegionManager>();
            regionManager.RequestNavigate(Constants.MAIN_REGION, "Details");
        }
    }
}
