using BuildBlockCore.Mediator.Messages;
using BuildBlockCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Application.Commands
{
    public class DeleteCustomerCommand : Command
    {

        public Guid Id { get; private set; }

        public DeleteCustomerCommand(LNotifications notifications, Guid _id):base(notifications)
        {
            Id = _id;
        }
    }
}
