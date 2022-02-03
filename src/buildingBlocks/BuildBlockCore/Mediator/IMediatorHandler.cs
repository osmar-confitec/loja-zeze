using BuildBlockCore.Mediator.Messages;
using BuildBlockCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
        Task<LNotifications> SendCommand<T>(T comando) where T : Command;
    }
}
