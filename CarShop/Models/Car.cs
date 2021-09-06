using System;
using System.Collections.Generic;

#nullable disable

namespace CarShop
{
    public partial class Car
    {
        public int CarId { get; set; }
        public string Model { get; set; }
        public int CategoryId { get; set; }
        public int Price { get; set; }
        public int Year { get; set; }
        public int ProducerId { get; set; }
        public string PhotoUrl { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public int? OrderId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Order Order { get; set; }
        public virtual Producer Producer { get; set; }
    }
}
