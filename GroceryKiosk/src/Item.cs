namespace GroceryKiosk
{
    public class Item
    {
        public string name { get; set; }
        public string price { get; set; }
        public string salePrice { get; set; }
        public int quantity { get; set; }

        public Item (string itemName, string itemPrice, string itemSalePrice, int itemQuantity)
        {
            name = itemName;
            price = itemPrice;
            salePrice = itemSalePrice;
            quantity = itemQuantity;

        }

    }

   
    
}
