using ReservationApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReservationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly ILogger<RoleController> logger;
        public RoleController(RoleManager<IdentityRole> roleManager,
                              UserManager<AppUser> userManager,
                              ILogger<RoleController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.logger = logger;
        }

        // actions

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Role model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.RoleName
                };

                IdentityResult result = await this.roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Index()
        {
            var roles = this.roleManager.Roles;
            return View(roles);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id is null)
            {
                return View("../Error/NotFound", "Please add the role Id in URL");
            }
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return View("../Error/NotFound", $"The role Id : {id} cannot be found");
            }

            EditRoleViewModel model = new EditRoleViewModel()
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()  // pour recuperer ds cette list les users de ce role
            };

            foreach (AppUser user in await userManager.Users.ToListAsync()) // userManager.Users is not awaitble so change to (await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                if (role is null)
                {
                    return View("../Error/NotFound", $"The role Id : {model.Id} cannot be found");
                }
                role.Name = model.RoleName;

                IdentityResult result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);

        }



        [HttpGet]
        public IActionResult Delete()
        {
            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            IdentityRole role = await roleManager.FindByIdAsync(id);


            try
            {
                if (!(role is null))
                {
                    IdentityResult result = await roleManager.DeleteAsync(role);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    return View("../Error/NotFound", $"The role Id : {id} cannot be found");

                }
                return RedirectToAction("Index");

            }
            catch (DbUpdateException ex)
            {
                this.logger.LogError(ex.Message);
                ViewBag.Error = "Delete Role";
                string errorMessage = role.Name + " role is in use , so this role cannot be deleted as there users in this role." +
                    "if you want to delete this role, " +
                    "please remove the users from the role and then try to delete ";
                return View("Error", errorMessage);

            }






        }


        [HttpGet]
        public async Task<IActionResult> EditUsersRole(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return View("../Error/NotFound", $"The role must be exist and not empty in Url");

            }
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return View("../Error/NotFound", $"The role Id : {role.Id} cannot be found");
            }

            var Models = new List<EditUsersRoleViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                EditUsersRoleViewModel model = new EditUsersRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    //IsSelected = false
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.IsSelected = true;
                }
                else
                {
                    model.IsSelected = false;
                }

                Models.Add(model);
            }
            ViewBag.RoleName = role.Name;
            //ViewBag.RoleId = roleId;
            return View(Models);





        }



        [HttpPost]
        public async Task<IActionResult> EditUsersRole(List<EditUsersRoleViewModel> model, string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return View("../Error/NotFound", $"The role must be exist and not empty in Url");

            }
            var role = await this.roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return View("../Error/NotFound", $"The role Id : {role.Id} cannot be found");
            }

            // role if deja affectté et in model is select il faut le supprimer , ou l'affecté si il est selecté au model mais non affecté before

            IdentityResult result = null;

            for (int i = 0; i < model.Count; i++)
            {
                AppUser user = await userManager.FindByIdAsync(model[i].UserId);

                if (await userManager.IsInRoleAsync(user, role.Name) && !model[i].IsSelected)
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else if (!(await userManager.IsInRoleAsync(user, role.Name)) && model[i].IsSelected)
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
            }

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return RedirectToAction("Edit", new { id = roleId });

        }


    }
}
