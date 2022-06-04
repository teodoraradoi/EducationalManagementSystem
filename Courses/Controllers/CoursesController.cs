using Courses.Data.Abstractions;
using Courses.Data.Entities;
using ExtCore.Data.Abstractions;
using Identity.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Courses.Controllers
{
    public class CoursesController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;
        public CoursesController(IStorage storage, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            _userManager = userManager;
        }

        // GET: CoursesController
        [Authorize]
        public ActionResult Index()
        {
            var user = _userManager.GetUserId(User);
            if (User.IsInRole("Student"))
            {
                return View(this.storage.GetRepository<ICourseRepository>().All());
            }
            if (User.IsInRole("Teacher"))
            {
                return View(this.storage.GetRepository<ICourseRepository>().All());
            }
            return View(this.storage.GetRepository<ICourseRepository>().All());
         
        }

        // GET: CoursesController/Details/5
        [Authorize(Roles = "Teacher, Student")]
        public ActionResult Details(Guid id)
        {
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            return View(course);
        }

        [Authorize(Roles = "Secretary")]
        // GET: CoursesController/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: CoursesController/Create
        [Authorize(Roles = "Secretary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                this.storage.GetRepository<ICourseRepository>().Create(course);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        // GET: CoursesController/Edit/5
        [Authorize(Roles = "Secretary")]
        public ActionResult Edit(Guid? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
           
            return View(course);
        }

        // POST: CoursesController/Edit/5
        [Authorize(Roles = "Secretary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid Id, [Bind("Id,Name,Year,Semester,Day,Time")] Course course)
        {
            /*if (id != course.Id)
            {
                return NotFound();
            }*/
            if(ModelState.IsValid)
            {
                this.storage.GetRepository<ICourseRepository>().Edit(course);
                this.storage.Save();
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: CoursesController/Delete/5
        [Authorize(Roles = "Secretary")]
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: CoursesController/Delete/5
        [Authorize(Roles = "Secretary")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            this.storage.GetRepository<ICourseRepository>().Delete(id);
            this.storage.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
