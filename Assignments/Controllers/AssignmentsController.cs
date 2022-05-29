using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExtCore.Data.Abstractions;
using Assignments.Data.Abstractions;
using Assignments.Data.Entities;
using System;
using Courses.Data.Entities;
using Laboratories.Data.Entities;
using System.Collections.Generic;
using Courses.Data.Abstractions;
using Laboratories.Data.Abstractions;

namespace Assignments.Controllers
{
    public class AssignmentsController : Controller
    {
        private IStorage storage;
        public AssignmentsController(IStorage storage)
        {
            this.storage = storage;
        }

        // GET: AssignmentsController
        public ActionResult Index()
        {
            IEnumerable<Assignment> assignments = this.storage.GetRepository<IAssignmentRepository>().All();
            //Tuple<Assignment, Course> courseTuple;
            //Tuple<Assignment, Laboratory> laboratoryTuple;
            //List list = List();
            List<Tuple<Assignment, Course, Laboratory>> list = new List<Tuple<Assignment, Course, Laboratory>>();
            foreach (Assignment assignment in assignments)
            {
                Course course = this.storage.GetRepository<ICourseRepository>().FindById(assignment.SubjectId);
                if(course != null)
                {
                    list.Add(new Tuple<Assignment, Course, Laboratory>(assignment, course, new Laboratory()));
                }
                else
                {
                    Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(assignment.SubjectId);
                    if (laboratory != null)
                    {
                        list.Add(new Tuple<Assignment, Course, Laboratory>(assignment, new Course(), laboratory));
                    }
                }
            }
            return View(list);
        }

        // GET: AssignmentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssignmentsController/Create
        public ActionResult Create(Guid id)
        {
            Assignment assignment = new Assignment();
            assignment.SubjectId = id;
            return View(assignment);
        }

        // POST: AssignmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                this.storage.GetRepository<IAssignmentRepository>().Create(assignment);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        // GET: AssignmentsController/Edit/5
        public ActionResult Edit(Guid id)
        {
            Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindById(id);
            return View(assignment);
        }

        // POST: AssignmentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("Id,Name,Description,SubjectId,DueDate,MaxGrade")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                assignment.Id = id;
                this.storage.GetRepository<IAssignmentRepository>().Edit(assignment);
                this.storage.Save();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: AssignmentsController/Delete/5
        public ActionResult Delete(Guid id)
        {
            Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindById(id);
            if (assignment == null)
            {
                return NotFound();
            }

            Course course = this.storage.GetRepository<ICourseRepository>().FindById(assignment.SubjectId);
            if (course != null)
            {
                ViewBag.SubjectName = course.Name;
            }
            else
            {
                Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(assignment.SubjectId);
                if (laboratory != null)
                {
                    ViewBag.SubjectName = laboratory.Name;
                }
            }
            return View(assignment);
        }

        // POST: AssignmentsController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            this.storage.GetRepository<IAssignmentRepository>().Delete(id);
            this.storage.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
