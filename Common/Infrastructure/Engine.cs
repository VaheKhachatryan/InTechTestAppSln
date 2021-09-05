using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure
{
    public class Engine : IEngine
    {
        private readonly IServiceProvider _serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;

            // most of API providers require TLS 1.2 nowadays
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type) => GetServiceProvider().GetRequiredService(type);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
            where T : class
        {
            return this.GetServiceProvider().GetRequiredService(typeof(T)) as T;
        }
        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public T ResolveUnregistered<T>(Type type)
            where T : class
        {
            var constructors = type.GetConstructors();
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var parameterInstances = new List<object>();
                foreach (var parameter in parameters)
                {
                    var service = this.Resolve(parameter.ParameterType);
                    if (service == null)
                    {
                        throw new Exception("Unknown dependency.");
                    }

                    parameterInstances.Add(service);
                }

                return Activator.CreateInstance(type, parameterInstances.ToArray()) as T;
            }

            throw new Exception("No constructor was found that had all the dependencies satisfied.");
        }

        /// <summary>
        /// Creates new scope from singleton scope
        /// </summary>
        /// <returns></returns>
        public IServiceScope BeginScope()
        {
            return this.GetServiceProvider().CreateScope();
        }

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected IServiceProvider GetServiceProvider()
        {
            var accessor = this._serviceProvider.GetService<IHttpContextAccessor>();
            var context = accessor?.HttpContext;
            return context?.RequestServices ?? this._serviceProvider;
        }
    }
}
