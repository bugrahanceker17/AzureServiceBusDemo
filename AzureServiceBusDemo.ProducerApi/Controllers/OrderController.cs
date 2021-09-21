using System;
using System.Threading.Tasks;
using AzureServiceBusDemo.Common;
using AzureServiceBusDemo.Common.Dto;
using AzureServiceBusDemo.Common.Events;
using AzureServiceBusDemo.ProducerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureServiceBusDemo.ProducerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AzureService _azureService;

        public OrderController(AzureService azureService)
        {
            _azureService = azureService;
        }


        [HttpPost("queue")]
        public async Task CreateOrderToQueue(OrderDto model)
        {
            OrderCreatedEvent orderCreated = new()
            {
                Id = model.Id,
                CreatedAt = DateTime.Now,
                ProductName = model.ProductName
            };
            await _azureService.CreateQueueIfNotExists(Constants.OrderCreatedQueueName);
            await _azureService.SendMessageToQueue(Constants.OrderCreatedQueueName, orderCreated);
        }

        [HttpPost("topic")]
        public async Task CreateOrderToTopic(OrderDto model)
        {
            OrderCreatedEvent orderCreated = new()
            {
                Id = model.Id,
                CreatedAt = DateTime.Now,
                ProductName = model.ProductName
            };
            await _azureService.CreateTopicIfNotExists(Constants.OrderTopic);
            await _azureService.SendMessagesToTopic(Constants.OrderTopic, orderCreated);
        }

        [HttpDelete("{id:int}")]
        public async Task DeleteOrder(int id)
        {
            OrderDeletedEvent orderDeleted = new()
            {
                Id = id,
                CreatedAt = DateTime.Now
            };
            await _azureService.CreateQueueIfNotExists(Constants.OrderDeletedQueueName);
            await _azureService.SendMessageToQueue(Constants.OrderDeletedQueueName, orderDeleted);
        }
    }
}