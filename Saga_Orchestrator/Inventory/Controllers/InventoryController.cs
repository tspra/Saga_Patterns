using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {

        [HttpPost]
        public int Post([FromBody] InventoryItem inventoryItem)
        {
            throw new Exception("Error while updating inventory");
            Console.WriteLine($"Updated inventory for  :{inventoryItem.ProductName}");
            return 2;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Console.WriteLine($"Deleted inventory :{id}");
        }
    }
}
