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


        [HttpPost]
        public async Task CreateOrder(OrderDto model)
        {
            
            OrderCreatedEvent orderCreatedEvent = new OrderCreatedEvent
            {
                Id = model.Id,
                CreatedAt = DateTime.Now,
                ProductName = model.ProductName
            };
            await _azureService.SendMessageToQueue(Constants.OrderCreatedQueueName, orderCreatedEvent);
        }
    }
}