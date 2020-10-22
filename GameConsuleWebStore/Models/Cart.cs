using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameConsuleWebStore.Models
{
    public class Cart
    {
        public int productId { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public int Qty { get; set; }
        public float Bill { get; set; }
    }
}
