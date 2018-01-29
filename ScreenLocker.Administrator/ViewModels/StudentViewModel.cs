using Ninject;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using ScreenLocker.Administrator.Models;
using ScreenLocker.Data.Contracts;
using ScreenLocker.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ScreenLocker.Administrator.ViewModels
{
    public class StudentViewModel : BindableBase, INavigationAware
    {
        private IRegionManager regionManager;
        private IKernel kernel;

        private StudentModel model;
        public StudentModel Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private string status;
        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        public ICommand CancelCommand => new DelegateCommand(OnCancel);
        public ICommand SaveCommand => new DelegateCommand(OnSave, SaveCanExecute)
                                                .ObservesProperty(() => Model.ControlNumber)
                                                .ObservesProperty(() => Model.FirstName)
                                                .ObservesProperty(() => Model.MiddleName)
                                                .ObservesProperty(() => Model.LastName)
                                                .ObservesProperty(() => Model.SecondLastName);

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }

        public StudentViewModel(IKernel kernel, IRegionManager regionManager)
        {
            this.kernel = kernel;
            this.regionManager = regionManager;
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
        }

        private void OnSave()
        {
            var student = new Student
            {
                Id = Model.Id,
                ControlNumber = Model.ControlNumber,
                FirstName = Model.FirstName,
                MiddleName = Model.MiddleName,
                LastName = Model.LastName,
                SecondLastName = Model.SecondLastName,
                IsActive = Model.IsActive
            };

            using (var unitOfWork = kernel.Get<IUnitOfWork>())
            {
                try
                {
                    if (student.Id > 0)
                    {
                        unitOfWork.StudentRepository.Update(student);
                    }
                    else
                    {
                        unitOfWork.StudentRepository.Create(student);
                    }

                    var succeded = unitOfWork.Save() > 0;

                    if (succeded)
                    {
                        regionManager.RequestNavigate(Constants.MAIN_REGION, "Details");
                    }
                    else
                    {
                        Status = $"Se generó un error al intentar guardar los cambios.";
                    }
                }
                catch (Exception ex)
                {
                    Status = $"Error: {ex.Message}";
                }
            }
        }

        private bool SaveCanExecute()
        {
            return !string.IsNullOrEmpty(Model?.ControlNumber) &&
                   Regex.IsMatch(Model?.ControlNumber, "^[0-9]{8,8}") &&
                   !string.IsNullOrEmpty(Model?.FirstName) &&
                   Regex.IsMatch(Model?.FirstName, "^[a-zA-ZüÜáéíóúÁÉÍÓÚñÑ]{3,50}") &&
                   !string.IsNullOrEmpty(Model?.LastName) &&
                   Regex.IsMatch(Model?.LastName, "^[a-zA-ZüÜáéíóúÁÉÍÓÚñÑ]{3,50}") &&
                   (string.IsNullOrEmpty(Model?.MiddleName) || Regex.IsMatch(Model?.MiddleName, "^[a-zA-ZüÜáéíóúÁÉÍÓÚñÑ]{1,50}")) &&
                   (string.IsNullOrEmpty(Model?.SecondLastName) || Regex.IsMatch(Model?.SecondLastName, "^[a-zA-ZüÜáéíóúÁÉÍÓÚñÑ]{1,50}"));
        }

        private void OnCancel()
        {
            ConfirmationRequest.Raise(new Confirmation
            {
                Title = "Confirmación",
                Content = "¿Está seguro de que desea cancelar los cambios?"
            },
            response =>
            {
                if (response.Confirmed)
                {
                    regionManager.RequestNavigate(Constants.MAIN_REGION, "Details");
                }
            });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Status = string.Empty;
            if (navigationContext.Parameters.Any(p => p.Key.Equals("entity")))
            {
                var student = navigationContext.Parameters["entity"] as Student;
                Model = new StudentModel
                {
                    Id = student.Id,
                    ControlNumber = student.ControlNumber,
                    FirstName = student.FirstName,
                    MiddleName = student.MiddleName,
                    LastName = student.LastName,
                    SecondLastName = student.SecondLastName,
                    IsActive = student.IsActive,
                    LastLogin = student.LastLogin
                };
                Title = "Edición de estudiante";
            }
            else
            {
                Title = "Creación de estudiante";
                Model = new StudentModel
                {
                    IsActive = true
                };
            }
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
