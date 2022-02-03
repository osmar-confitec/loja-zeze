using BuildBlockCore.Identity;
using BuildBlockCore.Utils;
using BuildBlockServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebZeZe.Services;

namespace WebZeZe.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<LNotification>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthService, AuthService>(opt =>
            {
                opt.BaseAddress = new Uri(configuration.GetSection("ApiGatewayUserUrl").Value);
            })
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .ConfigureHttpMessageHandlerBuilder(b =>
            {
                b.PrimaryHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback
                            = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
            })
            ;

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

        }
    }
}
