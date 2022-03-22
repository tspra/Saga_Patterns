using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using Plain.RabbitMQ;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IPublisher publisher;
        private readonly IOrderCreator orderCreator;

        public OrderController( IPublisher publisher, IOrderCreator orderCreator)
        {
            this.publisher = publisher;
            this.orderCreator = orderCreator;
        }

        [HttpPost]
        public async Task Post([FromBody] OrderDetail orderDetail)
        {
            var id = await orderCreator.Create(orderDetail);
            publisher.Publish(JsonConvert.SerializeObject(new OrderRequest { 
                OrderId = id,
                ProductId = orderDetail.ProductId,
                Quantity = orderDetail.Quantity
            }), "order.created", null);
        }

    }
}
