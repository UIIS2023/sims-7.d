using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApplication.WPF.View.TourGuestView.Validation
{
    public class DateValidation : ValidationRule
    {
        private DateTime _minDate = new DateTime(2022, 1, 1); // set your minimum date here

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is DateTime selectedDate && selectedDate < _minDate)
            {
                return new ValidationResult(false, $"Dates before {_minDate.ToShortDateString()} are not allowed.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
