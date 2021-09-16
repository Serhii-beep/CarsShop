using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace CarShop
{
    public partial class Order
    {
        public Order()
        {
            Cars = new HashSet<Car>();
        }

        public int OrderId { get; set; }
        [Display(Name = "Customer Full Name")]
        [Required(ErrorMessage ="Required field")]
        public string CustomerFullName { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Address { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
