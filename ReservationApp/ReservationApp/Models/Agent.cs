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
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "The agent name field is required !")]
        [Display(Name = "Enter The Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The service field is required !")]
        [Display(Name = "Service")]
        public Guid ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }
}
