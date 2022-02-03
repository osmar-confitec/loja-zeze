using BuildBlockCore.Data.Interfaces;
using BuildBlockCore.Identity;
using BuildBlockCore.Mediator;
using BuildBlockCore.Utils;
using CustomerApi.Application.Queries;
using CustomerApi.Data.Context;
using CustomerApi.Data.Repository;
using CustomerApi.Data.Uow;
using CustomerApi.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace CustomerApi.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            /*Cors*/

            services.AddCors(options => {

                options.AddPolicy("development", builder => {
                    builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
                });

                options.AddPolicy("production", builder => {
                    builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
                });

            });


            /*versioning*/
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });


            /*User Context*/
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            /*Mediator*/
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddMediatR(typeof(Startup));

            /*Notifications*/
            services.AddScoped<LNotifications>();

            /*Repository*/
            services.AddScoped<CustomerContext>();
            services.AddScoped<IUnitOfWork, UnitOfWorkCustomer>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerQueries, CustomerQueries>();

            /*Settings*/
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            return services;
        }

        public static void UseIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

        }
        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
