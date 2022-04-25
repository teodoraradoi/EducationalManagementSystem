using ExtCore.Data.Abstractions;
using Groups.Data.Abstractions;
using Groups.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
            return View();
        }

        // GET: SubgroupController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubgroupController/Create
        public ActionResult Create()
        {
            return View();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubgroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SubgroupController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubgroupController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
