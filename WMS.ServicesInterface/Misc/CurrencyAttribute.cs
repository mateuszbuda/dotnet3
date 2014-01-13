using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.ServicesInterface.Misc
{
    public class CurrencyAttribute : RegularExpressionAttribute
    {
        public CurrencyAttribute()
            : base(GetRegex())
        { }

        private static string GetRegex()
        {
            string separator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).ToString();
            return "[0-9]+(\\" + separator + "[0-9]*)?";
        }
    }
}
