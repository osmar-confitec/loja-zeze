using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersApi.Automapper;

namespace UsersApi.Configuration
{
    public static class AutoMapperSetupConfig
    {
        public static IServiceCollection AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(RequestToResponseModelMappingProfile)
                                   );

            return services;
        }
    }
}
