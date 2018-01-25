using Ninject;
using Prism.Ninject;
using ScreenLocker.Administrator.Views;
using ScreenLocker.Data;
using ScreenLocker.Data.Contracts;
using ScreenLocker.Model;
using ScreenLocker.Model.Contracts;
using System.Windows;

namespace ScreenLocker.Administrator
{
    public class Bootstrapper : NinjectBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Kernel.Get<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureKernel()
        {
            base.ConfigureKernel();
            Kernel.Bind<IContext>().To<ScreenLockerContext>();
            Kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
            Kernel.RegisterTypeForNavigation<Details>();
            Kernel.RegisterTypeForNavigation<Views.Student>();
        }
    }
}
