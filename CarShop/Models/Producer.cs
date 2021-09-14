using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
        [JsonIgnore]
        public virtual ICollection<Car> Cars { get; set; }
    }
}
