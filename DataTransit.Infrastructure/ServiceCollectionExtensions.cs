using System;
using DataTransit.Infrastructure.Caching;
using DataTransit.Infrastructure.Excel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataTransit.Infrastructure
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
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }


            //added to use Redis cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Host"] + ":" + configuration["Redis:Port"];
                options.InstanceName = "";
            });

            services.AddScoped<ICache, DistributedCache>();

            //excel provider
            services.AddScoped<IExcelProvider, ExcelProvider>();
        }
    }
}
