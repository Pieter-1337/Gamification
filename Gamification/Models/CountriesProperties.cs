using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamification.Models
{
    public class CountriesProperties
    {
        [Required (ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [Required (ErrorMessage = "This field is required")]
        public string Abbreviation { get; set; }
    }
}