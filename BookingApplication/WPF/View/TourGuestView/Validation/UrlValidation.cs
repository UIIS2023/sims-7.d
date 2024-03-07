using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApplication.WPF.View.TourGuestView.Validation
{
    class UrlValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString() == String.Empty || !Regex.IsMatch(value.ToString(),
                    @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$"))
            {
                return new ValidationResult(false, "URL is not valid.");
            }

            return ValidationResult.ValidResult;
        }
    }
}