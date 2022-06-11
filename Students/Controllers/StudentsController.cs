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

namespace Students.Controllers
{
    public class StudentsController : Controller
    {
        private IStorage storage;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(IStorage storage, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            _userManager = userManager;
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
    }
}
