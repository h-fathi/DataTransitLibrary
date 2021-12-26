using System;
using Hf.Core.EfCore.GenericRepoitory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hf.Core.EfCore
{
    /// <summary>
    /// Contain all the service collection extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add generic repository services to the .NET Dependency Injection container.
        /// </summary>
        /// <param name="services">The type to be extended.</param>
        /// <param name="lifetime">The life time of the service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="services"/> is <see langword="null"/>.</exception>
        public static void AddDataAccess(this IServiceCollection services, IConfiguration Configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            string connectionString = Configuration.GetConnectionString("DefaultDbConnection");
            services.AddDbContext<DemoDbContext>(option => option.UseSqlServer(connectionString));

            services.Add(new ServiceDescriptor(
               typeof(IRepository),
               serviceProvider => new Repository(ActivatorUtilities.CreateInstance<DemoDbContext>(serviceProvider)),
               lifetime)); // Call it just after registering your DbConext.
        }
    }
}
