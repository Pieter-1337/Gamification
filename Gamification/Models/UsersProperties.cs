using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamification.Models
{
    public class UsersProperties
    {
        [Required(ErrorMessage = "This field is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress(ErrorMessage = "Plz enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "First Name")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }

        
        [Display(Name = "Points Level 1")]
        public Nullable<int> Punten_LVL1 { get; set; }

        [Display(Name = "Points Level 2")]
        public Nullable<int> Punten_LVL2 { get; set; }

        [Display(Name = "Division")]
        public int DivisionID { get; set; }

        [Display(Name = "Country")]
        public int CountryID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped] // Does not effect your database
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
       
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}