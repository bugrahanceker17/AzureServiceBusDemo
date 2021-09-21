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

        public async Task SendMessageToQueue(string queueName, object messageContent, string messageType = null)
        {
            IQueueClient queueClient = new QueueClient(Constants.ConnectionString, queueName);
            await SendMessage(queueClient, messageContent, messageType);
        }

        public async Task CreateQueueIfNotExists(string queueName)
        {
            if (!await _managementClient.QueueExistsAsync(queueName))
                await _managementClient.CreateQueueAsync(queueName);
        }

        public async Task SendMessagesToTopic(string topicName, object messageContent, string messageType = null)
        {
            ITopicClient client = new TopicClient(Constants.ConnectionString, topicName);
            await SendMessage(client, messageContent, messageType);
        }

        public async Task CreateSubscriptionIfNotExists(string topicName, string subscriptionName, string messageType = null, string ruleName = null)
        {
            if (await _managementClient.SubscriptionExistsAsync(topicName, subscriptionName))
                return;

            if (messageType != null)
            {
                SubscriptionDescription sd = new(topicName, subscriptionName);
                CorrelationFilter filter = new();
                filter.Properties["MessageType"] = messageType;

                RuleDescription rd = new(ruleName ?? $"{messageType}+Rule", filter);
                await _managementClient.CreateSubscriptionAsync(sd, rd);
            }
        }

        public async Task CreateTopicIfNotExists(string topicName)
        {
            if (!await _managementClient.TopicExistsAsync(topicName))
                await _managementClient.CreateTopicAsync(topicName);
        }

        private async Task SendMessage(ISenderClient senderClient, object messageContent, string messageType = null)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
            Message message = new(byteArray);
            message.UserProperties["MessageType"] = messageType;
            await senderClient.SendAsync(message);
        }
    }
}