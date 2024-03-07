using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookingApplication.Domain.Regexes
{
    public class LanguageRegex
    {
        public static string LanguagePattern;
        public Regex Regex;
        public LanguageRegex()
        {
            LanguagePattern = "[a-zA-Z\\s]+";
            Regex = new Regex(LanguagePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}
