using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSub.Domain.Events
{
    public class OrderCreatedEvent : EventBase
    {
        public Guid OrderId { get; }
        public Guid UserId { get; }
        public decimal Amount { get; }

        public OrderCreatedEvent(Guid orderId, Guid userId, decimal amount)
        {
            OrderId = orderId;
            UserId = userId;
            Amount = amount;
        }
    }
}
