using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Collections.Generic;

namespace GroceryKiosk
{
    public class ScanningService : IScanningService
    {
        private readonly ILogger<IScanningService> _log;

        public ScanningService(ILogger<IScanningService> log)
        {
            _log = log;
        }

        public List<string> scanItems(string filepath)
        {

            List<string> cartItems = new List<string>();
            try
            {
                using (StreamReader r = new StreamReader(filepath))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        cartItems.Add(line.ToLower());
                        _log.LogInformation("Item scanned: " + line);
                    }
                }
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

            return cartItems;

        }

    }
    
}
