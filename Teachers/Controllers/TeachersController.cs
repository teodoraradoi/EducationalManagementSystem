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
using Courses.Data.Abstractions;
using System.Linq;
using Laboratories.Data.Abstractions;

namespace Teachers.Controllers
{
    public class TeachersController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private ITeachersRepository teachersRepo;  

        public TeachersController(IStorage storage, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.storage = storage;
            _userManager = userManager;
            _roleManager = roleManager;
            teachersRepo = this.storage.GetRepository<ITeachersRepository>();
        }

        // GET: TeachersController
        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = this.storage.GetRepository<ITeachersRepository>().All();
            List<Tuple<Teacher, ApplicationUser, int , int>> list = new List<Tuple<Teacher, ApplicationUser, int, int>>();
            foreach (Teacher teacher in teachers)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(teacher.UserId.ToString());
                List<Course> coursesTaught = this.storage.GetRepository<ICourseRepository>().GetAllByTeacherId(teacher.Id).ToList();
                List<Laboratory> laboratoriesTaught = this.storage.GetRepository<ILaboratoryRepository>().AllByTeacherId(teacher.Id).ToList();
                list.Add(new Tuple<Teacher, ApplicationUser, int, int>(teacher, user, coursesTaught.Count(), laboratoriesTaught.Count()));
            }
            return View(list);
        }

        // GET: TeachersController/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            DetailsViewModel viewModel = new DetailsViewModel();
            Teacher teacher = teachersRepo.FindById(id);
            viewModel.user = await _userManager.FindByIdAsync(teacher.UserId.ToString());
            viewModel.teacher = teacher;
            viewModel.courses = this.storage.GetRepository<ICourseRepository>().GetAllByTeacherId(teacher.Id).ToList();
            viewModel.laboratories = this.storage.GetRepository<ILaboratoryRepository>().AllByTeacherId(teacher.Id).ToList();
            return View(viewModel);
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
        public async Task<IActionResult> Edit(Guid id)
        {
            Teacher teacher = teachersRepo.FindById(id);
            ApplicationUser user = await _userManager.FindByIdAsync(teacher.UserId.ToString());
            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                teacher = teacher,
                user = user
            };
            //Tuple<Teacher, ApplicationUser> tuple = new Tuple<Teacher, ApplicationUser>(teacher, user);
            return View(detailsViewModel);
        }

        // POST: TeachersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = teachersRepo.FindById(id);
                teacher.Name = model.teacher.Name;
                this.storage.GetRepository<ITeachersRepository>().Update(teacher);
                this.storage.Save();

                ApplicationUser user = await _userManager.FindByIdAsync(teacher.UserId.ToString());
                user.Email = model.user.Email;
                user.UserName = model.user.Email;
                await _userManager.UpdateAsync(user);
                

                return RedirectToAction("Index", "Default");
            }
            return View();
        }

        // GET: TeachersController/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            Teacher teacher = teachersRepo.FindById(id);
            ApplicationUser user = await _userManager.FindByIdAsync(teacher.UserId.ToString());
            DetailsViewModel model = new DetailsViewModel()
            {
                teacher = teacher,
                user = user
            };
            return View(model);
        }

        // POST: TeachersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Teacher teacher = teachersRepo.FindById(id);
            ApplicationUser user = await _userManager.FindByIdAsync(teacher.UserId.ToString());
            teachersRepo.Delete(id);
            this.storage.Save();

            await _userManager.RemoveFromRoleAsync(user, "Teacher");
            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
