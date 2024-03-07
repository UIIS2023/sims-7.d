using BookingApplication.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using BookingApplication.Domain.Model;

namespace BookingApplication.WPF.View.TourGuestView.Converters
{
    public class LocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            dynamic obj = value;
            int locationId = obj.LocationId;

            var locationService = new LocationService();
            Location location = locationService.GetById(locationId);

            return location.Country + "•" + location.City;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
