using Prism.Mvvm;

namespace ScreenLocker.Models
{
    public class Login : BindableBase
    {
        private string controlNumber;
        public string ControlNumber
        {
            get => controlNumber;
            set => SetProperty(ref controlNumber, value);
        }
    }
}