using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BookingApplication.WPF.View.TourGuestView.Validation
{
    class LettersValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString() == String.Empty) // null value is considered valid
            {
                return ValidationResult.ValidResult;
            }
            else if (Regex.IsMatch(value.ToString(), @"^[a-zA-Z]+$")) // check if value contains only letters
            {
                return ValidationResult.ValidResult;
            }
            else // invalid value
            {
                return new ValidationResult(false, "Input must contain only letters.");
            }
        }
    }
}
