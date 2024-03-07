using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Regexes
{
    public class NumberRegex
    {
        public static string NumberPattern;
        public Regex Regex;
        public NumberRegex()
        {
            NumberPattern = "[\\d]+";
            Regex = new Regex(NumberPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

    }
}
