using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.OData.Client;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.ProxyExtensions;

namespace CSharpWriterUnitTests
{
    public static class DataServiceContextWrapperExtensions
    {
        public static DataServiceContextWrapper WithDefaultResolvers(this DataServiceContextWrapper context, string @namespace)
        {
            context.ResolveName = type => context.DefaultResolveNameInternal(type, @namespace, @namespace) ?? type.FullName;
            context.ResolveType = name => context.DefaultResolveTypeInternal(name, @namespace, @namespace);

            return context;
        }

        public static DataServiceContextWrapper WithIgnoreMissingProperties(this DataServiceContextWrapper context)
        {
            context.IgnoreMissingProperties = true;

            return context;
        }

        public static DataServiceContextWrapper UseJson(this DataServiceContextWrapper context, string edmx,
            bool addSchema = false)
        {
            if (addSchema)
                edmx =
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?><edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\">" +
                    edmx + "</edmx:Edmx>";

            Debug.WriteLine(edmx);

            var model = EdmxReader.Parse(XmlReader.Create(new StringReader(edmx)));

            context.Format.UseJson(model);

            return context;
        }

        public static ReadOnlyQueryableSetBase CreateCollection(this DataServiceContextWrapper context, Type collectionType, Type instanceType, string path, object entity = null)
        {
            return
                Activator.CreateInstance(collectionType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null,
                    new[]
                    {
                        context.GetType().GetMethod("CreateQuery", new []{typeof(string)}).MakeGenericMethod(instanceType).Invoke(context, new object[]{path}),
                        context,
                        entity,
                        path
                    }, null) as ReadOnlyQueryableSetBase;
        }

        public static RestShallowObjectFetcher CreateFetcher(this DataServiceContextWrapper context, Type fetcherType, string path)
        {
            var instance =
                Activator.CreateInstance(fetcherType) as
                    RestShallowObjectFetcher;

            instance.Initialize(context, path);

            return instance;
        }

        public static EntityBase CreateConcrete(this DataServiceContextWrapper context, Type concreteType)
        {
            var instance =
                Activator.CreateInstance(concreteType) as
                    EntityBase;

            typeof(BaseEntityType).GetProperty("Context", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(instance, context);

            context.AddObject(concreteType.Name + "s", instance);

            context.SaveChangesAsync().Wait();

            return instance;
        }
    }
}