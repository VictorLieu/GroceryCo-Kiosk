using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using GroceryKiosk.utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using GroceryKiosk.src;

namespace GroceryKiosk
{
    public class PriceCalculationService : IPriceCalculationService
    {

        private double totalPrice;
        private Hashtable regularCatalog;
        private Hashtable saleCatalog;
        private Hashtable cartItems;

        private readonly ILogger<IPriceCalculationService> _log;


        public PriceCalculationService(ILogger<IPriceCalculationService> log)
        {
            totalPrice = 0;
            regularCatalog = new Hashtable();
            saleCatalog = new Hashtable();
            cartItems = new Hashtable();
            _log = log;
        }

        // getters and setters for catalogs
        public Hashtable getRegularCatalog()
        {
            return regularCatalog;
        }

        public Hashtable getSaleCatalog()
        {
            return saleCatalog;
        }

        public void setRegularCatalog(Hashtable value)
        {
            regularCatalog = value;
        }

        public void setSaleCatalog(Hashtable value)
        {
            saleCatalog = value;
        }
        

        // Function used to initialize hashtables 
        // Each hashtable holds every item in the catalog one is for regular priced items and the other is for items that are on sale
        public void initCatalogs(string catalogPath)
        {
            try
            {
                using (StreamReader r = new StreamReader(catalogPath))
                {
                    string json = r.ReadToEnd();
                    // Since the catalog is a JSON file we desearlize it into a list of type Item
                    List<Item> catalogItems = JsonConvert.DeserializeObject<List<Item>>(json);
                    foreach (Item item in catalogItems)
                    {
                        regularCatalog.Add(item.name, item.price);
                        // if there is a sale price for the item in the catalog add to saleCatalog
                        if (item.salePrice != "N/A")
                        {
                            saleCatalog.Add(item.name, item.salePrice);
                        }

                    }
                };
            }
            catch (FileNotFoundException e)
            {
                _log.LogError(e.ToString());
                throw e;
            }
            catch (Exception e)
            {
                _log.LogError(e.ToString());
                throw e;
            }

        }

        // Function used to calculate the total cost of all items in the cart
        public double calculateTotal(List<string> itemList)
        {
            List<string> items = itemList;

            foreach (String item in items)
            {
                if (saleCatalog.ContainsKey(item))
                {
                    totalPrice += StringUtils.parsePrice((string)saleCatalog[item]);
                }
                else
                {
                    totalPrice += StringUtils.parsePrice((string)regularCatalog[item]);
                }        
            }
            return totalPrice;
        }

        // Function used to create and modify Item objects based on list of items in cart
        public Hashtable formatItems(List<string> itemsList)
        {
            foreach (String item in itemsList)
            {
                // if the hashtable already contains the item update the quantity of the object value mapped to the key
                if (cartItems.ContainsKey(item))
                {
                    (cartItems[item] as Item).quantity += 1;
                }
                // if the hashtable does not contain the item add a new entry where the value is an object representing that Item
                else
                {
                    if (saleCatalog.ContainsKey(item))
                    {
                        string regularPrice = ((string)regularCatalog[item]);
                        string salePrice = ((string)saleCatalog[item]);
                        Item itemObject = new Item(item, regularPrice, salePrice, 1);
                        cartItems.Add(item, itemObject);
                    }
                    else
                    {
                        string regularPrice = ((string)regularCatalog[item]);
                        Item itemObject = new Item(item, regularPrice, "N/A", 1);
                        cartItems.Add(item, itemObject);
                    }
                } 
            }
            return cartItems; 
        }
    }  
}
