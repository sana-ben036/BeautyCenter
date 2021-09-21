using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AgentController : Controller
    {
        private readonly AppDbContext _context;

        public AgentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AgentController
        public ActionResult Index()
        {
            var agents = _context.Agents.Include(a => a.Service).ToList().OrderBy(a => a.Service);
            return View(agents);
        }

        // GET: AgentController/Details/5
        public ActionResult Details(int id)
        {
            var agent = _context.Agents.Find(id);

            return View(agent);
        }

        // GET: AgentController/Create
        public IActionResult Create()
        {
            var agent = new Agent();
            IEnumerable<Service> services = _context.Services.ToList(); // pour charger les services et les afficher sur input select
            ViewBag.Services = services;

            return View(agent);
        }

        // POST: AgentController/Create

        [HttpPost]
        public IActionResult Create(Agent agent)
        {
            if (ModelState.IsValid)
            {
                _context.Agents.Add(agent);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Service> services = _context.Services.ToList();
            ViewBag.Services = services;
            return View(agent);
        }

        // GET: AgentController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id is null)
            {
                return View("../Error/NotFound", "Please add the agent Id in URL");
            }
            var agent = _context.Agents.Find(id);
            if (agent is null)
            {
                return View("../Error/NotFound", $"The agent Id : {id} cannot be found");
            }
            IEnumerable<Service> services = _context.Services.ToList();
            ViewBag.Services = services;
            return View(agent);
        }


        // POST: AgentController/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Agent agent)
        {

            
            var idService = agent.ServiceId;
            var service = _context.Services.Find(idService);


            if (ModelState.IsValid)
            {

                _context.Entry(agent).State = EntityState.Modified;


                _context.SaveChanges();

                return RedirectToAction(nameof(Index));

            }




            IEnumerable<Service> services = _context.Services.ToList();
            ViewBag.Services = services;
            return View(agent);

        }
    }
}
