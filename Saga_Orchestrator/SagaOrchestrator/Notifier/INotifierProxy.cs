using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaOrchestrator.Notifier
{
    public interface INotifierProxy
    {
        Task<(int, bool)> Send(OrderItem order);
    }
}
