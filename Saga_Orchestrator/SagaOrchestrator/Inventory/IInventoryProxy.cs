using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaOrchestrator.Inventory
{
    public interface IInventoryProxy
    {
        Task<(int, bool)> UpdateInventory(OrderItem order);
        void Delete(int orderId);
    }
}
