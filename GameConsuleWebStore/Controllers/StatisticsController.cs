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
            List<Item> ListOfItemsPerOrder = new List<Item>();

            //Blocking Not Admin
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Login", "Users");
            }
            Dictionary<int, int> hash1 = new Dictionary<int, int>();
            var query = _context.Item.Include(p => p.Product).ToList();

            foreach (Item it in query)
            {
                if (hash1.ContainsKey(it.Product.ProductId))
                {
                    hash1[it.Product.ProductId] = hash1[it.Product.ProductId] + it.Quantity;
                }
                else
                {
                    hash1.Add(it.Product.ProductId, it.Quantity);
                }
            }
            //Dictionary<int, int> hash1 = new Dictionary<int, int>();
            //var orders = _context.Order.ToList();
            //foreach(Order it in orders)
            //{
            //    var order = await _context.Order.Include(po => po.ProductOrders).Include(po => po.ItemsPerOrder).ThenInclude(o => o.Product).FirstOrDefaultAsync(m => m.Id == it.Id);
            //    foreach(Item tt in order.ItemsPerOrder)
            //    {
            //        if (hash1.ContainsKey(tt.Product.ProductId))
            //        {
            //            hash1[tt.Product.ProductId] = hash1[tt.Product.ProductId] + tt.Quantity;
            //        }
            //        else
            //        {
            //            hash1.Add(tt.Product.ProductId, tt.Quantity);
            //        }
            //    }
            //}

            return View("PieStat");
        }
    }
}
