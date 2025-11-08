using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils.Core
{
    public class ServiceManager
    {
        readonly Dictionary<Type, object> registeredServices = 
                                         new Dictionary<Type, object>();

        public Dictionary<Type, object> RegisteredServices => registeredServices;

        public ServiceManager Register<T>(T service) where T : class
        {
            Type type = typeof(T);

            if (registeredServices.ContainsKey(type))
            {
                Debug.LogError($"ServiceManager.Register : Service of type {type.FullName} already registered!");
            }

            registeredServices[type] = service;
            return this;
        }

        public ServiceManager Register(Type type, object service)
        {
            if (registeredServices.ContainsKey(type))
            {
                Debug.LogError($"ServiceManager.Register : Service of type {type.FullName} already registered!");
            }

            registeredServices[type] = service;
            return this;
        }

        public bool TryGet<T>(out T service) where T : class
        {
            Type type = typeof(T);

            if (registeredServices.TryGetValue(type, out object value))
            {
                service = value as T;
                return true;
            }

            Debug.LogError($"ServiceManager.TryGet : Service of type {type.FullName} is not registered!");
            service = null;
            return false;
        }

        public T Get<T>() where T : class
        {
            Type type = typeof(T);
            if(registeredServices.TryGetValue(type,out object value))
            {
                return value as T;
            }

            throw new ArgumentException($"ServiceManager.Get : Service of type {type.FullName} is not registered!");
        }
    }
}
