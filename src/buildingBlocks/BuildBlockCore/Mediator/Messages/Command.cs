using BuildBlockCore.Utils;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Mediator.Messages
{
    public abstract class Command : Message, IRequest<LNotifications>
    {
        public DateTime Timestamp { get; private set; }

        public LNotifications _Notifications { get; protected set; }

        public bool IsValids { get; protected set; }

       

        protected Command(LNotifications notifications)
        {
            _Notifications = notifications ?? new LNotifications();
            Timestamp = DateTime.Now;
           
        }


        protected void IsRequerid<T>(T value, string nameField = null)
        {
            if (value == null)
                return;

            if (value is string)
            {
                if ((value as string) == null)
                {
                    _Notifications.Add(new LNotification { Message = $" Atenção o campo ${nameField ?? value.GetType().Name} precisa ser preenchido. " });
                    return;
                }
            }


            if (value is decimal)
            {
                if ((Convert.ToDecimal(value) <= 0))
                {
                    _Notifications.Add(new LNotification { Message = $" Atenção o campo ${ nameField ?? value.GetType().Name} precisa ser preenchido. " });
                    return;
                }
            }

            if (value is int)
            {
                if ((Convert.ToInt32(value) <= 0))
                {
                    _Notifications.Add(new LNotification { Message = $" Atenção o campo ${ nameField ?? value.GetType().Name} precisa ser preenchido. " });
                    return;
                }
            }
        }
    }
}
