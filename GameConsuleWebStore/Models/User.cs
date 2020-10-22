using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameConsuleWebStore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }

        public String Password { get; set; }
        public String UserType { get; set; }
        public String Email { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
