using Courses.Data.Abstractions;
using Courses.Data.Entities;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Courses.Controllers
{
    public class CoursesController : Controller
    {
        private IStorage storage;
        public CoursesController(IStorage storage)
        {
            this.storage = storage;
        }

        // GET: CoursesController
        public ActionResult Index()
        {
            return View(this.storage.GetRepository<ICourseRepository>().All());
        }

        // GET: CoursesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CoursesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoursesController/Create
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
