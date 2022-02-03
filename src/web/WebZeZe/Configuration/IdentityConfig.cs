using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebZeZe.Configuration
{
    public static class IdentityConfig
    {

        public static void AddIdentityConfig(this IServiceCollection serviceCollection)
        {

            serviceCollection.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.LoginPath = "/login";
                       options.AccessDeniedPath = "/erro/403";
                   });
        }

        public static void UseIdentityConfig(this IApplicationBuilder app)
        {

            app.UseAuthentication();
            app.UseAuthorization();

        }

    }
}
