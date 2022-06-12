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
            List<Tuple<ApplicationUser, Group, Subgroup>> list = new List<Tuple<ApplicationUser, Group, Subgroup>>();
            foreach(Student student in students)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(student.UserId.ToString());
                Group group = this.storage.GetRepository<IGroupRepository>().FindById(student.GroupId);
                Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(student.SubgroupId);
                list.Add(new Tuple<ApplicationUser, Group, Subgroup>(user, group, subgroup));
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
            // model.Groups = this.GetGroupsList();
            ViewBag.Groups = this.GetGroupsList();
            //model.Subgroups = this.GetSubgroupsList();
            //model.Subgroups = new List<Subgroup>();
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

                //var hashedPassword = _passwordHasher.HashPassword(newStudent, model.Password);

                var user = await _userManager.CreateAsync(newStudent, model.Password);
                if (user.Succeeded)
                {
                    var createdUser = await _userManager.FindByIdAsync(newStudent.Id.ToString());
                    Student student = new Student
                    {
                        Id = Guid.NewGuid(),
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
    }
}
