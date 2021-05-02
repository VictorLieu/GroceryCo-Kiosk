# Design
This checkout system was designed to be run by a driver class (Program.cs). The driver executes each step of the 
checkout process in the following order: scan items, do price calculations, print receipt to console. The functionality of this checkout system has been split into three services: ScanningService, PriceCalculationService, and the ReceiptService. 

### Scanning (ScanningService)
A simple text file named "cart.txt" is read from, which represents the users cart (this is essentially the scanning phase). 
```
Apple
Banana
Apple
Orange
```

### Price Calculation (PriceCalculationService)
After the items are scanned, the total price of all items in the cart is calculated. The program does this by referrencing prices which come from the "priceCatalog.json" file, which is formatted in the following way: 
```
[
  {
    "name": "apple",
    "price": "$2.50",
    "salePrice": "$2.00"
  },

  {
    "name": "orange",
    "price": "$3.50",
    "salePrice": "$3.00"
  },

  {
    "name": "banana",
    "price": "$4.00",
    "salePrice": "N/A"
  }
]
```
The catalog can be extended by adding new JSON objects to this file. This catalog is abstracted into the program as hashtables to help each catalog item to its respective prices. The PriceCalculation service also creates an object representing each item in the cart, which contains properties such as price, salePrice and quantity of the item in the cart. 

### Printing the receipt (ReceiptService)
The final step in the process is the print the receipt out. A simple receipt containing the total amount due, each item name, item quantity, regular price, and sale price of each item is shown. Logging messages are also printed to the console when each item was scanned (via Serilog). 
```
[12:34:20 INF] Beginning checkout...
[12:34:20 INF] Item scanned: Apple
[12:34:20 INF] Item scanned: Banana
[12:34:20 INF] Item scanned: Apple
[12:34:20 INF] Item scanned: Orange
==============================
orange
Quantity: 1
Regular Price: $3.50
Applied sale price: $3.00
==============================
banana
Quantity: 1
Regular Price: $4.00
==============================
apple
Quantity: 2
Regular Price: $2.50
Applied sale price: $2.00
==============================
Total: $11.00
```

# Additional Notes
Some unit tests utilizing the NUnit framework have also been added to test the functionalities of certain features in this checkout system. The following NuGet packages have also been added to the project: </br>
* **Microsoft.Extensions.Hosting** - Used for startup configurations
* **Serilog** - For logging information during checkout process, and also logging errors to cosole when exceptions are thrown
* **Newtonsoft.Json** - Used to desearlize itemCatalog.json file


# Assumptions
* A text file named "cart.txt" exists under the data directory containing items to be checked out
* The users cart is not empty (cart.txt is not empty)
* All items in the cart exist in the catalog
* A JSON file named "itemCatalog.json" exists under the data directory
* When a catalog item is not on sale, the salePrice value will be "N/A"
