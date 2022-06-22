using Assignments.Data.Abstractions;
using Assignments.Data.Entities;
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
using Posts.Data.Abstractions;
using Students.Data.Abstractions;
using Students.Data.Entities;
using Submissions.Data.Abstractions;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        [Authorize(Roles = "Teacher")]
        public ActionResult DetailsForTeacher(Guid id)
        {
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            DetailsForTeacherViewModel model = new DetailsForTeacherViewModel()
            {
                course = course,
                assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(course.Id).ToList(),
                posts = this.storage.GetRepository<IPostRepository>().AllBySubjectId(course.Id).ToList(),
                laboratories = this.storage.GetRepository<ILaboratoryRepository>().GetAllByCourseId(course.Id).ToList()
            };
            List<CourseGroup> list = this.storage.GetRepository<ICourseGroupRepository>().GetByGroupId(course.Id).ToList();
            foreach (CourseGroup item in list)
            {
                Group group = this.storage.GetRepository<IGroupRepository>().FindById(item.GroupId);
                model.groups.Add(group);
            }
            if (model.course.GradingMethod != null)
            {
                string[] gradingMethod = model.course.GradingMethod.ToString().Split("-");
                ViewBag.GradingMethod = gradingMethod[0] + "% Final Exam Grade + " + gradingMethod[1] + "% Laboratory Grade";
            }
            return View(model);
        }

        [Authorize(Roles = "Student")]
        public ActionResult DetailsForStudent(Guid id)
        {
            var userId = _userManager.GetUserId(User);
            Student currentStudent = this.storage.GetRepository<IStudentRepository>().FindByUserId(Guid.Parse(userId));
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            DetailsForStudentViewModel model = new DetailsForStudentViewModel()
            {
                course = course,
                //assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(course.Id).ToList(),
                posts = this.storage.GetRepository<IPostRepository>().AllBySubjectId(course.Id).ToList(),
            };


            List<Tuple<Assignment, Submission>> Done = new List<Tuple<Assignment, Submission>>();
            List<Assignment> assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(course.Id).ToList(); // get all assigments for this course
            List<Assignment> ToDo = assignments;
            foreach (Assignment assignment in assignments.ToList())
            {
                List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id); // get all submissions for each assigment
                foreach (Submission submission in submissions)
                {
                    if (submission.StudentId == currentStudent.Id) // if the student already submitted their work for this assignment
                    {
                        Tuple<Assignment, Submission> tuple = new Tuple<Assignment, Submission>(assignment, submission);
                        Done.Add(tuple);
                        Assignment alreadyDone = assignment;
                        ToDo.Remove(alreadyDone); ;
                        //break; // ?? 
                    }
                    /*else
                    {
                        ToDo.Add(assignment);
                       // break; // ?? 
                    }*/
                }
            }



            model.ToDo = ToDo;
            model.Done = Done;



            /*List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().All().ToList();
            foreach(Submission submission in submissions)
            {
                foreach(Assignment assignment in assignments)
                {
                    if(submission.AssignmentId == assignment.Id)
                    {
                        assignments.Remove(assignment);
                    }
                }   
            }*/


            // Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(currentStudent.SubgroupId);
            List<Laboratory> laboratories = this.storage.GetRepository<ILaboratoryRepository>().GetAllByCourseId(id).ToList();
            Laboratory studentLab = new Laboratory();
            foreach (Laboratory lab in laboratories)
            {
                //studentLab = this.storage.GetRepository<ILaboratoryRepository>().AllBySubgroupId(currentStudent.SubgroupId).FirstOrDefault();
                if (lab.SubgroupId == currentStudent.SubgroupId)
                {
                    studentLab = lab;
                    break;
                }
            }
            model.laboratory = studentLab;
            Teacher teacher = this.storage.GetRepository<ITeachersRepository>().FindById(course.TeacherId);
            model.teacher = teacher;
            if (model.course.GradingMethod != null)
            {
                string[] gradingMethod = model.course.GradingMethod.ToString().Split("-");
                ViewBag.GradingMethod = gradingMethod[0] + "% Final Exam Grade + " + gradingMethod[1] + "% Laboratory Grade";
            }
            return View(model);
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

        [Authorize(Roles = "Teacher")]
        public ActionResult CreateGradingMethod(Guid id)
        {
            GradingMethodViewModel viewModel = new GradingMethodViewModel();
            viewModel.id = id;
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            ViewBag.CourseName = course.Name;
            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGradingMethod(Guid id, GradingMethodViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
                course.GradingMethod = viewModel.FinalExamPercent + "-" + viewModel.LaboratoryPercent;
                //course.GradingMethod = viewModel.FinalExamPercent + "% Final Exam Grade + " + viewModel.LaboratoryPercent + "% Laboratory Grade";
                course.AttendanceMatters = viewModel.AttendanceMatters;
                this.storage.GetRepository<ICourseRepository>().Edit(course);
                this.storage.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult EditGradingMethod(Guid id)
        {
            Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            GradingMethodViewModel viewModel = new GradingMethodViewModel();
            string[] gradingMethod = course.GradingMethod.ToString().Split("-");
            viewModel.FinalExamPercent = Convert.ToInt32(gradingMethod[0]);
            viewModel.LaboratoryPercent = Convert.ToInt32(gradingMethod[1]);
            viewModel.AttendanceMatters = course.AttendanceMatters;
            viewModel.id = id;
            ViewBag.CourseName = course.Name;
            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGradingMethod(Guid id, GradingMethodViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Course course = this.storage.GetRepository<ICourseRepository>().FindById(id);
                course.GradingMethod = viewModel.FinalExamPercent + "-" + viewModel.LaboratoryPercent;
                course.AttendanceMatters = viewModel.AttendanceMatters;
                this.storage.GetRepository<ICourseRepository>().Edit(course);
                this.storage.Save();
                return RedirectToAction(nameof(Index));
            }
            return View();
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
            if (id == null)
            {
                return NotFound();
            }
            CourseViewModel viewModel = new CourseViewModel();
            viewModel.course = this.storage.GetRepository<ICourseRepository>().FindById(id);
            viewModel.groups = this.getGroups();
            viewModel.Teachers = this.GetTeachersList();

            return View(viewModel);
        }

        // POST: CoursesController/Edit/5
        [Authorize(Roles = "Secretary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid Id, [Bind("Id,Name,Year,Semester,Day,Time,TeacherId")] Course course, [FromForm] IEnumerable<string> selectedGroups)
        {
            if (ModelState.IsValid)
            {


                course.Id = Id;
                this.storage.GetRepository<ICourseRepository>().Edit(course);
                this.storage.Save();

               /* List<CourseGroup> existing = this.storage.GetRepository<ICourseGroupRepository>().AllByCourseId(course.Id);

                this.storage.GetRepository<ICourseGroupRepository>().Delete(existing.ToArray());
                this.storage.Save();*/
                /* foreach(CourseGroup item in existing)
                 {
                     this.storage.GetRepository<ICourseGroupRepository>().Delete(item);
                     this.storage.Save();
                 }*/



                List<CourseGroup> courseGroups = new List<CourseGroup>();
                foreach (string selectedId in selectedGroups)
                {   
                    CourseGroup courseGroup = new CourseGroup();
                    courseGroup.Id = Guid.NewGuid();
                    courseGroup.CourseId = course.Id;
                    courseGroup.GroupId = Guid.Parse(selectedId);

                    courseGroups.Add(courseGroup);

                }

                this.storage.GetRepository<ICourseGroupRepository>().Create(courseGroups);
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
            //this.storage.GetRepository<ICourseGroupRepository>().
            this.storage.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
