using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    public class ImplementedInterfaces
    {
        public static IEnumerable<Type> ForFetcherClass(OdcmClass odcmClass)
        {
            var retVal = new List<Type>();

            foreach (var projection in odcmClass.DistinctProjections())
            {
                retVal.Add(new Type(NamesService.GetFetcherInterfaceName(odcmClass, projection)));
            }

            return retVal;
        }

        public static IEnumerable<Type> ForConcrete(OdcmClass odcmClass)
        {
            var retVal = new List<Type>
            {
                new Type(NamesService.GetConcreteInterfaceName(odcmClass)),
            };

            return retVal;
        }

        public static IEnumerable<Type> ForCollectionClass(OdcmClass odcmClass)
        {
            var retVal = new List<Type>();

            foreach (var projection in odcmClass.DistinctProjections())
            {
                retVal.Add(new Type(NamesService.GetCollectionInterfaceName(odcmClass, projection)));
            }

            return retVal;
        }

        public static IEnumerable<Type> ForEntityContainer(OdcmClass odcmContainer)
        {
            return new List<Type> { new Type(NamesService.GetEntityContainerInterfaceName(odcmContainer)) };
        }

        public static IEnumerable<Type> ForConcreteInterface(OdcmClass odcmClass)
        {
            var retVal = new List<Type>();

            var baseClass = odcmClass.Base;

            if (baseClass != null)
            {
                retVal.Add(new Type(NamesService.GetConcreteInterfaceName(baseClass)));
            }

            return retVal;
        }

        public static IEnumerable<Type> ForFetcherInterface(OdcmClass odcmClass)
        {
            var retVal = new List<Type>();

            var baseClass = odcmClass.Base as OdcmClass;

            if (baseClass != null)
            {
                retVal.Add(new Type(NamesService.GetFetcherInterfaceName(baseClass)));
            }

            return retVal;
        }

        public static IEnumerable<Type> Empty { get { return Enumerable.Empty<Type>(); } }
    }
}