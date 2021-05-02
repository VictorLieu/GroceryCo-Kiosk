using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryKiosk.utils
{
    class StringUtils
    {
        // Utility methods used to format prices
        public static double parsePrice(string price)
        {
            string priceString = price.Replace("$", string.Empty);
            double priceDouble = Convert.ToDouble(priceString);
            return priceDouble;
        }
        
        public static string formatPrice(double price)
        {
            string formattedPrice = "$" + price.ToString("F");
            return formattedPrice;
        }
        
    }
}
