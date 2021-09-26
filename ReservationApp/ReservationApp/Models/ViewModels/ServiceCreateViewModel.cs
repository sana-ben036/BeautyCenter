using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models.ViewModels
{
    public class ServiceCreateViewModel
    {
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "The service name field is required !")]
        [Display(Name = "Enter The Name")]
        public string Name { get; set; }

        public string Details { get; set; }

        public IFormFile Image { get; set; }

        public int Price { get; set; }
        public int TotalAgentsActif { get; set; }
    }
}
