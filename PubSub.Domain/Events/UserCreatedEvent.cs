using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubSub.Domain.Events
{
    public class UserCreatedEvent : EventBase
    {
        public Guid UserId { get; }
        public string Username { get; }
        public string Email { get; }

        public UserCreatedEvent(Guid userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}
