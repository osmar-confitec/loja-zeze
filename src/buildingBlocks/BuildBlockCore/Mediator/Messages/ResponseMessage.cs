using BuildBlockCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Mediator.Messages
{
    public class ResponseMessage  :   Message 
    {
        public LNotifications Notifications { get; set; } = new LNotifications();
        public string Response { get; private set; }



        public ResponseMessage(LNotifications notifications, string response)
        {
             Response = response;
             Notifications = notifications;
        }
    }
}
