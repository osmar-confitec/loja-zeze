using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Configuration
{
    public static class SqlConfig
    {

        public static void AddSql(this IServiceCollection services,
           IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Data.Context.CustomerContext>(options =>
                 options.UseSqlServer(connectionString, (x) => { x.EnableRetryOnFailure(); })
                 .EnableSensitiveDataLogging()
                 .UseLazyLoadingProxies()
                 );
        }

    }
}
