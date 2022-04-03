using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Services;
using PCBuilder.Infrastructure.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IComputerService, ComputerService>();

            return services;
        }
    }
}
