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
using Submissions.Data.Entities;
using Submissions.Data.Abstractions;
using Teachers.Data.Entities;
using Teachers.Data.Abstractions;
using Groups.Data.Entities;
using Groups.Data.Abstractions;

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
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            AssignmentsViewModel model = new AssignmentsViewModel();
            model.coursesAssignments = new List<Tuple<Assignment, Course, int>>();
            model.laboratoriesAssignments = new List<Tuple<Assignment,Laboratory, int>>();

            var userId = _userManager.GetUserId(User);
            Teacher teacher = this.storage.GetRepository<ITeachersRepository>().FindTeacherByUserId(Guid.Parse(userId));
            List<Course> courses = this.storage.GetRepository<ICourseRepository>().GetAllByTeacherId(teacher.Id).ToList();

            foreach(Course course in courses)
            {
                List<Assignment> courseAssignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(course.Id).ToList();
                if(courseAssignments != null)
                {
                    foreach(Assignment assignment in courseAssignments)
                    {
                        List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id);
                        model.coursesAssignments.Add(new Tuple<Assignment, Course, int>(assignment, course, submissions.Count()));
                    }
                }
            }

            List<Laboratory> laboratories = this.storage.GetRepository<ILaboratoryRepository>().AllByTeacherId(teacher.Id).ToList();
            foreach (Laboratory laboratory in laboratories)
            {
                List<Assignment> labAssignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(laboratory.Id).ToList();
                if (labAssignments != null)
                {
                    foreach (Assignment assignment in labAssignments)
                    {
                        List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id);
                        model.laboratoriesAssignments.Add(new Tuple<Assignment, Laboratory, int>(assignment, laboratory, submissions.Count()));
                    }
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Student")]
        public ActionResult StudentIndex()
        {
            var userId = _userManager.GetUserId(User);
            Student student = this.storage.GetRepository<IStudentRepository>().FindByUserId(Guid.Parse(userId));
            StudentAssignmentsViewModel model = new StudentAssignmentsViewModel();
            model.courseAssignments = new List<Tuple<Assignment, Course, Submission>>();
            model.laboratoryAssignments = new List<Tuple<Assignment, Laboratory, Submission>>();

            List<CourseGroup> courseGroups = this.storage.GetRepository<ICourseGroupRepository>().GetByGroupId(student.GroupId).ToList();
            List<Course> courses = new List<Course>();
            foreach(CourseGroup item in courseGroups)
            {
                Course course = this.storage.GetRepository<ICourseRepository>().FindById(item.CourseId);
                courses.Add(course);
            }
           
            foreach(Course course in courses)
            {
                List<Assignment> assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(course.Id).ToList();
                foreach (Assignment assignment in assignments)
                {
                    Submission submission = this.storage.GetRepository<ISubmissionRepository>().FindByStudentAndAssignmentId(student.Id, assignment.Id);
                    if (submission != null)
                    {
                        model.courseAssignments.Add(new Tuple<Assignment, Course, Submission>(assignment, course, submission));
                    }
                    else
                    {
                        model.courseAssignments.Add(new Tuple<Assignment, Course, Submission>(assignment, course, new Submission()));
                    }
                }
            }

            List<Laboratory> laboratories = this.storage.GetRepository<ILaboratoryRepository>().AllBySubgroupId(student.SubgroupId).ToList();
            foreach (Laboratory laboratory in laboratories)
            {
                List<Assignment> assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(laboratory.Id).ToList();
                foreach (Assignment assignment in assignments)
                {
                    Submission submission = this.storage.GetRepository<ISubmissionRepository>().FindByStudentAndAssignmentId(student.Id, assignment.Id);
                    if (submission != null)
                    {
                        model.laboratoryAssignments.Add(new Tuple<Assignment, Laboratory, Submission>(assignment, laboratory, submission));
                    }
                    else
                    {
                        model.laboratoryAssignments.Add(new Tuple<Assignment, Laboratory, Submission>(assignment, laboratory, new Submission()));
                    }
                }
            }

            return View(model);
        }

        // GET: AssignmentsController/Details/5
        public ActionResult Details(Guid id)
        {
            Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindById(id);
            return View(assignment);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult DetailsForTeacher(Guid id)
        {
            DetailsForTeacherViewModel model = new DetailsForTeacherViewModel();
            model.submissionsList = new List<Tuple<Submission, Student>>();
            model.assignment = this.storage.GetRepository<IAssignmentRepository>().FindById(id);
            List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(model.assignment.Id);
            foreach(Submission submission in submissions)
            {
                Student student = this.storage.GetRepository<IStudentRepository>().FindById(submission.StudentId);
                model.submissionsList.Add(new Tuple<Submission, Student>(submission, student));
            }
            return View(model);
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
        public ActionResult Create(Guid id, Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                assignment.SubjectId = id;
                assignment.Id = Guid.NewGuid();
                if(assignment.Description == null)
                {
                    assignment.Description = "";
                }


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
