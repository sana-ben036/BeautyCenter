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
   // [Authorize(Roles = "Admin")]
    [AllowAnonymous]
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
            IEnumerable<Agent> agents = _context.Agents.Include(a => a.Service).ToList();
            return View(agents);
        }

        // GET: AgentController/Delete/5
        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }

        // POST: AgentController/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            
            
            var agent = _context.Agents.Find(id);
            
            if (agent is null)
            {
                return View("../Error/NotFound", $"The Agent Id : {id} cannot be found");
            }
            var idService = agent.ServiceId;
            var service = _context.Services.Find(idService);

            if(agent.IsActive == true && service.TotalAgentsActif>0)
            {
                service.TotalAgentsActif--;
                _context.SaveChanges();
            }

            _context.Remove(agent);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // GET: AgentController/Create
        public IActionResult Create()
        {
            var agent = new Agent();
            var services = _context.Services.ToList(); // pour charger les services et les afficher sur input select
            ViewBag.Services = services;

            return View(agent);
        }

        // POST: AgentController/Create
        
        [HttpPost]
        public IActionResult Create(Agent agent)
        {
            var idService = agent.ServiceId;
            var service = _context.Services.Find(idService);

            if (ModelState.IsValid)
            {
                _context.Agents.Add(agent);
                if (agent.IsActive == true)
                {
                    service.TotalAgentsActif++;
                    _context.SaveChanges();
                }
                
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
            var status = agent.IsActive;


            if (ModelState.IsValid)
            {

                

                if (agent.IsActive != status)
                {
                    if(agent.IsActive == true)
                    {
                        service.TotalAgentsActif++;
                        _context.Entry(agent).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        service.TotalAgentsActif--;
                        _context.Entry(agent).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
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
