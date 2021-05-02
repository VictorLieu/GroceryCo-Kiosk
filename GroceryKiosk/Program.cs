using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GroceryKiosk.src;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GroceryKiosk
{
    partial class Program
    {

        // This class uses the Main function as a driver for the program
        static void Main(string[] args)
        {

            // set up serilog connection to config file with builder
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            // set up the logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            // use Mictosoft Extensions Hosting to create a default builder
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // get instances 
                    services.AddTransient<IScanningService, ScanningService>();
                    services.AddTransient<IPriceCalculationService, PriceCalculationService>();
                })
                .UseSerilog()
                .Build();

            // get instances from service provider
            var scanningService = ActivatorUtilities.GetServiceOrCreateInstance<IScanningService>(host.Services);
            var priceCalculationService = ActivatorUtilities.GetServiceOrCreateInstance<IPriceCalculationService>(host.Services);

            // Begin checkout process
            Log.Logger.Information("Beginning checkout...");

            try
            {
                string filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\data\\cart.txt";
                List<string> cartItems = scanningService.scanItems(filePath);
                string catalogPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\data\\priceCatalog.json";
                priceCalculationService.initCatalogs(catalogPath);
                double totalPrice = priceCalculationService.calculateTotal(cartItems);
                Hashtable subTotals = priceCalculationService.formatItems(cartItems);
                ReceiptService.printReceipt(subTotals, totalPrice);
            }
            catch
            {
                Console.WriteLine("Uh oh, something went wrong while checking out. Please close the terminal and try again.");
            }


        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                // add appsettings.json to build configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }


    }
}
