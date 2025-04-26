using Microsoft.AspNetCore.Mvc;
using PubSub.Application.Common;
using PubSub.Domain.Events;

namespace PubSub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public EventsController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost("test")]
        public IActionResult TestEvent()
        {
            var userId = Guid.NewGuid();

            var testUser = new UserCreatedEvent(userId, "testuser", "test@example.com");

            _eventBus.Publish(testUser);

            var orderId = Guid.NewGuid();

            var testOrder = new OrderCreatedEvent(orderId, userId, 99.99m);

            _eventBus.Publish(testOrder);

            return Ok(new { Message = "Test events published" });
        }
    }
}
