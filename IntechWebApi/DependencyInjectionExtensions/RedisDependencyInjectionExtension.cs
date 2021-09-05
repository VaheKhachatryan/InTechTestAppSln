using Common.Caching;
using Common.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntechWebApi.DependencyInjectionExtensions
{
	public static class RedisDependencyInjectionExtension
	{
		public static void RegisterRedisDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			var redisConfigSection = configuration.GetSection(typeof(DistributedCacheConfig).Name);
			var distributedCacheConfig = new DistributedCacheConfig();
			redisConfigSection.Bind(distributedCacheConfig);

			StackExchangeRedisCacheServiceCollection.AddStackExchangeRedisCache(services, options =>
			{
				options.Configuration = distributedCacheConfig.ConnectionString;
				options.InstanceName = distributedCacheConfig.InstanceName;
			});
		}
	}
}
