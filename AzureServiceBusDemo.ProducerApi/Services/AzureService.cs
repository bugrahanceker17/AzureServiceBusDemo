using System.Text;
using System.Threading.Tasks;
using AzureServiceBusDemo.Common;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace AzureServiceBusDemo.ProducerApi.Services
{
    public class AzureService
    {
        private readonly ManagementClient _managementClient;

        public AzureService(ManagementClient managementClient)
        {
            _managementClient = managementClient;
        }

        public async Task SendMessageToQueue(string queueName, object messageContent)
        {
            IQueueClient queueClient = new QueueClient(Constants.ConnectionString, queueName);
            await SendMessage(queueClient, messageContent);
        }

        public async Task CreateQueueIfNotExists(string queueName)
        {
            if (!await _managementClient.QueueExistsAsync(queueName))
                await _managementClient.CreateQueueAsync(queueName);
        }

        public async Task SendMessagesToTopic(string topicName, object messageContent)
        {
            ITopicClient client = new TopicClient(Constants.ConnectionString, topicName); 
            await SendMessage(client, messageContent);
        }

        public async Task CreateTopicIfNotExists(string topicName)
        {
            if (!await _managementClient.TopicExistsAsync(topicName))
                await _managementClient.CreateQueueAsync(topicName);
        }

        private async Task SendMessage(ISenderClient senderClient, object messageContent)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
            Message message = new Message(byteArray);
            await senderClient.SendAsync(message);
        }
    }
}