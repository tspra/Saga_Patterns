using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SagaOrchestrator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOrderManager orderManager;
        public OrderController(IHttpClientFactory factory,IOrderManager orderManager)
        {
            httpClientFactory = factory;
            this.orderManager = orderManager;
        }
        [HttpPost]
        public async Task<OrderResponse> Post([FromBody] OrderItem order)
        {

            var response = this.orderManager.CreateOrder(order);
            return new OrderResponse { Success = response };

            //string inventoryId = string.Empty;
            //var request = JsonConvert.SerializeObject(order);
            //var orderclient = httpClientFactory.CreateClient("Order");
            //var orderResponse = await orderclient.PostAsync("api/Order",
            //    new StringContent(request, Encoding.UTF8, "application/JSON"));
            //var orderId = await orderResponse.Content.ReadAsStringAsync();


            //try
            //{
            //    var inventoryclient = httpClientFactory.CreateClient("Inventory");
            //    var inventoryResponse = await inventoryclient.PostAsync("api/Inventory",
            //          new StringContent(request, Encoding.UTF8, "application/JSON"));
            //    if(inventoryResponse.StatusCode != System.Net.HttpStatusCode.OK)
            //    {
            //        throw new Exception(inventoryResponse.ReasonPhrase);
            //    }
            //    inventoryId = await inventoryResponse.Content.ReadAsStringAsync();
            //}
            //catch(Exception ex)
            //{
            //    await orderclient.DeleteAsync($"api/Order/{orderId}");
            //    return new OrderResponse
            //    {
            //        Status = "Failed",
            //        Reason = ex.Message
            //    };
            //}


            //var notificationclient = httpClientFactory.CreateClient("Notifer");
            //var notifierResponse =  await notificationclient.PostAsync("api/Notifier",
            //     new StringContent(request, Encoding.UTF8, "application/JSON"));

            //var notifierId = await notifierResponse.Content.ReadAsStringAsync();
            //return new OrderResponse { OrderId = orderId };
        }
    }
}
