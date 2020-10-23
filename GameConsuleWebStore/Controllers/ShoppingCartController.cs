using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameConsuleWebStore.Data;
using GameConsuleWebStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameConsuleWebStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        public static List<Item> cartTemp = new List<Item>();
        private GameConsuleWebStoreContext de;
        public ShoppingCartController(GameConsuleWebStoreContext de)
        {
            this.de = de;
        }
        public IActionResult Index()
        {
            List<Item> cart = cartTemp;

            ViewBag.CounterCart = cartTemp.Count;

            if (cart.Count==0)
            {
                ViewBag.cart = cart;
            }
            else
            {
                HttpContext.Session.SetString("Cart", cart.ToString());
                ViewBag.cart = cart;
            }
            /////////////////////////////////////////////////////////////////////////////////
            TempData["carTempData"] = ViewBag.CounterCart;
            return View("Cart");
        }

        private int isExistingProduct(int id)
        {
            List<Item> newCart = cartTemp;
            for(int i=0;i<newCart.Count;i++)
            {
                if (newCart[i].Product.ProductId == id)
                    return i;
            }
            return -1;
        }

        public IActionResult Delete(int id)
        {
            int index = isExistingProduct(id);
            List<Item> cart = cartTemp;

            cart.RemoveAt(index);
            HttpContext.Session.SetString("Cart", cart.ToString());
            ViewBag.cart = cart;
            cartTemp = cart;

            ViewBag.CounterCart = cartTemp.Count;
            /////////////////////////////////////////////////////////////////////////////////
            TempData["carTempData"] = ViewBag.CounterCart;

            return View("Cart");
        }


        public IActionResult OrderNow(int id)
        {
            if (HttpContext.Session.GetString("Cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item(de.Product.Find(id), 1));
                HttpContext.Session.SetString("Cart", cart.ToString());
                ViewBag.cart = cart;
                cartTemp = cart;
            }
            else
            {
                List<Item> cart = cartTemp;
                int index = isExistingProduct(id);
                if (index == -1)
                    cart.Add(new Item(de.Product.Find(id), 1));
                else
                    cart[index].Quantity++;
                
                HttpContext.Session.SetString("Cart", cart.ToString());
                ViewBag.cart = cart;
                cartTemp = cart;
            }

            ViewBag.CounterCart = cartTemp.Count;
            /////////////////////////////////////////////////////////////////////////////////
            TempData["carTempData"] = ViewBag.CounterCart;

            return View("Cart");
        }
    }
}
