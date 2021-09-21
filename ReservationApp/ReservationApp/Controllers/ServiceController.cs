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
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        public ServiceController(AppDbContext context)
        {
            _context = context;
        }




        // GET: ServiceController
        public ActionResult Index()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        // GET: ServiceController/Details/5
        public ActionResult Details(int id)
        {
            var service = _context.Services.Find(id);

            return View(service);
        }

        // GET: Service/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ReservationTypeController/Create
        [HttpPost]
        public ActionResult Create(Service model)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);

        }

        // GET: ServiceController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id is null)
            {
                return View("../Error/NotFound", "Please add the service Id in URL");
            }
            var type = _context.Services.Find(id);
            if (type is null)
            {
                return View("../Error/NotFound", $"The service Id : {id} cannot be found");
            }


            return View(type);
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Service service)
        {
            if (ModelState.IsValid)
            {

                _context.Entry(service).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View(service);
        }

        // GET: ServiceController/Delete/5
        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }

        // POST: ServiceController/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            var service = _context.Services.Find(id);
            if (service is null)
            {
                return View("../Error/NotFound", $"The service Id : {id} cannot be found");
            }

            _context.Remove(service);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
