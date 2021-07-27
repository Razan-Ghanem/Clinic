using Clinic.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Doctor
{
        [Key]
        [Required]
        public long Id { get; set; }
        [StringLength(50,ErrorMessage ="Maximum length is{1}")]
        [RegularExpression("^[A-za-z]+$")]
        public string FirstName { get; set; }
        [StringLength(50, ErrorMessage = "Maximum length is{1}")]
        [RegularExpression("^[A-za-z]+$")]
        public string LastName { get; set; }
        [StringLength(100, ErrorMessage = "Maximum length is{1}")]
        public string Address { get; set; }
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Currency)]
        public Decimal? MonthlySalary { get; set; }
        public string IBAN { get; set; }
        public List<Appointment>Appointments { get; set; }
        public long SpecializationId { get; set; }
        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }

        [Display(Name = "Doctor")]
        public string DoctorName
        {
            get { return $"{FirstName } {LastName}"; }
        }

        [StringLength(100, ErrorMessage = "Maximum length is{1}")]
        public string Country { get; set; }
        



    }
}
