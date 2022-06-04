using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Groups.Data.Abstractions;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Http;
using Groups.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Identity.Data.Entities;

namespace Groups.Controllers
{
    public class GroupsController : Controller
    {
        private IStorage storage;
        private UserManager<ApplicationUser> _userManager;

        public GroupsController(IStorage storage, UserManager<ApplicationUser> userManager)
        {
            this.storage = storage;
            _userManager = userManager;
        }
        
        //[Authorize(Roles = "Secretary")]
        public ActionResult Index()
        {
            return View(this.storage.GetRepository<IGroupRepository>().All());
        }

        // GET: GroupsController/Create
        [Authorize(Roles = "Secretary")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GroupsController/Create
        [Authorize(Roles = "Secretary")]
        [HttpPost]
        public ActionResult Create([FromForm] Group group)
        {
            if(ModelState.IsValid)
            {
                this.storage.GetRepository<IGroupRepository>().Create(group);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        // GET: GroupsController/Edit/5
        [Authorize(Roles = "Secretary")]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Group group = this.storage.GetRepository<IGroupRepository>().FindById(id);

            return View(group);
        }

        // POST: GroupssController/Edit/5
        [Authorize(Roles = "Secretary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid Id, [Bind("Id,SpecializationName,Name,Year")] Group group)
        {
            if (ModelState.IsValid)
            {
                this.storage.GetRepository<IGroupRepository>().Edit(group);
                this.storage.Save();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: GroupsController/Delete/5
        [Authorize(Roles = "Secretary")]
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Group group = this.storage.GetRepository<IGroupRepository>().FindById(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: GroupsController/Delete/5
        [Authorize(Roles = "Secretary")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            this.storage.GetRepository<IGroupRepository>().Delete(id);
            this.storage.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}