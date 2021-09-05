using System;
using System.Collections.Generic;

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
        public string CustomerFullName { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
