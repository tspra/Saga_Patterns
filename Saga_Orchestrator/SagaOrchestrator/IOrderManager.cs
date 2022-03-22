using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaOrchestrator
{
    public interface IOrderManager
    {
        public bool CreateOrder(OrderItem order);
    }
}
