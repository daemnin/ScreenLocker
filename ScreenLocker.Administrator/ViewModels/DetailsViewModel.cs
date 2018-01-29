using Ninject;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using ScreenLocker.Data.Contracts;
using ScreenLocker.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ScreenLocker.Administrator.ViewModels
{
    public class DetailsViewModel : BindableBase, INavigationAware
    {
        private IKernel kernel;
        private IRegionManager regionManager;

        #region Properties
        private string search;
        public string Search
        {
            get { return search; }
            set { SetProperty(ref search, value); }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private Student selectedItem;
        public Student SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value); }
        }

        private ICollectionView studentsView;
        public ICollectionView StudentsView
        {
            get { return studentsView; }
            set { SetProperty(ref studentsView, value); }
        }

        public ObservableCollection<Student> Students { get; set; }
        #endregion

        #region Commands
        public ICommand SearchingCommand => new DelegateCommand(OnSearching);
        public ICommand AddCommand => new DelegateCommand(OnAdding);
        public ICommand ChangeStatusCommand => new DelegateCommand(OnChangeStatus, SelectedItemCanExecute).ObservesProperty(() => SelectedItem);
        public ICommand EditCommand => new DelegateCommand(OnEditing, SelectedItemCanExecute).ObservesProperty(() => SelectedItem);
        public ICommand DeleteCommand => new DelegateCommand(OnDeleting, SelectedItemCanExecute).ObservesProperty(() => SelectedItem);

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }
        #endregion

        public DetailsViewModel(IKernel kernel, IRegionManager regionManager)
        {
            this.kernel = kernel;
            this.regionManager = regionManager;
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
        }

        private async Task Load()
        {
            IsBusy = true;

            try
            {
                using (var unitOfWork = kernel.Get<IUnitOfWork>())
                {
                    Students = new ObservableCollection<Student>(unitOfWork.StudentRepository.GetAll());
                    StudentsView = CollectionViewSource.GetDefaultView(Students);
                    StudentsView.Filter = new Predicate<object>(Fillter);
                }
            }
            catch (Exception ex)
            {
                Status = $"Error: {ex.Message}";
            }

            IsBusy = false;
        }

        private bool SelectedItemCanExecute()
        {
            return SelectedItem != null;
        }

        private bool Fillter(object obj)
        {
            Student s = obj as Student;
            return string.IsNullOrEmpty(Search) ||
                   s.ControlNumber.Contains(Search) ||
                   s.FullName.ToLower().ToLower().Contains(Search.ToLower()) ||
                   (s.IsActive ? "habilitado" : "deshabilitado").StartsWith(Search.ToLower()) ||
                   (s.LastLogin?.ToLocalTime().ToString("g").Contains(Search) == true);
        }

        private void OnSearching()
        {
            StudentsView.Refresh();
        }

        private void OnAdding()
        {
            Status = string.Empty;
            regionManager.RequestNavigate(Constants.MAIN_REGION, "Student");
        }

        private void OnEditing()
        {
            Status = string.Empty;
            var navigationParameters = new NavigationParameters
            {
                { "entity", SelectedItem }
            };
            regionManager.RequestNavigate(Constants.MAIN_REGION, "Student", navigationParameters);
        }

        private void OnDeleting()
        {
            Status = string.Empty;
            ConfirmationRequest.Raise(new Confirmation
            {
                Title = "Confirmación",
                Content = $"El alumno con matrícula {SelectedItem.ControlNumber} será eliminado."
            },
            async response =>
            {
                if (response.Confirmed)
                {
                    try
                    {
                        using (var unitOfWork = kernel.Get<IUnitOfWork>())
                        {
                            unitOfWork.StudentRepository.Delete(SelectedItem.Id);
                            var deleted = unitOfWork.Save() > 0;
                            if (deleted)
                            {
                                Status = $"El alumno {SelectedItem.FullName} fue eliminado.";
                                await Task.Run(Load);
                            }
                            else
                            {
                                Status = $"El alumno {SelectedItem.FullName} no pudo ser eliminado.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Status = $"Error: {ex.Message}";
                    }
                }
                else
                {
                    Status = "Operación cancelada.";
                }
            });
        }

        private void OnChangeStatus()
        {
            Status = string.Empty;
            try
            {
                SelectedItem.IsActive = !SelectedItem.IsActive;
                using (var unitOfWork = kernel.Get<IUnitOfWork>())
                {
                    unitOfWork.StudentRepository.Update(SelectedItem);
                    var updated = unitOfWork.Save() > 0;

                    if (updated)
                    {
                        Status = $"El alumno {SelectedItem.FullName} se actualizó correctamente.";
                        SelectedItem = null;
                        StudentsView.Refresh();
                    }
                    else
                    {
                        Status = $"El alumno {SelectedItem.FullName} no pudo ser actualizado.";
                    }
                }
            }
            catch (Exception ex)
            {
                Status = $"Error: {ex.Message}";
            }
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Status = string.Empty;
            await Task.Run(Load);
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