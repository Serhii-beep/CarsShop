using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace CarShop.DOM.Database
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

        public int WarehouseId { get; set; }
        public virtual ICollection<Car> Cars { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }
}
