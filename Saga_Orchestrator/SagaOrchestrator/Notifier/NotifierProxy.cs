using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SagaOrchestrator.Notifier
{
    public class NotifierProxy : INotifierProxy
    {
        private readonly IHttpClientFactory httpClientFactory;
        public NotifierProxy(IHttpClientFactory factory)
        {
            httpClientFactory = factory;
        }
        public async Task<(int, bool)> Send(OrderItem order)
        {
            try
            {
                var request = JsonConvert.SerializeObject(order);
                var notificationclient = httpClientFactory.CreateClient("Notifer");
                var notifierResponse = await notificationclient.PostAsync("api/Notifier",
                     new StringContent(request, Encoding.UTF8, "application/JSON"));
                var notifierId = await notifierResponse.Content.ReadAsStringAsync();
                return (Convert.ToInt32(notifierId), true);
            }
            catch (Exception ex)
            {
                return (-1, false);
            }
        }
    }
}
