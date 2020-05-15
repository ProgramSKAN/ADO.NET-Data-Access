using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ADO.NET.Settings.DataWrapper
{
    internal sealed class Singleton<T> where T : new()
    {
        private static readonly ConcurrentDictionary<Type, T> Instances = new ConcurrentDictionary<Type, T>();
        private Singleton()
        {
        }
        public static T Instance
        {
            get { return Instances.GetOrAdd(typeof(T), t => new T()); }
        }
    }
}
