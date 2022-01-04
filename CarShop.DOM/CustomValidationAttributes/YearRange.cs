using System;
using System.ComponentModel.DataAnnotations;

namespace CarShop.DOM.CustomValidationAttributes
{
    public class YearRange : ValidationAttribute
    {
        private int _minYear;
        
        public YearRange(int minYear)
        {
            _minYear = minYear;
        }

        public override bool IsValid(object value)
        {
            ErrorMessage = $"Year must be not less than {_minYear} and not greater than {DateTime.UtcNow.Year}";
            return ((int)value >= _minYear && (int)value <= DateTime.UtcNow.Year);
        }
    }
}
