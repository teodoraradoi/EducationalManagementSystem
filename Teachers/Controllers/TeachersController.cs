﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExtCore.Data.Abstractions;
using Teachers.Data.Abstractions;

namespace Teachers.Controllers
{
    public class TeachersController : Controller
    {
        private IStorage storage;

        public TeachersController(IStorage storage)
        {
            this.storage = storage;
        }
        // GET: TeachersController
        public ActionResult Index()
        {
           return View(this.storage.GetRepository<ITeachersRepository>().All());
        }

        // GET: TeachersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TeachersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeachersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: TeachersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeachersController/Edit/5
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

        // GET: TeachersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeachersController/Delete/5
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
