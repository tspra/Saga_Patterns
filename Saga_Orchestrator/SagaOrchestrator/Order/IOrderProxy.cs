using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaOrchestrator.Order
{
    public interface IOrderProxy
    {
         Task<(int, bool)> CreateOrder(OrderItem order);

        void  DeleteOrder(int orderId);
    }
}
