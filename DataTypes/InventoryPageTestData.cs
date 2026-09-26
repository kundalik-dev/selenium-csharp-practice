using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium_CSharp_Practice.DataTypes
{
    public class InventoryPageTestData
    {
        public List<InventoryProductTestData> Products { get; set; } = [];
    }

    public class InventoryProductTestData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductPrice { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
    }
}
