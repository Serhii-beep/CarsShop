using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarShop.DOM.Database
{
    public class Warehouse
    {
        [Display (Name ="Warehouse")]
        public int WarehouseId { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Range(-90, 90, ErrorMessage = "Latitude values ​​should be greater than 90 and less than 90")]
        public double Latitude { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Range(0, 360, ErrorMessage = "Longitude values ​​should be greater than 0 and less than 360 ")]
        public double Longitude { get; set; }

        public virtual ICollection<Car> Cars { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
