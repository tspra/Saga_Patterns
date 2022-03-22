using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SagaOrchestrator.Inventory
{
    public class InventoryProxy : IInventoryProxy
    {
        private readonly IHttpClientFactory httpClientFactory;
        public InventoryProxy(IHttpClientFactory factory)
        {
            httpClientFactory = factory;
        }

        public void Delete(int orderId)
        {
            var orderclient = httpClientFactory.CreateClient("Inventory");
            var response = orderclient.DeleteAsync($"api/Order/{orderId}").Result;
            Console.WriteLine("Inventory Deleted");
        }

        public async Task<(int, bool)> UpdateInventory(OrderItem order)
        {
            try
            {
                var request = JsonConvert.SerializeObject(order);
                var inventoryclient = httpClientFactory.CreateClient("Inventory");
                var inventoryResponse = await inventoryclient.PostAsync("api/Inventory",
                      new StringContent(request, Encoding.UTF8, "application/JSON"));
                if (inventoryResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(inventoryResponse.ReasonPhrase);
                }
                var inventoryId = await inventoryResponse.Content.ReadAsStringAsync();
                return (Convert.ToInt32(inventoryId), true);
            }
            catch (Exception ex)
            {
                return (-1, false);
            }
        }
    }
}
