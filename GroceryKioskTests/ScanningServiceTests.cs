using GroceryKiosk;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace GroceryKioskTests
{
    public class ScanningServiceTests
    {
        private ScanningService scanningService;

        [Test]
        public void scanItems_one_duplicate()
        {
            // Arrange
            var mock = new Mock<ILogger<IScanningService>>();
            ILogger<IScanningService> logger = mock.Object;
            scanningService = new ScanningService(logger);
            string filepath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\testData\\cartTestDuplicate.txt";
            List<string> correctList = new List<string>(new string[] { "apple", "banana", "apple", "orange" });

            // Act
            List<string> itemList = scanningService.scanItems(filepath);

            // Assert
            CollectionAssert.AreEqual(correctList, itemList);
        }

        [Test]
        public void scanItems_empty_cart()
        {
            // Arrange
            var mock = new Mock<ILogger<IScanningService>>();
            ILogger<IScanningService> logger = mock.Object;
            scanningService = new ScanningService(logger);
            string filepath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\testData\\cartTestEmpty.txt";
            List<string> correctList = new List<string>(new string[] { });

            // Act
            List<string> itemList = scanningService.scanItems(filepath);

            // Assert
            CollectionAssert.AreEqual(correctList, itemList);
        }

    }
}