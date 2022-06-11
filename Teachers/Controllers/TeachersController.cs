using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExtCore.Data.Abstractions;
using Teachers.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Teachers.ViewModels;
using Identity.Data.Entities;
using Teachers.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Courses.Data.Entities;
using Laboratories.Data.Entities;

namespace Teachers.Controllers
{
    public class TeachersController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public TeachersController(IStorage storage, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.storage = storage;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: TeachersController
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Index()
        {/*
            //IEnumerable<ApplicationUser> teachers = 
            List<Tuple<ApplicationUser, Course, Laboratory>> list = new List<Tuple<ApplicationUser, Course, Laboratory>>();
            foreach (Teacher student in teachers)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(student.UserId.ToString());
                Group group = this.storage.GetRepository<IGroupRepository>().FindById(student.GroupId);
                Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(student.SubgroupId);
                list.Add(new Tuple<ApplicationUser, Group, Subgroup>(user, group, subgroup));
            }
            return View(list);*/
            return View(this.storage.GetRepository<ITeachersRepository>().All());
        }

        // GET: TeachersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TeachersController/Create
        [Authorize(Roles = "Secretary")]
        public ActionResult Create()
        {
            TeacherAccountViewModel viewModel = new TeacherAccountViewModel();
            return View(viewModel);
        }

        // POST: TeachersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TeacherAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    Id = new Guid(),
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    EmailConfirmed = true
                };
                var user = await _userManager.CreateAsync(newUser, viewModel.Password);
                if (user.Succeeded)
                {
                    var createdUser = await _userManager.FindByIdAsync(newUser.Id.ToString());
                    Teacher teacher = new Teacher
                    {
                        Id = Guid.NewGuid(),
                        Name = viewModel.Name,
                        UserId = createdUser.Id
                    };
                    this.storage.GetRepository<ITeachersRepository>().Create(teacher);
                    this.storage.Save();

                    var teacherRole = await _roleManager.FindByNameAsync("Teacher");
                    if (teacherRole != null)
                    {
                        await _userManager.AddToRoleAsync(createdUser, "Teacher");
                    }
                    return RedirectToAction("Index", "Default");
                }
            }
            return View();
        }

        // GET: TeachersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeachersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeachersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeachersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
