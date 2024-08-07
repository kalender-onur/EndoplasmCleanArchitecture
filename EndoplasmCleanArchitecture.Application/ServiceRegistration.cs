
using EndoplasmCleanArchitecture.Application.Interfaces;
using EndoplasmCleanArchitecture.Application.Interfaces.User;
using EndoplasmCleanArchitecture.Application.Services.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EndoplasmCleanArchitecture.Presistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }

    }
}
