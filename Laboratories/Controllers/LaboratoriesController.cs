using ExtCore.Data.Abstractions;
using Laboratories.Data.Abstractions;
using Laboratories.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Laboratories.ViewModels;
using System.Linq;
using Courses.Data.Entities;
using Courses.Data.Abstractions;
using System;
using System.Collections;

namespace Laboratories.Controllers
{
    public class LaboratoriesController : Controller
    {
        private IStorage storage;
        public LaboratoriesController(IStorage storage)
        {
            this.storage = storage;
        }

        // GET: LaboratoriesController
        public ActionResult Index()
        {
            IEnumerable labs = this.storage.GetRepository<ILaboratoryRepository>().All();
            List<Tuple<Laboratory, Course>> list = new List<Tuple<Laboratory, Course>>();

            foreach(Laboratory lab in labs)
            {
                Course course = this.storage.GetRepository<ICourseRepository>().FindById(lab.CourseId);
                list.Add(new Tuple<Laboratory, Course>(lab, course));
            }
            return View(list);
            // return View(this.storage.GetRepository<ILaboratoryRepository>().All());
        }

        // GET: LaboratoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public List<Course> GetCreateList()
        {
            List<Course> list = new List<Course>();
            IEnumerable<Course> courses = this.storage.GetRepository<ICourseRepository>().All();
            list = courses.ToList();
            return list;
        }

        // GET: LaboratoriesController/Create
        public ActionResult Create()
        {
            CreateViewModel createViewModel = new CreateViewModel();
            createViewModel.Courses = this.GetCreateList();
            return View(createViewModel);
        }

        // POST: LaboratoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Laboratory laboratory)
        {

            if (ModelState.IsValid)
            {
                //laboratory.Id = Guid.NewGuid();
                this.storage.GetRepository<ILaboratoryRepository>().Create(laboratory);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        // GET: LaboratoriesController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(id);
            CreateViewModel createViewModel = new CreateViewModel();
            createViewModel.Courses = this.GetCreateList();
            createViewModel.Laboratory = laboratory;

            return View(createViewModel);
        }

        // POST: LaboratoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid Id, [Bind("Id,Name,Day,Time,CourseId")] Laboratory laboratory)
        {
            if (ModelState.IsValid)
            {
                laboratory.Id = Id;
                this.storage.GetRepository<ILaboratoryRepository>().Edit(laboratory);
                this.storage.Save();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: LaboratoriesController/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(id);
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(laboratory.CourseId);
            ViewBag.CourseName = course.Name;
            if (laboratory == null)
            {
                return NotFound();
            }
            return View(laboratory);
        }

        // POST: LaboratoriesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            this.storage.GetRepository<ILaboratoryRepository>().Delete(id);
            this.storage.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
