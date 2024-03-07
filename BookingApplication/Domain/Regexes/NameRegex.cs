using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Regexes
{
    public class NameRegex
    {
        public static string NamePattern;
        public Regex Regex;
        public NameRegex()
        {
            NamePattern = "[a-zA-Z\\d\\s:]+";
            Regex = new Regex(NamePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

    }
}
