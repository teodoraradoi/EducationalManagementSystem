using ExtCore.Data.Abstractions;
using Groups.Data.Abstractions;
using Groups.Data.Entities;
using Groups.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Groups.Controllers
{
    public class SubgroupsController : Controller
    {
        private IStorage storage;

        public SubgroupsController(IStorage storage)
        {
            this.storage = storage;
        }

        // GET: SubgroupController
        public ActionResult Index()
        {
            IEnumerable subgroups = this.storage.GetRepository<ISubgroupRepository>().All();
            List<Tuple<Subgroup, Group>> list = new List<Tuple<Subgroup, Group>>();

            foreach (Subgroup subgroup in subgroups)
            {
                Group group = this.storage.GetRepository<IGroupRepository>().FindById(subgroup.GroupId);
                list.Add(new Tuple<Subgroup, Group>(subgroup, group));
            }
            return View(list);
        }

        // GET: SubgroupController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public List<Group> GetCreateList()
        {
            List<Group> list = new List<Group>();
            IEnumerable<Group> groups= this.storage.GetRepository<IGroupRepository>().All();
            //list = (List<Group>)groups;
            list = groups.ToList();
            return list;
        }

        // GET: SubgroupController/Create
        public ActionResult Create()
        {
            CreateViewModel createViewModel = new CreateViewModel();
            createViewModel.Groups = this.GetCreateList();
            return View(createViewModel);
        }

        // POST: SubgroupController/Create
        [HttpPost]
        public ActionResult Create([FromForm] Subgroup subgroup)
        {
            if (ModelState.IsValid)
            {
                subgroup.Id = Guid.NewGuid();
                this.storage.GetRepository<ISubgroupRepository>().Create(subgroup);
                this.storage.Save();
                return RedirectToAction("Index", "Default");
            }
            return this.View();
        }

        // GET: SubgroupController/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(id);
            CreateViewModel createViewModel = new CreateViewModel();
            createViewModel.Groups = this.GetCreateList();
            createViewModel.Subgroup = subgroup;

            return View(createViewModel);
        }

        // POST: SubgroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid Id, [Bind("Id,Name,GroupId")] Subgroup subgroup)
        {
            if (ModelState.IsValid)
            {
                subgroup.Id = Id;
                this.storage.GetRepository<ISubgroupRepository>().Edit(subgroup);
                this.storage.Save();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: SubgroupController/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Subgroup subgroup = this.storage.GetRepository<ISubgroupRepository>().FindById(id);
            Group group = this.storage.GetRepository<IGroupRepository>().FindById(subgroup.GroupId);
            ViewBag.GroupName = group.Name;
            if (subgroup == null)
            {
                return NotFound();
            }
            return View(subgroup);
        }

        // POST: SubgroupController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            this.storage.GetRepository<ISubgroupRepository>().Delete(id);
            this.storage.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
