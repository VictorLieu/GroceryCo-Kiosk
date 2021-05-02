using System.Collections.Generic;

namespace GroceryKiosk
{
    public interface IScanningService
    {

        List<string> scanItems(string filepath);
    }
}