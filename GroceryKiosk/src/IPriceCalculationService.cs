using System;
using System.Collections;
using System.Collections.Generic;


namespace GroceryKiosk.src
{
    public interface IPriceCalculationService
    {
        public void initCatalogs(string catalogPath);
        public double calculateTotal(List<string> itemList);
        public Hashtable formatItems(List<string> itemList);

    }
}
