﻿namespace PubSub.Domain.Events
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime CreatedAt { get; }
    }
}
