using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ADO.NET.Settings.DataWrapper
{
    internal class InstanceCreator
    {
        // Gets the singleton instance for the type specified.
        public static InstanceCreator Default => Singleton<InstanceCreator>.Instance;

        /// Gets the constructor information for given type.
        public virtual ConstructorInfo GetConstructorInfo(Type type)
        {
            var constructors = type.GetTypeInfo().DeclaredConstructors;
            return constructors.FirstOrDefault(c => c.Name != ".cctor");
        }

        /// Creates the instance for the specified type.
        public object CreateInstance(Type type)
        {
            var constructorInfo = GetConstructorInfo(type);
            var parameterInfos = constructorInfo.GetParameters();

            if (parameterInfos.Length == 0)
            {
                return Activator.CreateInstance(type);
            }

            var parameters = new object[parameterInfos.Length];

            foreach (var parameterInfo in parameterInfos)
            {
                parameters[parameterInfo.Position] = CreateInstance(parameterInfo.ParameterType);
            }

            return Activator.CreateInstance(type, parameters);
        }
    }
}
