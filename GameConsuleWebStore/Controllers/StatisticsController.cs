using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameConsuleWebStore.Data;
using GameConsuleWebStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameConsuleWebStore.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly GameConsuleWebStoreContext _context;
        public StatisticsController(GameConsuleWebStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            
            //Blocking Not Admin
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Login", "Users", new { messageAlert = "You are not connected as Admin member." });
            }

            return View("PieStat");
        }
    }
}
