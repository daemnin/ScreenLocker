using Prism.Mvvm;
using System;

namespace ScreenLocker.Administrator.Models
{
    public class StudentModel : BindableBase
    {
        public int Id { get; set; }

        public DateTime? LastLogin { get; set; }

        private string controlNumber;
        public string ControlNumber
        {
            get { return controlNumber; }
            set { SetProperty(ref controlNumber, value); }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }

        private string middleName;
        public string MiddleName
        {
            get { return middleName; }
            set { SetProperty(ref middleName, value); }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value); }
        }

        private string secondLastName;
        public string SecondLastName
        {
            get { return secondLastName; }
            set { SetProperty(ref secondLastName, value); }
        }

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { SetProperty(ref isActive, value); }
        }
    }
}