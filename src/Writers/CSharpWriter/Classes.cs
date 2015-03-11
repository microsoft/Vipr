using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal static class Classes
    {
        public static IEnumerable<Class> Empty { get { return Enumerable.Empty<Class>(); } }

        public static IEnumerable<Class> ForOdcmClassEntity(OdcmEntityClass odcmClass)
        {
            return new[]
            {
                Class.ForConcrete(odcmClass),
                Class.ForFetcher(odcmClass),
                Class.ForCollection(odcmClass),
            };
        }

        public static IEnumerable<Class> ForOdcmClassComplex(OdcmClass odcmClass)
        {
            return new[] {Class.ForComplex(odcmClass)};
        }

        public static IEnumerable<Class> ForOdcmClassService(OdcmServiceClass odcmClass, OdcmModel model)
        {
            return new[] {Class.ForEntityContainer(model, odcmClass)};
        }

        public static IEnumerable<Class> ForUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new[]
            {
                Class.ForFetcherUpcastMethods(odcmClass),
                Class.ForConcreteIFetcherUpcastMethods(odcmClass),
            };
        }
    }
}