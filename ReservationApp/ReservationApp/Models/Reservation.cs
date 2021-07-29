using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class Reservation
    {

        [Key]
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "The Date field is required !")]
        [Display(Name = "Date of Reservation")]
        public DateTime BookingDate { get; set; }
        public Status Status { get; set; }
        
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

        [Required(ErrorMessage = "The service field is required !")]
        [Display(Name = "Service")]
        public Guid TypeId { get; set; }
        public virtual Service Service { get; set; }
    }
}
