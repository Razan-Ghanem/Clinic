using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class MedicalHistory
    {
        public long Id { get; set; }
        public long PatientId {get; set;}
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        [StringLength(150, ErrorMessage = "Maximum length is{1}")]
        public string Description { get; set; }

    }
}
