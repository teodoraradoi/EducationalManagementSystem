using Assignments.Data.Abstractions;
using Assignments.Data.Entities;
using Courses.Data.Abstractions;
using Courses.Data.Entities;
using ExtCore.Data.Abstractions;
using Groups.Data.Abstractions;
using Groups.Data.Entities;
using Identity.Data.Entities;
using Laboratories.Data.Abstractions;
using Laboratories.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Students.Data.Abstractions;
using Students.Data.Entities;
using Submissions.Data.Abstractions;
using Submissions.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Data.Abstractions;
using Teachers.Data.Entities;

namespace Barebone.Controllers
{
    public class DefaultController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;

        public DefaultController(IStorage storage, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            _userManager = userManager;
        }

        public ActionResult Index()
        {

            if(!User.Identity.IsAuthenticated)
            {
                return View("LoggedOut");
            }


            var userId = _userManager.GetUserId(User);
            if (User.IsInRole("Secretary"))
            {
                List<Student> students = this.storage.GetRepository<IStudentRepository>().All().ToList();
                ViewBag.NumberOfStudents = students.Count();

                List<Teacher> teachers = this.storage.GetRepository<ITeachersRepository>().All().ToList();
                ViewBag.NumberOfTeachers = teachers.Count();

                List<Course> courses = this.storage.GetRepository<ICourseRepository>().All().ToList();
                ViewBag.NumberOfCourses = courses.Count();

                List<Laboratory> laboratories = this.storage.GetRepository<ILaboratoryRepository>().All().ToList();
                ViewBag.NumberOfLaboratories = laboratories.Count();

                List<Group> groups = this.storage.GetRepository<IGroupRepository>().All().ToList();
                ViewBag.NumberOfGroups = groups.Count();

                List<Subgroup> subgroups = this.storage.GetRepository<ISubgroupRepository>().All().ToList();
                ViewBag.NumberOfSubgroups = subgroups.Count();
            }
            if (User.IsInRole("Teacher"))
            {
                Teacher teacher = this.storage.GetRepository<ITeachersRepository>().FindTeacherByUserId(Guid.Parse(userId));

                List<Course> courses = this.storage.GetRepository<ICourseRepository>().GetAllByTeacherId(teacher.Id).ToList();
                ViewBag.NumberOfCourses = courses.Count();

                List<Laboratory> laboratories = this.storage.GetRepository<ILaboratoryRepository>().AllByTeacherId(teacher.Id).ToList();
                ViewBag.NumberOfLaboratories = laboratories.Count();

                ViewBag.NumberOfCourseAssignments = 0;
                ViewBag.NumberOfLaboratoryAssignments = 0;
                ViewBag.CourseAssignmentSubmissions = 0;
                ViewBag.LaboratoryAssignmentSubmissions = 0;
                foreach (Course course in courses)
                {
                    List<Assignment> assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(course.Id).ToList();
                    ViewBag.NumberOfCourseAssignments += assignments.Count();

                    foreach (Assignment assignment in assignments)
                    {
                        List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id).ToList();
                        ViewBag.CourseAssignmentSubmissions += submissions.Count();
                    }
                }
                foreach (Laboratory laboratory in laboratories)
                {
                    List<Assignment> assignments = this.storage.GetRepository<IAssignmentRepository>().AllBySubjectId(laboratory.Id).ToList();
                    ViewBag.NumberOfLaboratoryAssignments = ViewBag.NumberOfLaboratoryAssignments + assignments.Count();

                    foreach (Assignment assignment in assignments)
                    {
                        List<Submission> submissions = this.storage.GetRepository<ISubmissionRepository>().AllByAssignmentId(assignment.Id).ToList();
                        ViewBag.LaboratoryAssignmentSubmissions += submissions.Count();
                    }
                }
            }
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
                ViewBag.Courses = courses.Count;


                IEnumerable<Laboratory> labs = this.storage.GetRepository<ILaboratoryRepository>().AllBySubgroupId(student.SubgroupId);
                List<Tuple<Laboratory, Course>> list = new List<Tuple<Laboratory, Course>>();
                foreach (Laboratory lab in labs)
                {
                    Course course = this.storage.GetRepository<ICourseRepository>().FindById(lab.CourseId);
                    list.Add(new Tuple<Laboratory, Course>(lab, course));
                }
                ViewBag.Labs = list.Count;

                ViewBag.Group = this.storage.GetRepository<IGroupRepository>().FindById(student.GroupId);
                ViewBag.Subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(student.SubgroupId);
            }


            return View();
        }
    }
}
