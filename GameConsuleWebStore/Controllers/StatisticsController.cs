using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameConsuleWebStore.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Login", "Users");
            }
            return View("PieStat");
        }
    }
}
