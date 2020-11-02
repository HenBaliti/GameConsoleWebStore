using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameConsuleWebStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public String Name {get; set;}

        public DateTime ReleaseDate { get; set; }

        public double Price { get; set; }

        public int StockUnit { get; set; }

        public String pathPicture { get; set; }

        public String Category { get; set; }
        public String ConsoleType { get; set; }

        public String StoreLocation { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
