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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Identity.Data.Entities;
using Students.Data.Abstractions;
using Students.Data.Entities;
using System.Linq;
using Assignments.ViewModels;

namespace Assignments.Controllers
{
    public class AssignmentsController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignmentsController(IStorage storage, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            _userManager = userManager;
        }

        // GET: AssignmentsController
        [Authorize(Roles = "Student, Teacher")]
        public ActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            if (User.IsInRole("Student"))
            {
                AssignmentsViewModel viewModel = new AssignmentsViewModel();
                viewModel.coursesAssignments = new List<Tuple<Assignment, Course>>();
                viewModel.laboratoriesAssignments = new List<Tuple<Assignment, Laboratory>>();
                Student student = this.storage.GetRepository<IStudentRepository>().FindByUserId(Guid.Parse(userId));

                // Get all course assignments for the current student
                IEnumerable<CourseGroup> courseGroups = this.storage.GetRepository<ICourseGroupRepository>().GetByGroupId(student.GroupId);
                List<Course> courses = new List<Course>();
                foreach (CourseGroup courseGroup in courseGroups)
                {
                    Course course = this.storage.GetRepository<ICourseRepository>().FindById(courseGroup.CourseId);
                    courses.Add(course);
                }
                foreach(Course course in courses)
                {
                    Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindBySubjectId(course.Id);
                    if(assignment != null)
                    {
                        viewModel.coursesAssignments.Add(new Tuple<Assignment, Course>(assignment, course));
                    }                  
                }

                // Get all laboratories assignments for the current student
                List<Laboratory> labs = this.storage.GetRepository<ILaboratoryRepository>().AllBySubgroupId(student.SubgroupId).ToList();
                foreach (Laboratory lab in labs)
                {
                    Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindBySubjectId(lab.Id);
                    if(assignment != null)
                    {
                        viewModel.laboratoriesAssignments.Add(new Tuple<Assignment, Laboratory>(assignment, lab));
                    }            
                }

                //Tuple<List<Course>, List<Laboratory>> model = new Tuple<List<Course>, List<Laboratory>>(courses, labs);
                return View(viewModel);
            }



            

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
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(Guid id)
        {
            Assignment assignment = new Assignment();
            assignment.SubjectId = id;
            return View(assignment);
        }

        // POST: AssignmentsController/Create
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(Guid id)
        {
            Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindById(id);
            return View(assignment);
        }

        // POST: AssignmentsController/Edit/5
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
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
