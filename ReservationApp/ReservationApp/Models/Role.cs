using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class Role
    {
        [Required(ErrorMessage = "The role name field is required !")]
        [Display(Name = "Enter the name")]
        public string RoleName { get; set; }
    }
}
