using GroceryKiosk.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GroceryKiosk
{
    public static class ReceiptService
    {

        public static void printReceipt(Hashtable items, double totalPrice)
        {
            foreach (DictionaryEntry entry in items)
            {
                string price = ((entry.Value as Item).price);
                string salePrice = ((entry.Value as Item).salePrice);
                int quantity = ((entry.Value as Item).quantity);
                Console.WriteLine("==============================");
                Console.WriteLine(entry.Key);
                Console.WriteLine("Quantity: " + quantity);
                Console.WriteLine("Regular Price: " + price);
                if (salePrice != "N/A")
                {
                    Console.WriteLine("Applied sale price: " + salePrice);
                }

            }
            Console.WriteLine("==============================");
            Console.WriteLine("Total: " + StringUtils.formatPrice(totalPrice));

        }

    }
}
