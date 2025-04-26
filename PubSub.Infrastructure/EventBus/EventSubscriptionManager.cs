using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PubSub.Application.Common;
using PubSub.Domain.Events;

namespace PubSub.Infrastructure.EventBus
{
    public class EventSubscriptionManager
    {
        private readonly ConcurrentDictionary<Type, List<Type>> _handlers = new();
        private readonly IServiceProvider _serviceProvider;

        public EventSubscriptionManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void AddSubscription<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);

            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = new List<Type>();
            }

            if (_handlers[eventType].Contains(typeof(THandler)))
            {
                throw new ArgumentException($"Handler Type {typeof(THandler).Name} already registered for '{eventType.Name}'", nameof(THandler));
            }

            _handlers[eventType].Add(typeof(THandler));
        }

        public void RemoveSubscription<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);

            if (!_handlers.ContainsKey(eventType))
            {
                return;
            }

            _handlers[eventType].Remove(typeof(THandler));

            if (!_handlers[eventType].Any())
            {
                _handlers.TryRemove(eventType, out _);
            }
        }

        public async Task ProcessEvent<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventType = @event.GetType();

            if (_handlers.ContainsKey(eventType))
            {
                using var scope = _serviceProvider.CreateScope();
                foreach (var handlerType in _handlers[eventType])
                {
                    var handler = scope.ServiceProvider.GetService(handlerType);
                    if (handler == null) continue;

                    var genericHandler = typeof(IEventHandler<>).MakeGenericType(eventType);
                    var method = genericHandler.GetMethod("HandleAsync");

                    if (method != null)
                    {
                        await (Task)method.Invoke(handler, [@event])!;
                    }
                }
            }
        }

        public bool HasSubscriptionsForEvent<TEvent>() where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            return _handlers.ContainsKey(eventType) && _handlers[eventType].Any();
        }

        public bool HasSubscriptionsForEvent(Type eventType)
        {
            return _handlers.ContainsKey(eventType) && _handlers[eventType].Any();
        }

        public IEnumerable<Type> GetHandlersForEvent<TEvent>() where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            return _handlers.ContainsKey(eventType) ? _handlers[eventType] : Enumerable.Empty<Type>();
        }
    }
}
