
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService
{
    public class OrderDeletor : IOrderDeletor
    {
        public async Task Delete(int orderId)
        {
            // delete from database.
        }
    }
}
