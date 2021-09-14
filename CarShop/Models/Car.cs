using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CarShop.CustomValidationAttributes;
using System.Text.Json.Serialization;

#nullable disable

namespace CarShop
{
    public partial class Car
    {
        public int CarId { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Required field")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Required field")]
        [YearRange(minYear: 2000)]
        public int Year { get; set; }
        [Required(ErrorMessage = "Required field")]
        [Display(Name = "Producer")]
        public int ProducerId { get; set; }
        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Path to photo")]
        public string PhotoUrl { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Description { get; set; }
        public int? OrderId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
        public virtual Producer Producer { get; set; }
    }
}
