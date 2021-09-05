using System;
using System.Runtime.CompilerServices;

namespace Common.Infrastructure
{
    public static class EngineContext
    {
        /// <summary>
        /// Create a static instance of the Benivo platform engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(IServiceProvider serviceProvider, bool forceRecreate = false)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = new Engine(serviceProvider);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// Gets the singleton Benivo` platform engine used to access Benivo` platform services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    throw new ArgumentNullException("Instance cannot be null.");
                }

                return Singleton<IEngine>.Instance;
            }
        }
    }
}
