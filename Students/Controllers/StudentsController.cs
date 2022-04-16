using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Students.Data.Abstractions;

namespace Students.Controllers
{
    public class StudentsController : Controller
    {
        private IStorage storage;

        public StudentsController(IStorage storage)
        {
            this.storage = storage;
        }
        public ActionResult Index()
        {
            return View(this.storage.GetRepository<IStudentRepository>().All());
        }
    }
}
