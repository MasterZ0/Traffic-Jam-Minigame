using System;
using System.Collections.Generic;

namespace Hasbro.TheGameOfLife.Shared
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new();

        public static void AddService<T>(T service)
        {
            Type type = typeof(T);
            services[type] = service;
        }

        public static T GetService<T>()
        {
            Type type = typeof(T);
            if (services.TryGetValue(type, out object value))
            {
                return (T)value;
            }

            throw new KeyNotFoundException($"Service of type {type.Name} not found in ServiceLocator.");
        }
    }
}
