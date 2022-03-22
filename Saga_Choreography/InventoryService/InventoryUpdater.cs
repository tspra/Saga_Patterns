using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryService
{
    public class InventoryUpdater : IInventoryUpdater
    {
        
        Task IInventoryUpdater.Update(int orderId, int quanity)
        {
            // Update the inventory database;
            return Task.CompletedTask;
        }
    }
}
