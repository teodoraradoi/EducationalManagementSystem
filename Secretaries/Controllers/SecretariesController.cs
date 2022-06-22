using ExtCore.Data.Abstractions;
using Groups.Data.Abstractions;
using Groups.Data.Entities;
using Identity.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Secretaries.Data.Abstractions;
using Secretaries.Data.Entities;
using Secretaries.ViewModels;
using Students.Data.Abstractions;
using Students.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaries.Controllers
{
    public class SecretariesController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public SecretariesController(IStorage storage, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.storage = storage;
            _userManager = userManager;
           _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Index()
        {
            List<IndexViewModel> list = new List<IndexViewModel>();
            List<Secretary> secretaries = this.storage.GetRepository<ISecretaryRepository>().All().ToList();
            foreach (Secretary secretary in secretaries)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(secretary.UserId.ToString());
                list.Add(new IndexViewModel()
                {
                    secretary = secretary,
                    user = user
                });
            }
             return View(list);
        }

        [Authorize(Roles = "Secretary")]
        public ActionResult Create()
        {
            SecretaryViewModel model = new SecretaryViewModel();
            return View(model);
        }

        [Authorize(Roles = "Secretary")]
        [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([FromForm] SecretaryViewModel model)
         {
            ApplicationUser newUser = new ApplicationUser
            {
                Id = new Guid(),
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };
            var user = await _userManager.CreateAsync(newUser, model.Password);
            if (user.Succeeded)
            {
                var createdUser = await _userManager.FindByIdAsync(newUser.Id.ToString());
                Secretary secretary = new Secretary
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    UserId = createdUser.Id
                };
                this.storage.GetRepository<ISecretaryRepository>().Create(secretary);
                this.storage.Save();

                var secretaryRole = await _roleManager.FindByNameAsync("Secretary");
                if (secretaryRole != null)
                {
                    await _userManager.AddToRoleAsync(createdUser, "Secretary");
                }
                return RedirectToAction("Index", "Default");
            }
            return View();
        }
    }
}
