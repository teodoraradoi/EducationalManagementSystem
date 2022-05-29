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

namespace Groups.Controllers
{
    public class GroupsController : Controller
    {
        private IStorage storage;

        public GroupsController(IStorage storage)
        {
            this.storage = storage;
        }

        public ActionResult Index()
        {
            //return View();
            return View(this.storage.GetRepository<IGroupRepository>().All());
        }

        // GET: GroupsController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: GroupsController/Create
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