using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameConsuleWebStore.Data;
using GameConsuleWebStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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


        //Procced To CheckOut - Redirect to order-creat with the data we need
        public IActionResult ProcceedToCheck()
        {
            List<int> lstQty = new List<int>();
            List<int> lstId = new List<int>();
            int idProduct, qtyProduct;
            foreach (Item it in cartTemp)
            {
                idProduct = it.Product.ProductId;
                qtyProduct = it.Quantity;
                lstId.Add(idProduct);
                lstQty.Add(qtyProduct);
            }
            var result1 = string.Join(";", lstQty.Select(x => x.ToString()).ToArray());
            HttpContext.Session.SetString("lstQty", result1);
            var result2 = string.Join(";", lstId.Select(x => x.ToString()).ToArray());
            HttpContext.Session.SetString("lstIds", result2);


            return RedirectToAction("Create", "Orders");
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
            HttpContext.Session.SetString("CartNumOfItems", cartTemp.Count.ToString());
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
            HttpContext.Session.SetString("CartNumOfItems", cartTemp.Count.ToString());

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

            HttpContext.Session.SetString("CartNumOfItems", cartTemp.Count.ToString());


            return View("Cart");
        }
    }
}
