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

    }
}