using Microsoft.AspNetCore.Mvc;
using ReservationApp.Models;
using ReservationApp.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Controllers
{
    public class AgentController : Controller
    {
        private readonly ICenterRepository<Agent> _centerRepository;

        public AgentController(ICenterRepository<Agent> centerRepository)
        {
            _centerRepository = centerRepository;
           


        }
        public ActionResult Index()
        {
            IEnumerable<Agent> Agents = _centerRepository.GetList();


            return View(Agents);
        }
    }
}
