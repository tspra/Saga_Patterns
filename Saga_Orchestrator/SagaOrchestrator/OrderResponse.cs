using System;

namespace SagaOrchestrator
{
    public class OrderResponse
    {
      public string OrderId { get; set; }
        public string Status { get; set; }
        public string Reason { get;  set; }
        public bool Success { get; internal set; }
    }
}
