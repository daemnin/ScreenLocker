using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ScreenLocker.Model
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public string ControlNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string SecondLastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get => string.Join(" ", (new[] { FirstName, MiddleName, LastName, SecondLastName }).Where(s => !string.IsNullOrEmpty(s)));
        }

        public bool IsActive { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}
