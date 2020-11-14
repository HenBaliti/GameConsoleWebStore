using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameConsuleWebStore.Models
{
    public class Product
    {
        [Display(Name = "ID")]
        public int ProductId { get; set; }
        public String Name {get; set;}
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        public double Price { get; set; }

        [Display(Name = "Stock Unit")]
        public int StockUnit { get; set; }
        [Display(Name = "File Name of Picture")]
        public String pathPicture { get; set; }

        public String Category { get; set; }
        [Display(Name = "Console Type")]
        public String ConsoleType { get; set; }

        [Display(Name = "Store Location")]
        public String StoreLocation { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
