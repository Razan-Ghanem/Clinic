using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Models
{
    public class AppType
    {
        [Key]
        [Required]
        public long AppId { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is{1}")]
        [Display(Name = "Type")]

        public string ?AppointementType { get; set; }
  
    }
}
