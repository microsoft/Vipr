using System;
using System.Linq;
using System.Reflection;

namespace Vipr
{
    public static class TypeResolver
    {
        internal static T GetInstance<T>(string assemblyName) where T:class
        {
            if (!typeof(T).IsInterface)
                throw new InvalidOperationException(String.Format("{0} is not an interface.", typeof(T).Name));

            var asm = Assembly.Load(assemblyName);

            var implementers = asm.GetTypes().Where(t => t.GetInterfaces().Contains(typeof (T))).ToList();

            if (!implementers.Any())
                throw new InvalidOperationException(String.Format("{0} does not have a type that implements {1}.",
                    assemblyName, typeof (T).Name));

            if (implementers.Count() > 1)
                throw new InvalidOperationException(String.Format("{0} has more than one type that implements {1}.",
                    assemblyName, typeof(T).Name));

            return Activator.CreateInstance(implementers.First()) as T;
        }
    }
}
