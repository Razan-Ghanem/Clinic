using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class Appointment
    {   
        [Key]
        [Required]
        public long Id { get; set; }
        public long DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
        public long PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        [DataType(DataType.DateTime)]

        public DateTime Reservation { get; set; }

        public  long AppId { get; set; }
        [ForeignKey("AppId")]
        [Display(Name = "Appointment Type")]

        public AppType appType { get; set; }

     }
}
