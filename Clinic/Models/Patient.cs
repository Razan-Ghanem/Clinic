using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Patient
{
        [Key]
        [Required]
        public long Id { get; set; }
        [RegularExpression("^[A-za-z]+$")]

        public string FirstName { get; set; }

        [RegularExpression("^[A-za-z]+$")]
        public string LastName { get; set; }
        [Display(Name = "DateOfBirth")]

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }
        public string Gender { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "Maximum length is{1}")]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RegistrationDate { get; set; }

        [RegularExpression("^\\d{9}$")]
        public string SSN { get; set; }
        public List<Appointment> Appointments { get; set; }

        public List<MedicalHistory> MedicalHistory { get; set; }
        [Display(Name = "Patient")]

        public string PatientName
        {
            get { return $"{FirstName } {LastName}"; }
        }
        public string Country { get; set; }




    }
}
