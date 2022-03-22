using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
