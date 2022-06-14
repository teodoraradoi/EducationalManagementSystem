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
using Groups.Data.Entities;
using Groups.Data.Abstractions;
using Teachers.Data.Entities;
using Teachers.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Identity.Data.Entities;
using Students.Data.Abstractions;
using Students.Data.Entities;
using Posts.Data.Abstractions;
using Assignments.Data.Entities;
using Submissions.Data.Entities;
using Assignments.Data.Abstractions;
using Submissions.Data.Abstractions;

namespace Laboratories.Controllers
{
    public class LaboratoriesController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;

        public LaboratoriesController(IStorage storage, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            _userManager = userManager;
        }

        [Authorize(Roles = "Secretary, Teacher, Student")]
        // GET: LaboratoriesController
        public ActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            if (User.IsInRole("Secretary"))
            {
                IEnumerable labs = this.storage.GetRepository<ILaboratoryRepository>().All();
                List<Tuple<Laboratory, Course>> list = new List<Tuple<Laboratory, Course>>();

                foreach (Laboratory lab in labs)
                {
                    Guid courseId = lab.CourseId;
                    Course course = this.storage.GetRepository<ICourseRepository>().FindById(courseId);
                    list.Add(new Tuple<Laboratory, Course>(lab, course));
                }
                return View(list);
            }
            if (User.IsInRole("Teacher"))
            {
                Guid id = new Guid(userId);
                Teacher currentTeacher = this.storage.GetRepository<ITeachersRepository>().FindTeacherByUserId(id);
                IEnumerable<Laboratory> labs = this.storage.GetRepository<ILaboratoryRepository>().AllByTeacherId(currentTeacher.Id);
                List<Tuple<Laboratory, Course>> list = new List<Tuple<Laboratory, Course>>();

                foreach (Laboratory lab in labs)
                {
                    Guid courseId = lab.CourseId;
                    Course course = this.storage.GetRepository<ICourseRepository>().FindById(courseId);
                    list.Add(new Tuple<Laboratory, Course>(lab, course));
                }
                return View(list);
            }
            if (User.IsInRole("Student"))
            {
                Student student = this.storage.GetRepository<IStudentRepository>().FindByUserId(Guid.Parse(userId));
                IEnumerable<Laboratory> labs = this.storage.GetRepository<ILaboratoryRepository>().AllBySubgroupId(student.SubgroupId);

                List<Tuple<Laboratory, Course>> list = new List<Tuple<Laboratory, Course>>();
                foreach (Laboratory lab in labs)
                {
                    Course course = this.storage.GetRepository<ICourseRepository>().FindById(lab.CourseId);
                    list.Add(new Tuple<Laboratory, Course>(lab, course));
                }
                return View(list);


                /*IEnumerable<CourseGroup> courseGroups = this.storage.GetRepository<ICourseGroupRepository>().GetByGroupId(student.GroupId);

                List<Course> courses = new List<Course>();
                foreach (CourseGroup courseGroup in courseGroups)
                {
                    Course course = this.storage.GetRepository<ICourseRepository>().FindById(courseGroup.CourseId);
                    courses.Add(course);
                }

                 IEnumerable<Course> studentCourses = courses;
                List<Laboratory> labs = new List<Laboratory>();
                foreach (Laboratory lab in labs)
                {
                    Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById()
                }*/

            }




                return View(this.storage.GetRepository<ILaboratoryRepository>().All());
        }

        [Authorize(Roles = "Student")]
        public ActionResult DetailsForStudent(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            Student currentStudent = this.storage.GetRepository<IStudentRepository>().FindByUserId(Guid.Parse(userId));
            Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(id);
            DetailsForStudentViewModel model = new DetailsForStudentViewModel()
            {
                laboratory = laboratory,
                //course = this.storage.GetRepository<ICourseRepository>().FindById
                posts = this.storage.GetRepository<IPostRepository>().AllBySubjectId(laboratory.Id).ToList(),
            };


            List<Tuple<Assignment, Submission>> Done = new List<Tuple<Assignment, Submission>>();
            List<Assignment> assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(laboratory.Id).ToList();
            List<Assignment> ToDo = assignments;
            foreach (Assignment assignment in assignments.ToList())
            {
                List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id);
                foreach (Submission submission in submissions)
                {
                    if (submission.StudentId == currentStudent.Id)
                    {
                        Tuple<Assignment, Submission> tuple = new Tuple<Assignment, Submission>(assignment, submission);
                        Done.Add(tuple);
                        Assignment alreadyDone = assignment;
                        ToDo.Remove(alreadyDone); ; 
                    }
                }
            }

            model.ToDo = ToDo;
            model.Done = Done;

            //Teacher teacher = this.storage.GetRepository<ITeachersRepository>().FindById(course.TeacherId);
            //model.teacher = teacher;
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult DetailsForTeacher(Guid id)
        {
            Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(id);
            DetailsForTeacherViewModel model = new DetailsForTeacherViewModel()
            {
                laboratory = laboratory,
                assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(laboratory.Id).ToList(),
                posts = this.storage.GetRepository<IPostRepository>().AllBySubjectId(laboratory.Id).ToList(),
            };

            Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(laboratory.SubgroupId);
            Group group = this.storage.GetRepository<IGroupRepository>().FindById(subgroup.GroupId);
            model.group = new Tuple<Group, Subgroup>(group, subgroup);
            
            return View(model);
        }

        public ActionResult AttachedLaboratories(Guid id)
        {
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            ViewBag.CourseName = course.Name;
      
            List<Tuple<Laboratory, Subgroup>> list = new List<Tuple<Laboratory, Subgroup>>();
            IEnumerable<Laboratory> labs = this.storage.GetRepository<ILaboratoryRepository>().GetAllByCourseId(id);

            foreach (Laboratory lab in labs)
            {
                Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(lab.SubgroupId);
                list.Add(new Tuple<Laboratory, Subgroup>(lab, subgroup));
            }

            return View(list);
        }


        /*public List<Course> GetCoursesList()
        {
            List<Course> list = new List<Course>();
            IEnumerable<Course> courses = this.storage.GetRepository<ICourseRepository>().All();
            list = courses.ToList();
            return list;
        }*/

        public List<Subgroup> GetSubgroupList(Guid id)
        {
            List<Subgroup> list = new List<Subgroup>();
            //IEnumerable<Subgroup> subgroups = this.storage.GetRepository<ISubgroupRepository>().All();
            IEnumerable<Subgroup> subgroups = this.storage.GetRepository<ISubgroupRepository>().AllByGroupId(id);
            list = subgroups.ToList();
            return list;
        }
        public List<Teacher> GetTeachersList()
        {
            List<Teacher> list = new List<Teacher>();
            IEnumerable<Teacher> teachers = this.storage.GetRepository<ITeachersRepository>().All();
            list = teachers.ToList();
            return list;
        }

        // GET: LaboratoriesController/Create
        public ActionResult Create(Guid id)
        {
            CreateViewModel createViewModel = new CreateViewModel();
            //createViewModel.Courses = this.GetCoursesList();
            createViewModel.Laboratory = new Laboratory();
            createViewModel.Laboratory.CourseId = id;

            CourseGroup courseGroup = this.storage.GetRepository<ICourseGroupRepository>().GetGroupByCourseId(id);
            createViewModel.Subgroups = this.GetSubgroupList(courseGroup.GroupId);
            createViewModel.Teachers = this.GetTeachersList();
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
        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateViewModel createViewModel = new CreateViewModel();
            //createViewModel.Courses = this.GetCoursesList();
            createViewModel.Laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(id);
            //createViewModel.Laboratory.CourseId = id;

            CourseGroup courseGroup = this.storage.GetRepository<ICourseGroupRepository>().GetGroupByCourseId(createViewModel.Laboratory.CourseId);
            createViewModel.Subgroups = this.GetSubgroupList(courseGroup.GroupId);
            createViewModel.Teachers = this.GetTeachersList();
            return View(createViewModel);




            
            /*Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(id);
            CreateViewModel createViewModel = new CreateViewModel();
            //createViewModel.Courses = this.GetCoursesList();
            createViewModel.Laboratory = laboratory;

            return View(createViewModel);*/
        }

        // POST: LaboratoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid Id, [Bind("Id,Name,Day,Time,CourseId,SubgroupId,TeacherId")] Laboratory laboratory)
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
