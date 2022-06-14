using Assignments.Data.Abstractions;
using Assignments.Data.Entities;
using Courses.Data.Abstractions;
using Courses.Data.Entities;
using ExtCore.Data.Abstractions;
using Identity.Data.Entities;
using Laboratories.Data.Abstractions;
using Laboratories.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Students.Data.Abstractions;
using Students.Data.Entities;
using Submissions.Data.Abstractions;
using Submissions.Data.Entities;
using Submissions.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Teachers.Data.Abstractions;
using Teachers.Data.Entities;

namespace Submissions.Controllers
{
    public class SubmissionsController : Controller
    {
        private IStorage storage;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubmissionsController(IStorage storage, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            webHostEnvironment = hostEnvironment;
            _userManager = userManager;
        }

        [Authorize(Roles = "Teacher, Student")]
        // GET: SubmissionsController
        public ActionResult Index()
        {
            var userId = Guid.Parse(_userManager.GetUserId(User));
            if(User.IsInRole("Teacher"))
            {
                IndexViewModel model = new IndexViewModel();
                model.courseSubmissions = new List<Tuple<Submission, Assignment, Course, Student>>();
                model.laboratorySubmissions = new List<Tuple<Submission, Assignment, Laboratory, Student>>();

                Teacher teacher = this.storage.GetRepository<ITeachersRepository>().FindTeacherByUserId(userId);
                List<Course> courses = this.storage.GetRepository<ICourseRepository>().GetAllByTeacherId(teacher.Id).ToList();
                foreach(Course course in courses)
                {
                    List<Assignment> courseAssignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(course.Id).ToList();
                    foreach(Assignment assignment in courseAssignments)
                    {
                        List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id);
                        foreach(Submission submission in submissions)
                        {
                            Student student = this.storage.GetRepository<IStudentRepository>().FindById(submission.StudentId);
                            model.courseSubmissions.Add(new Tuple<Submission, Assignment, Course, Student>(submission, assignment, course, student));
                        }
                    }
                }

                List<Laboratory> laboratories = this.storage.GetRepository<ILaboratoryRepository>().AllByTeacherId(teacher.Id).ToList();
                foreach (Laboratory laboratory in laboratories)
                {
                    List<Assignment> laboratoryAssignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(laboratory.Id).ToList();
                    foreach (Assignment assignment in laboratoryAssignments)
                    {
                        List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id);
                        foreach (Submission submission in submissions)
                        {
                            Student student = this.storage.GetRepository<IStudentRepository>().FindById(submission.StudentId);
                            model.laboratorySubmissions.Add(new Tuple<Submission, Assignment, Laboratory, Student>(submission, assignment, laboratory, student));
                        }
                    }
                }

                return View(model);
            }

            return RedirectToAction("Index");




            /*
                IEnumerable submissions = this.storage.GetRepository<ISubmissionRepository>().All();
            List<Tuple<Submission, Assignment>> list = new List<Tuple<Submission, Assignment>>();

            foreach (Submission submission in submissions)
            {
                Assignment assignment = this.storage.GetRepository<IAssignmentRepository>().FindById(submission.AssignmentId);
                list.Add(new Tuple<Submission, Assignment>(submission, assignment));
            }
            return View(list);*/
        }

        // GET: SubmissionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubmissionsController/Create
        [Authorize(Roles = "Student")]
        public ActionResult Create(Guid id)
        {
            SubmissionViewModel submission = new SubmissionViewModel();
            submission.AssignmentId = id;
            return View(submission);
        }

        // POST: SubmissionsController/Create
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] SubmissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                Student currentStudent = this.storage.GetRepository<IStudentRepository>().FindByUserId(Guid.Parse(userId));
                string uniqueFileName = UploadedFile(model);
                Submission submission = new Submission
                {
                    FilePath = uniqueFileName,
                    AssignmentId = model.AssignmentId,
                    TurnedInDate = DateTime.Now,
                    StudentId = currentStudent.Id
                };


                this.storage.GetRepository<ISubmissionRepository>().Create(submission);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        private string UploadedFile(SubmissionViewModel model)
        {
            string uniqueFileName = null;
            if (model.FilePath != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "files");
                uniqueFileName = model.FilePath.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FilePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: SubmissionsController/Edit/5
        public ActionResult Edit(Guid id)
        {
            SubmissionViewModel submission = new SubmissionViewModel();
            submission.AssignmentId = id;
            return View(submission);
        }

        // POST: SubmissionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromForm] SubmissionViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Submission submission = new Submission
                {
                    FilePath = uniqueFileName,
                    AssignmentId = model.AssignmentId,
                    TurnedInDate = DateTime.Now
                };

                this.storage.GetRepository<ISubmissionRepository>().Edit(submission);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        // GET: SubmissionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubmissionsController/Delete/5
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
