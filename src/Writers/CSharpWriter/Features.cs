using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal static class Features
    {
        public static IEnumerable<Feature> ForOdcmEnum(OdcmEnum odcmEnum)
        {
            return new[]
            {
                new Feature 
                {
                    Enums = new[]
                    {
                        new Enum(odcmEnum)
                    }
                }
            };
        }

        public static IEnumerable<Feature> ForOdcmClass(OdcmClass odcmClass)
        {
            switch (odcmClass.Kind)
            {
                case OdcmClassKind.Complex:
                    return Features.ForOdcmClassComplex(odcmClass);

                case OdcmClassKind.MediaEntity:
                case OdcmClassKind.Entity:
                    return Features.ForOdcmClassEntity(odcmClass);

                case OdcmClassKind.Service:
                    return Enumerable.Empty<Feature>();
            }

            throw new NotImplementedException(string.Format("OdcmClassKind {0} is not recognized", odcmClass.Kind));
        }

        public static IEnumerable<Feature> ForEntityContainer(OdcmClass odcmClass, OdcmModel model)
        {
            switch (odcmClass.Kind)
            {
                case OdcmClassKind.Complex:
                case OdcmClassKind.MediaEntity:
                case OdcmClassKind.Entity:
                    return Enumerable.Empty<Feature>();

                case OdcmClassKind.Service:
                    return Features.ForOdcmClassService(odcmClass, model);
            }

            throw new NotImplementedException(string.Format("OdcmClassKind {0} is not recognized", odcmClass.Kind));
        }

        private static IEnumerable<Feature> ForOdcmClassService(OdcmClass odcmClass, OdcmModel model)
        {
            return new[]
            {
                new Feature
                {
                    Classes = new[] {Class.ForEntityContainer(model, odcmClass)},
                    Interfaces = new[] {Interface.ForEntityContainer(odcmClass)},
                }
            };
        }

        private static IEnumerable<Feature> ForOdcmClassEntity(OdcmClass odcmClass)
        {
            return new[]
            {
                new Feature
                {
                    Classes = new[]
                    {
                        Class.ForConcrete(odcmClass),
                        Class.ForFetcher(odcmClass),
                        Class.ForCollection(odcmClass)
                    },
                    Interfaces = new[]
                    {
                        Interface.ForConcrete(odcmClass),
                        Interface.ForFetcher(odcmClass),
                        Interface.ForCollection(odcmClass)
                    }
                }
            };
        }

        private static IEnumerable<Feature> ForOdcmClassComplex(OdcmClass odcmClass)
        {
            return new[]
            {
                new Feature
                {
                    Classes = new[] {Class.ForComplex(odcmClass)}
                }
            };
        }
    }
}