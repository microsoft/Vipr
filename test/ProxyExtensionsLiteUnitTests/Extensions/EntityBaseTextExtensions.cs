using System.Reflection;
using Microsoft.OData.ProxyExtensions.Lite;

namespace ProxyExtensionsUnitTests.Extensions
{
    internal static class EntityBaseTextExtensions
    {
        public static void CallOnPropertyChanged(this EntityBase entityBase, string propertyName)
        {
            typeof(EntityBase).GetMethod("OnPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(entityBase, new[] { propertyName });
        }
    }
}
