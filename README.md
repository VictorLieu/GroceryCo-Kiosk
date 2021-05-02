# Design
This checkout system was designed to be run by a driver class `Program.cs`. The driver executes each step of the checkout process in the following order: scan items, perform price calculations, print receipt to console. Since there are three main functionalities, the checkout system has been split into three services: `ScanningService`, `PriceCalculationService`, and `ReceiptService`. 


### Scanning (ScanningService)
The ScanningService reads in data from a text file which is supposed to represent a cart holding items that will be checked out. 
The text file is assumed to be named `cart.txt`, and is stored under `/data`. An example of the contents of `cart.txt` can be seen below:
```
Apple
Banana
Apple
Orange
```


### Price Calculation (PriceCalculationService)
The PriceCalculation service mainly focuses on calculating the total price of all cart items that were scanned, as well as the creation of objects used to represent each cart item (to be used by the ReceiptService). After the items are scanned, the total price of all items in the cart is calculated. The program does this by referencing prices that come from the `priceCatalog.json` file, which is formatted as follows: 
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
The catalog can be extended by adding new JSON objects to this file. JSON was chosen to store catalog values to mimic the idea that catalog data could potentially be retrieved from a REST API in a real application/scenario. This catalog is abstracted into the program as two hashtables, one hashtable contains item names as keys and regular prices as values, while the other also contains item names as keys but has sale prices as its values. This is done to help map each catalog item to its respective regular and sale price (ex. the hashtable storing sale prices may contain an entry with a key value of "apple" and a price of "$2.00"). The PriceCalculation service also creates objects of the type **`Item`** and each object represents a type of item in the cart. This is done to associate each item type in the cart with an object that holds the appropriate data for it. Each of these objects contains properties such as price, salePrice, and quantity of the item in the cart. For example, if two apples were scanned from the cart, an apple object will be initialized with the following fields (assume we are following the price catalog above): </br>
* name: "apple"
* price: "$2.50"
* salePrice: "$2.00"
* quantity: 2 
</br>


### Printing the receipt (ReceiptService)
The final step in the process is the print the receipt out. A simple receipt containing:  item quantity, regular price, and the sale price of each type of item that was checked out, along with the total amount due. Logging messages are also printed to the console when each item was scanned (via Serilog). 
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
Source code for the project exists under `/src`, and data (item catalog, cart text file) exists under `/data`. A `StringUtils.cs` class which contains static methods can be used to format price strings. This can be found under `/utils`. Some unit tests utilizing the NUnit framework have also been added to test functionalities against different input combinations (ex. duplicates of an item). These tests can be found under `/GroceryKioskTests`. The following NuGet packages have also been added to the project: </br>
* **Microsoft.Extensions.Hosting** - Used for startup configurations
* **Serilog** - For logging information during the checkout process, and also logging errors to console when exceptions are thrown
* **Newtonsoft.Json** - Used to desearlize `priceCatalog.json` file
* **NUnit** - Used as the unit testing framework
* **Moq** - Used to mock objects for unit tests


# Limitations/Assumptions
* Prices in catalog are non-negative and > $0.00
* A text file named `cart.txt` exists under the data directory containing items to be checked out
* The users cart is not empty (`cart.txt` is not empty, and contains at least one item)
* All items in the cart exist in the catalog
* A JSON file named `priceCatalog.json` exists under the data directory
* When a catalog item is not on sale, the salePrice value will be `N/A`
* If a sale price exists for the item, it will show up on the receipt
* If a sale price exists for the item, it will be considered as the effective price of the item
* Invalid filepath/file name exceptions will be logged to console as errors and the program will terminate
* cart and priceCatalog files for tests are located under `/testData`
