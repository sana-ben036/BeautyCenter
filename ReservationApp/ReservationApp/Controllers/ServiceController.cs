using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using ReservationApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ServiceController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }



        [AllowAnonymous]
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

        // GET: ServiceController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: ServiceController/Create
        [HttpPost]
        public ActionResult Create(ServiceCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid()+"_"+model.Image.FileName;
                    string path = Path.Combine(uploadsFolder, uniqueFileName); //wwwroot/images/5656788889999nails.jpg
                    model.Image.CopyTo(new FileStream(path, FileMode.Create));
                }

                Service service = new Service()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Details = model.Details,
                    ImagePath = uniqueFileName

                };
                _context.Services.Add(service);
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
            var service = _context.Services.Find(id);
            if (service is null)
            {
                return View("../Error/NotFound", $"The service Id : {id} cannot be found");
            }

            ServiceEditViewModel model = new ServiceEditViewModel()
            {
                Id = service.Id,
                Name = service.Name,
                Price = service.Price,
                Details = service.Details,
                ImagePath = service.ImagePath
            };


            return View(model);
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
