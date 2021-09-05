using System;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure
{
    public interface IEngine
    {
        IServiceScope BeginScope();

        object Resolve(Type type);

        T Resolve<T>() where T : class;

        T ResolveUnregistered<T>(Type type) where T : class;
    }
}