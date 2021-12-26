using System;
using DataTransit.Infrastructure.Caching;
using DataTransit.Infrastructure.Excel;
using DataTransit.Services;
using Hf.Core.EfCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contain all the service collection extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add infrastructure services to the .NET Dependency Injection container.
        /// </summary>
        /// <param name="services">The type to be extended.</param>
        /// <param name="lifetime">The life time of the service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is <see langword="null"/>.</exception>
        public static void AddServices(this IServiceCollection services, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }


            // register data layer
            services.AddDataAccess(configuration, lifetime);

            services.AddScoped<ICustomerService, CustomerService>();
        }
    }
}
