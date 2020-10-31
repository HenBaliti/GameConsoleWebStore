using GameConsuleWebStore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameConsuleWebStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public DateTime DateOrder { get; set; }

        public User User { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; }
        public ICollection<Item> ItemsPerOrder { get; set; }
    }
}
