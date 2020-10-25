using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameConsuleWebStore.Models
{
    public class StoreAddress
    {
        [Key]
        public int SotreAddressId { get; set; }

        public string StoreName { get; set; }
        public string AddressName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
