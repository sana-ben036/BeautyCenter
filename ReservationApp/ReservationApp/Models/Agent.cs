using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "The agent first name field is required !")]
        [Display(Name = "Enter The First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The agent last name field is required !")]
        [Display(Name = "Enter The Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Adress { get; set; }

        [Required(ErrorMessage = "The service field is required !")]
        [Display(Name = "Service")]
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }
}
