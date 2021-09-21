using System;
using System.Text;
using System.Threading.Tasks;
using AzureServiceBusDemo.Common;
using AzureServiceBusDemo.Common.Events;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace AzureServiceBusDemo.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsumeQueue<OrderCreatedEvent>(Constants.OrderCreatedQueueName, i =>
            {
                Console.WriteLine($"OrderCreatedEvent ReceivedMessage with Id: {i.Id}, Name: {i.ProductName}");
            }).Wait();
            
            ConsumeQueue<OrderDeletedEvent>(Constants.OrderDeletedQueueName, i =>
            {
                Console.WriteLine($"OrderDeletedEvent ReceivedMessage with Id: {i.Id}");
            }).Wait();
            Console.WriteLine("\n\n\n");
            Console.ReadLine();
        }

        private static async Task ConsumeQueue<T>(string queueName, Action<T> receivedAction)
        {
            IQueueClient client = new QueueClient(Constants.ConnectionString, queueName);
            client.RegisterMessageHandler(async (message, ct) =>
            {
                var model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
                receivedAction(model);
                await Task.CompletedTask;
            }, new MessageHandlerOptions(c=>Task.CompletedTask));
            
            Console.WriteLine($"{typeof(T)} is listening now...");
        }
    }
}