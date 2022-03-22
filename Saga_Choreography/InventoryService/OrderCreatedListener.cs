
using Microsoft.Extensions.Hosting;
using Models;
using Newtonsoft.Json;
using Plain.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InventoryService
{
    public class OrderCreatedListener : IHostedService
    {
        private readonly IPublisher publisher;
        private readonly ISubscriber subscriber;
        private readonly IInventoryUpdater inventoryUpdater;
        public OrderCreatedListener(IPublisher publisher, ISubscriber subscriber, IInventoryUpdater inventoryUpdater)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.inventoryUpdater = inventoryUpdater;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            subscriber.Subscribe(Subscribe);
            return Task.CompletedTask;
        }
        private bool Subscribe(string message, IDictionary<string, object> header)
        {
            var response = JsonConvert.DeserializeObject<OrderRequest>(message);
            try
            {
                inventoryUpdater.Update(response.ProductId, response.Quantity).GetAwaiter().GetResult();
                publisher.Publish(JsonConvert.SerializeObject(
                    new InventoryResponse { OrderId = response.OrderId, IsSuccess = true }
                    ), "inventory.response", null);
            }
            catch (Exception)
            {
                publisher.Publish(JsonConvert.SerializeObject(
                    new InventoryResponse { OrderId = response.OrderId, IsSuccess = false }
                    ), "inventory.response", null);
            }

            return true;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
