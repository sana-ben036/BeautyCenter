using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "The service name field is required !")]
        [Display(Name = "Enter The Name")]
        public string Name { get; set; }

        public string Details { get; set; }

        public string ImagePath  { get; set; }

        public int Price { get; set; }

        public int TotalAgentsActif { get; set; }
        public int TotalReservConfirm { get; set; }

        public virtual IList<Agent> Agents { get; set; }
        public virtual IList<Reservation> Reservations { get; set; }
    }
}
