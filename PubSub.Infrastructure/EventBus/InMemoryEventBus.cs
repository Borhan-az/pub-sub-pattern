using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PubSub.Application.Common;
using PubSub.Domain.Events;

namespace PubSub.Infrastructure.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly EventSubscriptionManager _subscriptionManager;
        private readonly ILogger<InMemoryEventBus> _logger;

        public InMemoryEventBus(
            EventSubscriptionManager subscriptionManager,
            ILogger<InMemoryEventBus> logger)
        {
            _subscriptionManager = subscriptionManager;
            _logger = logger;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventType = @event.GetType();
            _logger.LogInformation("Publishing event {EventId} of type {EventType}", @event.Id, eventType.Name);

            if (_subscriptionManager.HasSubscriptionsForEvent(eventType))
            {
                Task.Run(async () => await _subscriptionManager.ProcessEvent(@event));
            }
            else
            {
                _logger.LogWarning("No subscribers registered for event {EventId} of type {EventType}", @event.Id, eventType.Name);
            }
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            _logger.LogInformation("Subscribing {HandlerType} to event {EventType}", typeof(THandler).Name, typeof(TEvent).Name);
            _subscriptionManager.AddSubscription<TEvent, THandler>();
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            _logger.LogInformation("Unsubscribing {HandlerType} from event {EventType}", typeof(THandler).Name, typeof(TEvent).Name);
            _subscriptionManager.RemoveSubscription<TEvent, THandler>();
        }
    }
}
