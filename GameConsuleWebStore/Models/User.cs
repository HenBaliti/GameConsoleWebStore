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
        [Display(Name = "Full Name")]
        [Required]
        public String Name { get; set; }

        [Required]
        public String UserName { get; set; }

        [StringLength(8, ErrorMessage = "{0} must be between {2}-{1} characters", MinimumLength = 6)]
        [Required]
        public String Password { get; set; }
        public String UserType { get; set; }
        //[Required]
        public String Email { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}
