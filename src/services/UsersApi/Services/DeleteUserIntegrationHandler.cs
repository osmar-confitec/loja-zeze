using BuildBlockCore.Mediator.Messages;
using BuildBlockCore.Mediator.Messages.Integration;
using BuildBlockCore.Utils;
using BuildBlockMessageBus.MessageBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UsersApi.Services
{
    public class DeleteUserIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;


        public DeleteUserIntegrationHandler(IServiceProvider serviceProvider,
                       IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> DeleteUser(UserDeletedIntegrationEvent request)
        {

            var noty = default(LNotifications);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {

                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                    noty = scope.ServiceProvider.GetRequiredService<LNotifications>();
                    noty = noty ?? new LNotifications();
                    var userDelete = await userManager.FindByIdAsync(request.Id.ToString());
                    if (userDelete != null)
                    {
                        var result = await userManager.DeleteAsync(userDelete);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                                noty.Add(new LNotification { Message = error.Description });
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {

                throw;
            }

        
            var notResponse = new ResponseMessage(noty, null);
            return notResponse;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UserDeletedIntegrationEvent, ResponseMessage>(async request =>
                await DeleteUser(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }
    }
}
