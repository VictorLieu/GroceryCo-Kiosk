using System;
using System.Collections.Generic;
using GroceryKiosk;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.IO;
using GroceryKiosk.src;
using System.Collections;

namespace GroceryKioskTests
{
    class PriceCalculationServiceTests
    {

        private PriceCalculationService priceCalculationService;

        [Test]
        public void init_catalog_no_sale_banana()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            string catalogPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\testData\\catalogTestNoSaleBanana.json";
            Hashtable correctPriceCatalog = new Hashtable() 
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable correctSaleCatalog = new Hashtable(){
                { "apple", "$2.00" },
                { "orange", "$3.00" }
            };

            // Act
            priceCalculationService.initCatalogs(catalogPath);

            // Assert
            CollectionAssert.AreEqual(correctPriceCatalog, priceCalculationService.getRegularCatalog());
            CollectionAssert.AreEqual(correctSaleCatalog, priceCalculationService.getSaleCatalog());
        }

        [Test]
        public void init_catalog_no_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            string catalogPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\testData\\catalogTestNoSales.json";
            Hashtable correctPriceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable correctSaleCatalog = new Hashtable(){};

            // Act
            priceCalculationService.initCatalogs(catalogPath);

            // Assert
            CollectionAssert.AreEqual(correctPriceCatalog, priceCalculationService.getRegularCatalog());
            CollectionAssert.AreEqual(correctSaleCatalog, priceCalculationService.getSaleCatalog());
        }

        [Test]
        public void init_catalog_all_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            string catalogPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\testData\\catalogTestAllSales.json";
            Hashtable correctPriceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable correctSaleCatalog = new Hashtable() 
            {
                { "apple", "$2.00" },
                { "orange", "$3.00" },
                { "banana", "$3.50" }
            };

            // Act
            priceCalculationService.initCatalogs(catalogPath);

            // Assert
            CollectionAssert.AreEqual(correctPriceCatalog, priceCalculationService.getRegularCatalog());
            CollectionAssert.AreEqual(correctSaleCatalog, priceCalculationService.getSaleCatalog());
        }

        [Test]
        public void calculateTotal_mixed_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            Hashtable priceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable saleCatalog = new Hashtable()
            {
                { "apple", "$2.00" },
                { "banana", "$3.50" }
            };
            priceCalculationService.setRegularCatalog(priceCatalog);
            priceCalculationService.setSaleCatalog(saleCatalog);
            List<string> inputList = new List<string>(new string[] { "apple", "banana", "orange" });
            double expectedTotal = 9.00;

            // Act
            double total = priceCalculationService.calculateTotal(inputList);

            // Assert
            Assert.AreEqual(expectedTotal, total);
        }

        [Test]
        public void calculateTotal_all_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            Hashtable priceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable saleCatalog = new Hashtable()
            {
                { "apple", "$2.00" },
                { "orange", "$3.00" },
                { "banana", "$3.50" }
            };
            priceCalculationService.setRegularCatalog(priceCatalog);
            priceCalculationService.setSaleCatalog(saleCatalog);
            List<string> inputList = new List<string>(new string[] { "apple", "banana", "orange" });
            double expectedTotal = 8.50;

            // Act
            double total = priceCalculationService.calculateTotal(inputList);

            // Assert
            Assert.AreEqual(expectedTotal, total);
        }

        [Test]
        public void calculateTotal_no_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            Hashtable priceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable saleCatalog = new Hashtable()
            {

            };
            priceCalculationService.setRegularCatalog(priceCatalog);
            priceCalculationService.setSaleCatalog(saleCatalog);
            List<string> inputList = new List<string>(new string[] { "apple", "banana", "orange" });
            double expectedTotal = 10.00;

            // Act
            double total = priceCalculationService.calculateTotal(inputList);

            // Assert
            Assert.AreEqual(expectedTotal, total);
        }

        [Test]
        public void formatItems_mixed_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            Hashtable priceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable saleCatalog = new Hashtable()
            {
                { "apple", "$2.00" },
                { "banana", "$3.50" }
            };
            priceCalculationService.setRegularCatalog(priceCatalog);
            priceCalculationService.setSaleCatalog(saleCatalog);
            List<string> inputList = new List<string>(new string[] { "apple", "banana", "orange" });
            Item apple = new Item("apple","$2.50","$2.00",1);
            Item orange = new Item("orange", "$3.50", "N/A", 1);
            Item banana = new Item("banana", "$4.00", "$3.50", 1);
            Hashtable expectedItems = new Hashtable()
            {
                { "apple", apple },
                { "orange", orange },
                { "banana", banana }
            };


            // Act
            Hashtable items = priceCalculationService.formatItems(inputList);

            // Assert
            foreach (DictionaryEntry entry in items)
            {
                string itemName = ((entry.Value as Item).name);
                Assert.AreEqual(((entry.Value as Item).name), ((expectedItems[itemName] as Item).name));
                Assert.AreEqual(((entry.Value as Item).price), ((expectedItems[itemName] as Item).price));
                Assert.AreEqual(((entry.Value as Item).salePrice), ((expectedItems[itemName] as Item).salePrice));
                Assert.AreEqual(((entry.Value as Item).quantity), ((expectedItems[itemName] as Item).quantity));
            }
            
        }

        [Test]
        public void formatItems_all_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            Hashtable priceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable saleCatalog = new Hashtable()
            {
                { "apple", "$2.00" },
                { "orange", "$3.00" },
                { "banana", "$3.50" }
            };
            priceCalculationService.setRegularCatalog(priceCatalog);
            priceCalculationService.setSaleCatalog(saleCatalog);
            List<string> inputList = new List<string>(new string[] { "apple", "banana", "orange" });
            Item apple = new Item("apple", "$2.50", "$2.00", 1);
            Item orange = new Item("orange", "$3.50", "$3.00", 1);
            Item banana = new Item("banana", "$4.00", "$3.50", 1);
            Hashtable expectedItems = new Hashtable()
            {
                { "apple", apple },
                { "orange", orange },
                { "banana", banana }
            };


            // Act
            Hashtable items = priceCalculationService.formatItems(inputList);

            // Assert
            foreach (DictionaryEntry entry in items)
            {
                string itemName = ((entry.Value as Item).name);
                Assert.AreEqual(((entry.Value as Item).name), ((expectedItems[itemName] as Item).name));
                Assert.AreEqual(((entry.Value as Item).price), ((expectedItems[itemName] as Item).price));
                Assert.AreEqual(((entry.Value as Item).salePrice), ((expectedItems[itemName] as Item).salePrice));
                Assert.AreEqual(((entry.Value as Item).quantity), ((expectedItems[itemName] as Item).quantity));
            }

        }

        [Test]
        public void formatItems_no_sales()
        {
            // Arrange
            var mock = new Mock<ILogger<IPriceCalculationService>>();
            ILogger<IPriceCalculationService> logger = mock.Object;
            priceCalculationService = new PriceCalculationService(logger);
            Hashtable priceCatalog = new Hashtable()
            {
                { "apple", "$2.50" },
                { "orange", "$3.50" },
                { "banana", "$4.00" }
            };
            Hashtable saleCatalog = new Hashtable()
            {

            };
            priceCalculationService.setRegularCatalog(priceCatalog);
            priceCalculationService.setSaleCatalog(saleCatalog);
            List<string> inputList = new List<string>(new string[] { "apple", "banana", "orange" });
            Item apple = new Item("apple", "$2.50", "N/A", 1);
            Item orange = new Item("orange", "$3.50", "N/A", 1);
            Item banana = new Item("banana", "$4.00", "N/A", 1);
            Hashtable expectedItems = new Hashtable()
            {
                { "apple", apple },
                { "orange", orange },
                { "banana", banana }
            };


            // Act
            Hashtable items = priceCalculationService.formatItems(inputList);

            // Assert
            foreach (DictionaryEntry entry in items)
            {
                string itemName = ((entry.Value as Item).name);
                Assert.AreEqual(((entry.Value as Item).name), ((expectedItems[itemName] as Item).name));
                Assert.AreEqual(((entry.Value as Item).price), ((expectedItems[itemName] as Item).price));
                Assert.AreEqual(((entry.Value as Item).salePrice), ((expectedItems[itemName] as Item).salePrice));
                Assert.AreEqual(((entry.Value as Item).quantity), ((expectedItems[itemName] as Item).quantity));
            }

        }

    }


}
