using Microsoft.Extensions.Logging;
using PubSub.Application.Common;
using PubSub.Domain.Events;

namespace PubSub.Application.Events
{
    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(UserCreatedEvent @event)
        {
            _logger.LogInformation("User created: {Username} ({UserId})", @event.Username, @event.UserId);

            _logger.LogInformation("Sending welcome email to {Email}", @event.Email);

            return Task.CompletedTask;
        }
    }

}
