using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Model
{
    public static class DateTimeFormat
    {
        public static string Format = "dd/MM/yyyy HH:mm:ss";
        public static CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
        public static CultureInfo GetCultureInfo()
        {
            CultureInfo.DateTimeFormat.ShortDatePattern = Format.Split(' ')[0];
            CultureInfo.DateTimeFormat.FullDateTimePattern = Format;

            return CultureInfo;
        }
    }
}