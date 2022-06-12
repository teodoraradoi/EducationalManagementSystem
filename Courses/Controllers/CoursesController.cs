using Courses.Data.Abstractions;
using Courses.Data.Entities;
using Courses.ViewModels;
using ExtCore.Data.Abstractions;
using Groups.Data.Abstractions;
using Groups.Data.Entities;
using Identity.Data.Entities;
using Laboratories.Data.Abstractions;
using Laboratories.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Students.Data.Abstractions;
using Students.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Teachers.Data.Abstractions;
using Teachers.Data.Entities;

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
            var userId = _userManager.GetUserId(User);
            if (User.IsInRole("Student"))
            {
                Student student = this.storage.GetRepository<IStudentRepository>().FindByUserId(Guid.Parse(userId));
                IEnumerable<CourseGroup> courseGroups = this.storage.GetRepository<ICourseGroupRepository>().GetByGroupId(student.GroupId);

                List<Course> courses = new List<Course>();
                foreach (CourseGroup courseGroup in courseGroups)
                {
                    Course course = this.storage.GetRepository<ICourseRepository>().FindById(courseGroup.CourseId);
                    courses.Add(course);
                }

                IEnumerable<Course> studentCourses = courses;

                return View(studentCourses);
            }
            if (User.IsInRole("Teacher"))
            {
                /*Guid id = new Guid(userId);
                Teacher currentTeacher = this.storage.GetRepository<ITeachersRepository>().FindTeacherByUserId(id);
                IEnumerable<Course> courses = this.storage.GetRepository<ICourseRepository>().GetAllByTeacherId(currentTeacher.Id);
                List<Tuple<Course, Laboratory>> list = new List<Tuple<Course, Laboratory>>();

                foreach(Course course in courses)
                {
                    Laboratory laboratory = this.storage.GetRepository<ILaboratoryRepository>().FindById(course.Id);
                    list.Add(new Tuple<Course, Laboratory>(course, laboratory)) ;
                }
                return View(list);*/

                Guid id = new Guid(userId);
                Teacher currentTeacher = this.storage.GetRepository<ITeachersRepository>().FindTeacherByUserId(id);
                IEnumerable<Course> courses = this.storage.GetRepository<ICourseRepository>().GetAllByTeacherId(currentTeacher.Id);
                return View(courses);
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

        public IEnumerable<SelectListItem> getGroups()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            var allGroups = this.storage.GetRepository<IGroupRepository>().All();
            foreach (Group item in allGroups)
            {
                items.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return items;
        }


        [Authorize(Roles = "Secretary")]
        // GET: CoursesController/Create
        public ActionResult Create()
        {
            CourseViewModel viewModel = new CourseViewModel();
            viewModel.groups = this.getGroups();
            viewModel.Teachers = this.GetTeachersList();
            
            return View(viewModel);



            /*
            CourseViewModel viewModel = new CourseViewModel();
            viewModel.groups = new List<SelectListItem>();
            var allGroups = this.storage.GetRepository<IGroupRepository>().All();
            foreach(Group item in allGroups)
            {
                viewModel.groups.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            return View(viewModel);*/
        }


        // POST: CoursesController/Create
        [Authorize(Roles = "Secretary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] CourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Guid courseId = Guid.NewGuid();


                Course course = viewModel.course;
                course.Id = courseId;
                this.storage.GetRepository<ICourseRepository>().Create(course);
                this.storage.Save();

                //var user = _userManager.GetUserId(User);
                // add teacher dropdown to course

               List<CourseGroup> courseGroups = new List<CourseGroup>();

                foreach (string selectedId in viewModel.selectedGroups)
                {
                    /*CourseGroup courseGroup = new CourseGroup
                    {
                        Id = Guid.NewGuid(),
                        CourseId = courseId,
                        GroupId = Guid.Parse(selectedId)
                    };*/
                    CourseGroup courseGroup = new CourseGroup();
                    courseGroup.Id = Guid.NewGuid();
                    courseGroup.CourseId = courseId;
                    //courseGroup.CourseId = Guid.NewGuid();
                    courseGroup.GroupId = Guid.Parse(selectedId);

                    courseGroups.Add(courseGroup);

               
                }

                this.storage.GetRepository<ICourseGroupRepository>().Create(courseGroups);
                this.storage.Save();

                return RedirectToAction("Index", "Default");
            }

            return this.View();
        }

        public List<Teacher> GetTeachersList()
        {
            List<Teacher> list = new List<Teacher>();
            IEnumerable<Teacher> teachers = this.storage.GetRepository<ITeachersRepository>().All();
            list = teachers.ToList();
            return list;
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
