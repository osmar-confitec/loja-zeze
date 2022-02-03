using BuildBlockCore.Data.Interfaces;
using BuildBlockCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Mediator.Messages
{
    public abstract class CommandHandler
    {
        protected readonly LNotifications _notifications;
        protected readonly IUnitOfWork _unitOfWork;




        protected CommandHandler(LNotifications notifications, IUnitOfWork unitOfWork)
        {
            _notifications = notifications ?? new LNotifications();
            _unitOfWork = unitOfWork;
        }

        protected void AddError(string mensagem)
        {
            _notifications.Add(new LNotification { Message = mensagem });
        }

        protected async Task<LNotifications> CommitAsync()
        {
            if (!await _unitOfWork.CommitAsync()) AddError("Houve um erro ao persistir os dados");
            return _notifications;
        }
    }
}
