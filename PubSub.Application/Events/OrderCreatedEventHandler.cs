using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PubSub.Application.Common;
using PubSub.Domain.Events;

namespace PubSub.Application.Events
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(OrderCreatedEvent @event)
        {
            _logger.LogInformation("Order created: {OrderId} for user {UserId} with amount {Amount}",
                @event.OrderId, @event.UserId, @event.Amount);

            _logger.LogInformation("Processing payment and shipping for order {OrderId}", @event.OrderId);

            return Task.CompletedTask;
        }
    }
}