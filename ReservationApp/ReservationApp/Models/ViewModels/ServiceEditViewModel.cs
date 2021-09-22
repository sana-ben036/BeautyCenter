using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Models.ViewModels
{
    public class ServiceEditViewModel : ServiceCreateViewModel
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
    }
}
