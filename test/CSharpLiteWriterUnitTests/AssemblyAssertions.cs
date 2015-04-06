using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EdmDecompositionUnitTests
{
    public class AssemblyAssertions : FluentAssertions.Reflection.AssemblyAssertions
    {
        internal AssemblyAssertions(Assembly assembly) : base(assembly)
        {
        }
    }

    public static class AssemblyExtensions
    {
        public static AssemblyAssertions Should(this Assembly asm)
        {
            return new AssemblyAssertions(asm);
        }

        public static IEnumerable<string> GetNamespaces(this Assembly asm)
        {
            return asm.GetTypes().Select(t => t.Namespace).Where(n => n != null).Distinct();
        }

        public static IEnumerable<EnumType> GetEnums(this Assembly asm)
        {
            return asm.GetTypes().Where(t => t.IsEnum).Select(t => new EnumType(t));
        }

        public static EnumType GetEnum(this Assembly asm, string @namespace, string name)
        {
            return asm.GetEnums().FirstOrDefault(e => e.Name == name && e.Namespace == @namespace);
        }

        public static IEnumerable<Type> GetClasses(this Assembly asm)
        {
            return asm.GetTypes().Where(t => t.IsClass);
        }

        public static Type GetClass(this Assembly asm, string @namespace, string name)
        {
            return asm.GetClasses().FirstOrDefault(e => e.Name == name && e.Namespace == @namespace);
        }
    }

    public class EnumType
    {
        readonly Type _instance;

        internal EnumType(Type instance)
        {
            _instance = instance;
        }

        public string Name { get { return _instance.Name; } }

        public string Namespace { get { return _instance.Namespace; } }

        public IDictionary<string, object> Members
        {
            get
            {
                return _instance.GetEnumValues()
                    .Cast<object>()
                    .ToDictionary(
                        value => _instance.GetEnumName(value), 
                        value => Convert.ChangeType(value, UnderlyingType()));
            }
        }

        public Type UnderlyingType()
        {
            return _instance.GetEnumUnderlyingType();
        }
    }
}
