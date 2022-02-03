using BuildBlockCore.Mediator;
using BuildBlockCore.Mediator.Messages;
using BuildBlockCore.Mediator.Messages.Integration;
using BuildBlockCore.Utils;
using BuildBlockMessageBus.MessageBus;
using BuildBlockServices.Enum;
using CustomerApi.Application.Commands;
using CustomerApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerApi.Services
{
    public class InsertCustomerIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public InsertCustomerIntegrationHandler(IServiceProvider serviceProvider,
                       IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        private void SetResponder()
        {
            _bus.RespondAsync<UserInsertedIntegrationEvent, ResponseMessage>(async request =>
                await RegisterCustomer(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        private async Task<ResponseMessage> RegisterCustomer(UserInsertedIntegrationEvent request)
        {
            var noty = default(LNotifications);
            try
            {
                var insertCustomerCommand = new InsertCustomerCommand(notifications: noty ?? new LNotifications(),
                                                                       _Id: request.Id,
                                                                       name: request.Name,
                                                                       email: request.Email,
                                                                       cpf: request.CPF,
                                                                       publicPlace: request.PublicPlace,
                                                                       number: request.Number,
                                                                       complement: request.Complement,
                                                                       zipCode: request.ZipCode,
                                                                       city: request.City,
                                                                       typeAddress: (TypeAddress)request.TypeAddress,
                                                                       district: request.District,
                                                                       state: request.State
                                                                       );


                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                    noty = scope.ServiceProvider.GetRequiredService<LNotifications>();
                    noty = noty ?? new LNotifications();
                    noty = await mediator.SendCommand(insertCustomerCommand);
                }
                var notResponse = new ResponseMessage(noty, null);
                return notResponse;
            }
            catch (Exception ex)
            {
                noty = noty ?? new LNotifications();
                noty.Add(new LNotification { Message = ex.Message , TypeNotificationNoty = TypeNotificationNoty.BreakSystem });
                return new ResponseMessage(noty, null);
                   
            }
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }
    }
}
