using System;

namespace AzureServiceBusDemo.Common.Events
{
    public class EventBase
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}