using EndoplasmCleanArchitecture.Authentication.Interfaces;
using EndoplasmCleanArchitecture.Authentication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndoplasmCleanArchitecture.Authentication
{
    public static class ServiceRegistration
    {
        public static void AddAuthenticationLayer(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddDataProtection();
            services.AddScoped<IProtectionService, ProtectionService>();
        }
    }
}
