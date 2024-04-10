using System;
using System.Collections.Generic;
using System.Reflection;
using Z3.Utils;

namespace Marmalade.TheGameOfLife.Shared
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new();

        public static void AddService(object service)
        {
            Type type = service.GetType();
            services[type] = service;
        }

        public static void RemoveService(object service)
        {
            Type type = service.GetType();
            services.Remove(type);
        }

        public static T GetService<T>() where T : class
        {
            Type type = typeof(T);
            if (services.TryGetValue(type, out object service))
            {
                return (T)service;
            }

            throw new KeyNotFoundException($"Service of type {type.Name} not found in ServiceLocator.");
        }

        public static void InjectServices(this object target)
        {
            const string IgnoredNamespace = "UnityEngine";

            List<(FieldInfo, InjectAttribute)> fields = ReflectionUtils.GetAllFieldsWithAttribute<InjectAttribute>(target, IgnoredNamespace);
            List<(PropertyInfo, InjectAttribute)> properties = ReflectionUtils.GetAllPropertyWithAttribute<InjectAttribute>(target, IgnoredNamespace);

            foreach ((FieldInfo field, _) in fields)
            {
                if (services.TryGetValue(field.FieldType, out object service))
                {
                    field.SetValue(target, service);
                }
            }

            foreach ((PropertyInfo property, _) in properties)
            {
                if (services.TryGetValue(property.PropertyType, out object service))
                {
                    property.SetValue(target, service);
                }
            }
        }
    }
}
