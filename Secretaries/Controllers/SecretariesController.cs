using ExtCore.Data.Abstractions;
using Groups.Data.Abstractions;
using Groups.Data.Entities;
using Identity.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Secretaries.Data.Abstractions;
using Secretaries.ViewModels;
using Students.Data.Abstractions;
using Students.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secretaries.Controllers
{
    public class SecretariesController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SecretariesController(IStorage storage, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.storage = storage;
            _userManager = userManager;
           _roleManager = roleManager;
        }

        [Authorize(Roles = "Secretary")]
        public ActionResult Index()
        {
            //return View("~/Views/Index.cshtml");
            //return View("~/Views/Index.cshtml", this.storage.GetRepository<ISecretaryRepository>().All());
             return View(this.storage.GetRepository<ISecretaryRepository>().All());
            //return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([FromForm] Secretary model)
         {
         }*/

        public List<Group> GetGroupsList()
        {
            List <Group> list = new List<Group>();
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

        // GET: SecretariesController/CreateStudent
        [Authorize(Roles = "Secretary")]
        public ActionResult CreateStudent()
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
        public async Task<IActionResult> CreateStudent([FromForm] StudentAccountViewModel model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser newStudent = new ApplicationUser
                {
                    Id = new Guid(),
                    UserName = model.Name,
                    Email = model.Email,
                    EmailConfirmed = true
                };
                var user = await _userManager.CreateAsync(newStudent, model.Password);
                if(user.Succeeded)
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
                    if(studentRole != null)
                    {
                        await _userManager.AddToRoleAsync(createdUser, "Student");
                    }
                    return RedirectToAction("Index", "Default");
                }
            }
            return View();
            //return RedirectToAction("Default", "Home");
        }
    }
}
