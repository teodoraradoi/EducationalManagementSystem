using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Students.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Students.Data.Entities;
using Groups.Data.Entities;
using Groups.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Identity.Data.Entities;
using Students.ViewModels;
using Courses.Data.Abstractions;
using Courses.Data.Entities;
using Laboratories.Data.Entities;
using Laboratories.Data.Abstractions;

namespace Students.Controllers
{
    public class StudentsController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public StudentsController(IStorage storage, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.storage = storage;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Secretary")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Student> students = this.storage.GetRepository<IStudentRepository>().All();
            List<Tuple<Student, ApplicationUser, Group, Subgroup>> list = new List<Tuple<Student, ApplicationUser, Group, Subgroup>>();
            foreach(Student student in students)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(student.UserId.ToString());
                Group group = this.storage.GetRepository<IGroupRepository>().FindById(student.GroupId);
                Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(student.SubgroupId);
                list.Add(new Tuple<Student, ApplicationUser, Group, Subgroup>(student, user, group, subgroup));
            }
            return View(list);
        }

        public List<Group> GetGroupsList()
        {
            List<Group> list = new List<Group>();
            IEnumerable<Group> groups = this.storage.GetRepository<IGroupRepository>().All();
            list = groups.ToList();
            return list;
        }

        [HttpGet]
        public JsonResult GetSubgroupsList(Guid id)
        {
            List<Subgroup> list = new List<Subgroup>();
            IEnumerable<Subgroup> subgroups = this.storage.GetRepository<ISubgroupRepository>().AllById(id);
            list = subgroups.ToList();
            return Json(list);
        }
        public List<Subgroup> GetSubgroupsList()
        {
            List<Subgroup> list = new List<Subgroup>();
            IEnumerable<Subgroup> subgroups = this.storage.GetRepository<ISubgroupRepository>().All();
            list = subgroups.ToList();
            return list;
        }

        // GET: StudentsController/CreateStudent
        [Authorize(Roles = "Secretary")]
        public ActionResult Create()
        {
            StudentAccountCreateViewModel model = new StudentAccountCreateViewModel();
            ViewBag.Groups = this.GetGroupsList();
            return View(model);
        }

        [Authorize(Roles = "Secretary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] StudentAccountViewModel model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser newStudent = new ApplicationUser
                {
                    Id = new Guid(),
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var user = await _userManager.CreateAsync(newStudent, model.Password);
                if (user.Succeeded)
                {
                    var createdUser = await _userManager.FindByIdAsync(newStudent.Id.ToString());
                    Student student = new Student
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        UserId = createdUser.Id,
                        GroupId = new Guid(model.GroupId),
                        SubgroupId = new Guid(model.SubgroupId)
                    };
                    this.storage.GetRepository<IStudentRepository>().Create(student);
                    this.storage.Save();

                    var studentRole = await _roleManager.FindByNameAsync("Student");
                    if (studentRole != null)
                    {
                        await _userManager.AddToRoleAsync(createdUser, "Student");
                    }
                    return RedirectToAction("Index", "Default");
                }
            }
            return View();
        }

        public async Task<ActionResult> Details(Guid id)
        {
            DetailsViewModel model = new DetailsViewModel();
            model.student = this.storage.GetRepository<IStudentRepository>().FindById(id);
            model.user = await _userManager.FindByIdAsync(model.student.UserId.ToString());

            IEnumerable<CourseGroup> courseGroups = this.storage.GetRepository<ICourseGroupRepository>().GetByGroupId(model.student.GroupId);
            List<Course> courses = new List<Course>();
            foreach (CourseGroup courseGroup in courseGroups)
            {
                Course course = this.storage.GetRepository<ICourseRepository>().FindById(courseGroup.CourseId);
                courses.Add(course);
            }
            model.courses = courses;


            IEnumerable<Laboratory> labs = this.storage.GetRepository<ILaboratoryRepository>().AllBySubgroupId(model.student.SubgroupId);
            List<Tuple<Laboratory, Course>> list = new List<Tuple<Laboratory, Course>>();
            foreach (Laboratory lab in labs)
            {
                Course course = this.storage.GetRepository<ICourseRepository>().FindById(lab.CourseId);
                list.Add(new Tuple<Laboratory, Course>(lab, course));
            }
            model.laboratories = list;
            Group group= this.storage.GetRepository<IGroupRepository>().FindById(model.student.GroupId);
            ViewBag.GroupName = group.Name;
            ViewBag.GroupId = group.Id;
            Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(model.student.SubgroupId);
            ViewBag.SubgroupName = subgroup.Name;
            ViewBag.SubgroupId = subgroup.Id;

            return View(model);
        }

        [Authorize(Roles = "Secretary")]
        public async Task<ActionResult> Edit(Guid id)
        {
            Student student = this.storage.GetRepository<IStudentRepository>().FindById(id);
            ApplicationUser user = await _userManager.FindByIdAsync(student.UserId.ToString());
            StudentAccountViewModel model = new StudentAccountViewModel()
            {
                Id = student.Id.ToString(),
                Name = student.Name,
                Email = user.Email,
                Password = user.PasswordHash,
                GroupId = student.GroupId.ToString(),
                SubgroupId = student.SubgroupId.ToString()
            };

            ViewBag.Groups = this.GetGroupsList();
            return View(model);
        }

        [Authorize(Roles = "Secretary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Email,GroupId,SubgroupId")] StudentAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                Student student = this.storage.GetRepository<IStudentRepository>().FindById(id);
                student.Name = model.Name;
                student.GroupId = Guid.Parse(model.GroupId);
                student.SubgroupId = Guid.Parse(model.SubgroupId);
                this.storage.GetRepository<IStudentRepository>().Update(student);
                this.storage.Save();

                ApplicationUser user = await _userManager.FindByIdAsync(student.UserId.ToString());
                user.Email = model.Email;
                user.UserName = model.Email;
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index", "Default");  
            }
            return View();
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student student = this.storage.GetRepository<IStudentRepository>().FindById(id);
            ApplicationUser user = await _userManager.FindByIdAsync(student.UserId.ToString());
            DetailsViewModel model = new DetailsViewModel()
            {
                student = student,
                user = user
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Student student = this.storage.GetRepository<IStudentRepository>().FindById(id);
            ApplicationUser user = await _userManager.FindByIdAsync(student.UserId.ToString());
            this.storage.GetRepository<IStudentRepository>().Delete(id);
            this.storage.Save();

            await _userManager.RemoveFromRoleAsync(user, "Student");
            await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

    }
}
