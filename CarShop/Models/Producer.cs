using System;
using System.Collections.Generic;

#nullable disable

namespace CarShop
{
    public partial class Producer
    {
        public Producer()
        {
            Cars = new HashSet<Car>();
        }

        public int ProducerId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string LogoUrl { get; set; }
        public string Info { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
