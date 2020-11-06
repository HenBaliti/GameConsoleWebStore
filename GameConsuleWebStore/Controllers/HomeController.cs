using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GameConsuleWebStore.Models;
using Microsoft.AspNetCore.Http;
using GameConsuleWebStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace GameConsuleWebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameConsuleWebStoreContext _context;

        public HomeController(ILogger<HomeController> logger, GameConsuleWebStoreContext context)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult SearchAutoComplete(string term)
        {
            var query = from p in _context.Product
                        where p.Name.Contains(term)
                        select new {id=p.ProductId ,label =p.Name ,value=p.ProductId };
            return Json(query.ToList());
        }

        public IActionResult Index()
        {
            return JustForYouByRecentOrders();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Sign-In users - can see a suita`ble products by thier recent orders.
        public IActionResult JustForYouByRecentOrders()
        {

            List<Product> result = new List<Product>();
            if (HttpContext.Session.GetString("UserId") != null)
            {
                var UserOrders = _context.Order.Where(p => p.User.Id.ToString().Equals(HttpContext.Session.GetString("UserId")));
                var UserProducts = _context.ProductOrder.Include(p=>p.Product).Join(UserOrders, up => up.OrderId, uo => uo.Id, (up, uo) => up).Select(p=>p.Product).ToList();

                if (UserOrders.ToList().Count != 0)
                {
                    var ConsuleCounter = UserProducts.GroupBy(x => x.ConsoleType)
.ToDictionary(y => y.Key, y => y.Count())
.OrderByDescending(z => z.Value);

                    var CategoryCounter = UserProducts.GroupBy(x => x.Category)
    .ToDictionary(y => y.Key, y => y.Count())
    .OrderByDescending(z => z.Value);

                    //None of them
                    if (CategoryCounter == null && CategoryCounter == null)
                    {
                        ViewBag.ForYou = null;
                        return View("Index");
                    }
                    //only Category
                    if (ConsuleCounter == null && CategoryCounter.First().Value != 0)
                    {
                        result = _context.Product.Where(p => p.Category.Equals(CategoryCounter.First().Key)).ToList();
                        ViewBag.ForYou = result.Take(4);
                    }
                    //only Consule
                    if (ConsuleCounter.First().Value != 0 && CategoryCounter == null)
                    {
                        result = _context.Product.Where(p => p.ConsoleType.Equals(ConsuleCounter.First().Key)).ToList();
                        ViewBag.ForYou = result.Take(4);
                    }
                    //Consule + Category
                    if (ConsuleCounter.First().Value != 0 && CategoryCounter.First().Value != 0)
                    {
                        List<Product> lstProd1 = new List<Product>();
                        List<Product> lstProd2 = new List<Product>();
                        //result = ConsuleCounter.Join(CategoryCounter, con => con, cat => cat, (con, cat) => con);
                        lstProd1 = _context.Product.Where(p => p.ConsoleType.Equals(ConsuleCounter.First().Key) && p.Category.Equals(CategoryCounter.First().Key)).ToList();
                        if (lstProd1.Count < 4)
                        {
                            lstProd2 = _context.Product.Where(p => p.ConsoleType.Equals(ConsuleCounter.First().Key)).ToList();
                            //Erase the products that in lstProd1 From lstProd2
                            lstProd2 = lstProd2.Except(lstProd1).ToList();
                            //Putting the "Consule And Category" option prefer in front of the list
                            lstProd2.AddRange(lstProd1);
                            //Reversing the list for preffering the "Consule and Category" Option
                            lstProd2.Reverse();
                            //Taking the 4 Best For User Product
                            ViewBag.ForYou = lstProd2.Take(4);
                        }
                        else
                        {
                            ViewBag.ForYou = lstProd1.Take(4);
                        }
                    }
                }

                else
                {
                    ViewBag.ForYou = null;
                }


            }
            else
            {
                return View("Index");
            }
            return View("Index");
        }

    }
}
