using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Secretaries.Data.Abstractions;
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

    public SecretariesController(IStorage storage)
        {
            this.storage = storage;
        }
        public ActionResult Index()
        {
            //return View("~/Views/Index.cshtml");
            return View("~/Views/Index.cshtml", this.storage.GetRepository<ISecretaryRepository>().All());
           // return View(this.storage.GetRepository<ISecretaryRepository>().All());
            //return View();
        }
    }
}
