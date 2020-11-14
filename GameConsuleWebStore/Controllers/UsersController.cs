using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameConsuleWebStore.Data;
using GameConsuleWebStore.Models;
using Microsoft.AspNetCore.Http;
using GameConsuleWebStore.Controllers;

namespace GameConsuleWebStore.Controllers
{
    public class UsersController : Controller
    {
        private readonly GameConsuleWebStoreContext _context;

        public UsersController(GameConsuleWebStoreContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Dashboard()
        {
            return View(await _context.User.ToListAsync());
        }


        private void SignIn(User user)
        {
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserType", user.UserType.ToString());
        }


        //Login
        public IActionResult Login(string dsf)
        {
            if (dsf != null)
            {
                ViewBag.AlertUser = dsf;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                SignIn(user);
                return RedirectToAction("Index", "Home");
            }
            else //Username ore pass isnt good
            {
                //return RedirectToAction("Eror", "Users");
       
                return RedirectToAction("Login", "Users");

            }
        }

        //-------------------------------------------------------------------------------------------------



        //Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Name,UserName,Password,UserType,Email")] User user)
        {
            user.UserType = "User";
            _context.Add(user);
            await _context.SaveChangesAsync();

            SignIn(user);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index", "Home");
        }


        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserType") == "Admin")
            {
                return View(await _context.User.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
                if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserType") != "Admin")
            {
                return RedirectToAction("Login", "Users");
            }
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UserName,Password,UserType,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserName,Password,UserType,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        //DeleteOrder First
        public void DeleteOrderFirst(int id)
        {
            var order2 = _context.Order.Include(x => x.ItemsPerOrder).First(o => o.Id == id);

            //*********Removing the entity "Item" because its one to many realationship**********
            foreach (Item idItem in order2.ItemsPerOrder)
            {
                Item item = _context.Item.Find(idItem.ItemId);
                _context.Item.Remove(item);
            }
            Order order = _context.Order.Find(id);
            _context.SaveChangesAsync();
            _context.Order.Remove(order);
            _context.SaveChangesAsync();

        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = _context.Order.Where(p => p.User.Id==id);
            
            //*********Removing the entity "Order" because its one to many realationship**********
            foreach (Order idOrder in orders)
            {
                DeleteOrderFirst(idOrder.Id);
            }
            var user = await _context.User.FindAsync(id);
            await _context.SaveChangesAsync();
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        public async Task<IActionResult> SearchByUserType(string TypeUserSelect, string UserNamesSelect, string NamesSelect)
        {
            IQueryable<User> users = _context.User;
            IQueryable<User> users1 = _context.User;
            if (TypeUserSelect != null && UserNamesSelect==null  && NamesSelect == null)
            {
                users1 = users.Where(u => u.UserType.Contains(TypeUserSelect));
                return View("Index", await users1.ToListAsync());
            }
            if (TypeUserSelect == null && UserNamesSelect != null && NamesSelect == null)
            {
                users1 = users.Where(u => u.UserName.Contains(UserNamesSelect));
                return View("Index", await users1.ToListAsync());
            }
            if (TypeUserSelect == null && UserNamesSelect == null && NamesSelect != null)
            {
                users1 = users.Where(u => u.Name.Contains(NamesSelect));
                return View("Index", await users1.ToListAsync());
            }
            if (TypeUserSelect != null && UserNamesSelect != null && NamesSelect == null)
            {
                users1 = users.Where(u => u.UserType.Contains(TypeUserSelect)).Where(u=>u.UserName.Contains(UserNamesSelect));
                return View("Index", await users1.ToListAsync());
            }
            if (TypeUserSelect != null && UserNamesSelect == null && NamesSelect != null)
            {
                users1 = users.Where(u => u.UserType.Contains(TypeUserSelect)).Where(u => u.Name.Contains(NamesSelect));
                return View("Index", await users1.ToListAsync());
            }
            if (TypeUserSelect == null && UserNamesSelect != null && NamesSelect != null)
            {
                users1 = users.Where(u => u.UserName.Contains(UserNamesSelect)).Where(u => u.Name.Contains(NamesSelect));
                return View("Index", await users1.ToListAsync());
            }
            if (TypeUserSelect != null && UserNamesSelect != null && NamesSelect != null)
            {
                users1 = users.Where(u => u.UserType.Contains(TypeUserSelect)).Where(u => u.UserName.Contains(UserNamesSelect)).Where(u => u.Name.Contains(NamesSelect));
                return View("Index", await users1.ToListAsync());
            }
            //nothing
            return View("Index", await users.ToListAsync());
        }

    }
}
    