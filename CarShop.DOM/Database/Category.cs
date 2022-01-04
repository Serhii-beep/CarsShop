using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CarShop.DOM.Database
{
    public partial class Category
    {
        public Category()
        {
            Cars = new HashSet<Car>();
        }

        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Name { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
