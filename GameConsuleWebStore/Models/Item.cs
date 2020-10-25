using GameConsuleWebStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameConsuleWebStore.Controllers
{
    public class Item
    {
        private Product product = new Product();
        private int quantity;

        public Item()
        {

        }

        public Item(Product product,int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }

        public Product Product { get => product; set => product = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
