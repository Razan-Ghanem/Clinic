using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Country
{
    public class CountryModel
    {

        public CountryModel()
        {

        }

        public CountryModel(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        public string Name { get; set; }
    }
}
