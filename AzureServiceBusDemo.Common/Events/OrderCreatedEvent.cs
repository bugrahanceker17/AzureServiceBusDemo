using System;

namespace AzureServiceBusDemo.Common.Events
{
    public class OrderCreatedEvent
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ProductName { get; set; }
    }
}