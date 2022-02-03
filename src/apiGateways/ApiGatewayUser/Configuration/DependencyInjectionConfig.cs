using ApiGatewayUser.Models;
using ApiGatewayUser.Services;
using BuildBlockCore.Identity;
using BuildBlockServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGatewayUser.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            //

            services.AddHttpClient<ICustomerService, CustomerService>(opt =>
            {
                opt.BaseAddress = new Uri(configuration.GetSection("AppSettings:CustomerApiUrl").Value);
            })
                .ConfigureHttpMessageHandlerBuilder(b =>
                {
                    b.PrimaryHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback
                                = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                })
              .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.PollyWaitAndRetryAsync())
              .AddTransientHttpErrorPolicy(
                 p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(configuration.GetSection("AppSettings:UserApiUrl").Value);
            })
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
              .AddPolicyHandler(PollyExtensions.PollyWaitAndRetryAsync())
             .ConfigureHttpMessageHandlerBuilder(b =>
             {
                 b.PrimaryHandler = new HttpClientHandler
                 {
                     ServerCertificateCustomValidationCallback
                             = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                 };
             })
            .AddTransientHttpErrorPolicy(
               p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

        }
    }
}
