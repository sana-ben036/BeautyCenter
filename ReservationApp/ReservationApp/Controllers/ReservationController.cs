using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        public ReservationController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }




        // GET: ReservationController
        public async Task<ActionResult> Index()
        {


            if (signInManager.IsSignedIn(User) && !User.IsInRole("Admin"))
            {
                AppUser user = await userManager.FindByEmailAsync(User.Identity.Name);
                var reservations = _context.Reservations.Include(r => r.Service).ToList().Where(r => r.UserId == user.Id).OrderBy(r => r.Status); // include pour afficher le nom de type d'objet type
                return View(reservations);
            }
            else
            {
                var reservations = _context.Reservations.Include(r => r.Service).Include(r => r.User).ToList().OrderBy(r => r.Status);
                return View(reservations);
            }

        }

        // GET: ReservationController/Details/5
        public ActionResult Details(int id)
        {
            var reservation = _context.Reservations.Find(id);

            return View(reservation);
        }

        // GET: ReservationController/Create
        public IActionResult Create()
        {
            var reservation = new Reservation();
            ViewBag.UserId = userManager.GetUserId(HttpContext.User);  // recuperer id de user en connection pour remplir la valeur de UserId comme proprietaire de cette reservation crée
            IEnumerable<Service> services = _context.Services.ToList(); // pour charger les services et les afficher sur input select
            ViewBag.Services = services;

            return View(reservation);
        }

        // POST: ReservationController/Create

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            var idService = reservation.ServiceId;
            var service = _context.Services.Find(idService);

            if (ModelState.IsValid)
            {
                _context.Reservations.Add(reservation);

                if(service.TotalReservConfirm < service.TotalAgentsActif)
                {
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("../Error/NotFound", "There is no Agent avalaible ");
                }
            }

            ViewBag.UserId = userManager.GetUserId(HttpContext.User);
            IEnumerable<Service> services = _context.Services.ToList();
            ViewBag.Services = services;
            return View(reservation);
        }

        // GET: ReservationController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id is null)
            {
                return View("../Error/NotFound", "Please add the reservation Id in URL");
            }
            var reservation = _context.Reservations.Find(id);
            if (reservation is null)
            {
                return View("../Error/NotFound", $"The reservation Id : {id} cannot be found");
            }
            ViewBag.UserId = userManager.GetUserId(HttpContext.User);
            IEnumerable<Service> services = _context.Services.ToList();
            ViewBag.Services = services;
            return View(reservation);
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Reservation reservation)
        {

            var idUser = reservation.UserId;
            AppUser user = await userManager.FindByIdAsync(idUser);
            var idService = reservation.ServiceId;
            var service = _context.Services.Find(idService);


            if (ModelState.IsValid)
            {

                _context.Entry(reservation).State = EntityState.Modified;

                
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));

            }




            ViewBag.UserId = userManager.GetUserId(HttpContext.User);
            IEnumerable<Service> services = _context.Services.ToList();
            ViewBag.Services = services;
            return View(reservation);

        }

        // GET: ReservationController/Delete/5
        public ActionResult Delete()
        {
            return RedirectToAction("Index");
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation is null)
            {
                return View("../Error/NotFound", $"The reservartion Id : {id} cannot be found");
            }

            var idUser = reservation.UserId;
            AppUser user = await userManager.FindByIdAsync(idUser);
            var idService = reservation.ServiceId;
            var service = _context.Services.Find(idService);

           
            _context.Remove(reservation);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        




    }
}
