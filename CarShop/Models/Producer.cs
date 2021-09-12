using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage = "Required field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Path to photo")]
        public string LogoUrl { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Info { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
