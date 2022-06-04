using Identity.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barebone.Controllers
{
    public class DefaultController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        public DefaultController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public ActionResult Index()
        {
            var user = _userManager.GetUserId(User);
            return View();
        }
    }
}
