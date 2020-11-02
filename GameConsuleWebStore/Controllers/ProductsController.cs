using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameConsuleWebStore.Data;
using GameConsuleWebStore.Models;

namespace GameConsuleWebStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly GameConsuleWebStoreContext _context;
        public ProductsController(GameConsuleWebStoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {

            var PriceLst = from game in _context.Product
                           select game.Price;
            ViewBag.MaxPrice = PriceLst.Max();

            ViewBag.GamesLength = _context.Product.Count();


            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var product2 = await _context.Product
    .FirstOrDefaultAsync(m => m.ProductId == id);
            var StoreNameOfCurrent = product2.StoreLocation;

            var Store2 = await _context.StoreAddress
    .FirstOrDefaultAsync(m => m.StoreName == StoreNameOfCurrent);

            var LatitudeNum = Store2.Latitude;
            var LongitudeNum = Store2.Longitude;

            ViewBag.StoreMapLatitude = LatitudeNum;
            ViewBag.StoreMapLongitude = LongitudeNum;

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ReleaseDate,Price,StockUnit,pathPicture,ConsoleType,Category,StoreLocation")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ReleaseDate,Price,StockUnit,pathPicture,ConsoleType,Category,StoreLocation")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
        public async Task<IActionResult> SearchByConsoleType(string TypeConsoleSelect, string FilterByPrice, String TypeCategorySelect)
        {
            IQueryable<Product> products = _context.Product;
            IQueryable<Product> products1 = _context.Product;
            //only console selection
            if (TypeConsoleSelect != null && int.Parse(FilterByPrice) == 0 && TypeCategorySelect == null)
            {
                products1 = products.Where(p => p.ConsoleType.Contains(TypeConsoleSelect));
                return View("Index", await products1.ToListAsync());
            }
            //console + year selection
            if (TypeConsoleSelect != null && int.Parse(FilterByPrice) == 0 && TypeCategorySelect != null)
            {
                products1 = products.Where(p => p.ConsoleType.Contains(TypeConsoleSelect)).Where(p => p.Category == TypeCategorySelect);
                return View("Index", await products1.ToListAsync());
            }
            //console + price selection
            if (TypeConsoleSelect != null && int.Parse(FilterByPrice) != 0 && TypeCategorySelect == null)
            {
                products1 = products.Where(p => p.ConsoleType.Contains(TypeConsoleSelect)).Where(p => p.Price <= int.Parse(FilterByPrice)); ;
                return View("Index", await products1.ToListAsync());
            }

            //3 together
            if (TypeConsoleSelect != null && int.Parse(FilterByPrice) != 0 && TypeCategorySelect != null)
            {
                products1 = products.Where(p => p.ConsoleType.Contains(TypeConsoleSelect)).Where(p => p.Category == TypeCategorySelect).Where(p => p.Price <= int.Parse(FilterByPrice)); ;
                return View("Index", await products1.ToListAsync());
            }
            //only year selection
            if (TypeConsoleSelect == null && int.Parse(FilterByPrice) == 0 && TypeCategorySelect != null)
            {
                products1 = products.Where(p => p.Category == TypeCategorySelect);
                return View("Index", await products1.ToListAsync());
            }
            //year + price
            if (TypeConsoleSelect == null && int.Parse(FilterByPrice) != 0 && TypeCategorySelect != null)
            {
                products = products1.Where(p => p.Category == TypeCategorySelect).Where(p => p.Price <= int.Parse(FilterByPrice)); ;
                return View("Index", await products1.ToListAsync());
            }

            //only price select
            if (TypeConsoleSelect == null && int.Parse(FilterByPrice) != 0 && TypeCategorySelect == null)
            {
                products1 = products.Where(p => p.Price <= int.Parse(FilterByPrice));
                return View("Index", await products1.ToListAsync());
            }
            //nothing select - show all
            //if (TypeConsoleSelect == null && FilterByPrice == null && YearRealeaseSelect == 0)
            //    return View("Index", await products.ToListAsync());
            return View("Index", await products.ToListAsync());
        }

    }
}
