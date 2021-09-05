using System;
using System.Collections.Generic;

#nullable disable

namespace CarShop
{
    public partial class Category
    {
        public Category()
        {
            Cars = new HashSet<Car>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
