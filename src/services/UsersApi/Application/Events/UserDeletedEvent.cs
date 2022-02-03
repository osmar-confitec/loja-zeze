using BuildBlockCore.Mediator.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersApi.Application.Events
{
    public class UserDeletedEvent : Event
    {
        public Guid Id { get; private set; }

        public UserDeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}
