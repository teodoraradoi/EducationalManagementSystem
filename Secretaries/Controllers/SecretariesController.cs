using ExtCore.Data.Abstractions;
using Groups.Data.Abstractions;
using Groups.Data.Entities;
using Identity.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Secretaries.Data.Abstractions;
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
        public ActionResult Index()
        {
             return View(this.storage.GetRepository<ISecretaryRepository>().All());
        }

        public ActionResult Create()
        {
            return View();
        }

        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([FromForm] Secretary model)
         {
         }*/
    }
}
