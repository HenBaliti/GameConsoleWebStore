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
        
        [Required]
        [Display(Name = "Full Name")]
        [RegularExpression(@"^[A-Za-z ]*$", ErrorMessage = "• Please enter a valid name between A-Z or a-z")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "User Name")]
        [RegularExpression(@"^[A-Za-z0-9]*$", ErrorMessage = "• Please enter a valid characters between A-Z or a-z or 0-9")]
        public String UserName { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "• {0} must be between {2}-{1} characters", MinimumLength = 6)]
        public String Password { get; set; }

        [Display(Name = "User Type")]
        public String UserType { get; set; }
        
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        
        public ICollection<Order> Orders { get; set; }

    }
}
