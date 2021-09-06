using System.Text;
using System.Threading.Tasks;
using AzureServiceBusDemo.Common;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace AzureServiceBusDemo.ProducerApi.Services
{
    public class AzureService
    {
        public async Task SendMessageToQueue(string queueName, object messageContent)
        {
            IQueueClient queueClient = new QueueClient(Constants.ConnectionString, queueName);
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
            Message message = new(byteArray);
            await queueClient.SendAsync(message);
        }
    }
}