using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Caching.StackExchangeRedis
{
    /// <summary>
    /// Extension methods for setting up Redis distributed cache related services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class StackExchangeRedisCacheServiceCollection
    {
        /// <summary>
        /// Adds Redis distributed caching services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{RedisCacheOptions}"/> to configure the provided
        /// <see cref="RedisCacheOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddStackExchangeRedisCache(IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());

            return services;
        }
    }
}
