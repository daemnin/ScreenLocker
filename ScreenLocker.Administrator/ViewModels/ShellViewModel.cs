using Prism.Mvvm;
using Prism.Regions;

namespace ScreenLocker.Administrator.ViewModels
{
    public class ShellViewModel : BindableBase, INavigationAware
    {
        public ShellViewModel()
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}