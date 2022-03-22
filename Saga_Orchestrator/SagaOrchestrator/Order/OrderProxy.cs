using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SagaOrchestrator.Order
{
    public class OrderProxy : IOrderProxy
    {
        private readonly IHttpClientFactory httpClientFactory;
        public OrderProxy(IHttpClientFactory factory)
        {
            httpClientFactory = factory;
        }
      
        public async Task<(int, bool)> CreateOrder(OrderItem order)
        {
            try
            {
                var request = JsonConvert.SerializeObject(order);
                var orderclient = httpClientFactory.CreateClient("Order");
                var orderResponse = await orderclient.PostAsync("api/Order",
                    new StringContent(request, Encoding.UTF8, "application/JSON"));
                var orderId = await orderResponse.Content.ReadAsStringAsync();
                return (Convert.ToInt32(orderId), true);
            }
            catch(Exception ex)
            {
                return (-1, false);
            }
        }

        public void DeleteOrder(int orderId)
        {
            var orderclient = httpClientFactory.CreateClient("Order");
            var response =  orderclient.DeleteAsync($"api/Order/{orderId}").Result;
            Console.WriteLine("Order Deleted");
        }
    }
}
