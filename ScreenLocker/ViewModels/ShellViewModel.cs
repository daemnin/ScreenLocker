using Ninject;
using Prism.Commands;
using Prism.Mvvm;
using ScreenLocker.Data.Contracts;
using ScreenLocker.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScreenLocker.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private IKernel kernel;
        private int sessionTimer;

        #region Properties
        private string controlNumber;
        public string ControlNumber
        {
            get => controlNumber;
            set => SetProperty(ref controlNumber, value);
        }

        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        private string error;
        public string Error
        {
            get => error;
            set => SetProperty(ref error, value);
        }
        #endregion

        #region Commands
        public ICommand LoginCommand => new DelegateCommand(OnLoginExecuted, OnLoginCanExecute)
                                                    .ObservesProperty(() => ControlNumber);
        public ICommand KeyPressedCommand => new DelegateCommand<Key?>(OnEnterPressedExecuted);
        #endregion

        public ShellViewModel(IKernel kernel)
        {
            this.kernel = kernel;
            sessionTimer = int.Parse(ConfigurationManager.AppSettings[Constants.SESSION_TIMER]) * 1000 * 60;
        }

        private void OnEnterPressedExecuted(Key? key)
        {
            if (OnLoginCanExecute() && key == Key.Enter)
            {
                OnLoginExecuted();
            }
        }

        private bool OnLoginCanExecute()
        {
            return !string.IsNullOrEmpty(ControlNumber) && Regex.IsMatch(ControlNumber, "^[0-9]{8,8}$");
        }

        private void OnLoginExecuted()
        {
            try
            {
                using (var unitOfWork = kernel.Get<IUnitOfWork>())
                {
                    var student = unitOfWork.StudentRepository.FindBy(s => s.ControlNumber.Equals(ControlNumber)).FirstOrDefault();

                    if (student == null)
                    {
                        Error = "Número de control no registrado, pase a mostrador para darse de alta.";
                        return;
                    }
                    var log = new UsageLog
                    {
                        StudentId = student.Id,
                        Timestamp = DateTime.UtcNow
                    };

                    unitOfWork.UsageLogRepository.Create(log);

                    student.LastLogin = log.Timestamp;

                    unitOfWork.StudentRepository.Update(student);

                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                Error = $"Error: {ex.Message}";
            }

            ControlNumber = string.Empty;
            IsVisible = false;
            Error = string.Empty;

            Action timer = async () =>
            {
                await Task.Delay(sessionTimer);

                IsVisible = true;
            };
            timer();
        }
    }
}