using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Mediator.Messages.Integration
{
   public class UserDeletedIntegrationEvent : IntegrationEvent
    {
        public UserDeletedIntegrationEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }


    }
}
