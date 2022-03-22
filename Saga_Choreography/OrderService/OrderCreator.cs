
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService
{
    public class OrderCreator : IOrderCreator
    {
        private readonly ILogger<OrderCreator> logger;
        public OrderCreator(ILogger<OrderCreator> logger)
        {
            this.logger = logger;
        }
        public async Task<int> Create(OrderDetail orderDetail)
        {
            // Save into database and returun the order id;
            return 1;
        }
    }
}
