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

        public DefaultController(IStorage storage)
        {
            this.storage = storage;
        }
        
        public ActionResult Index()
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

            return View();
        }
    }
}
