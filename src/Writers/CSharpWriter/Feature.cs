// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class Feature
    {
        public string Name { get; set; }
        public IEnumerable<Enum> Enums { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<Interface> Interfaces { get; set; }

        internal Feature()
        {
            Enums = Enumerable.Empty<Enum>();
            Classes = Enumerable.Empty<Class>();
            Interfaces = Enumerable.Empty<Interface>();
        }

        public static Feature ForCountableCollection(OdcmClass odcmClass)
        {
            return new Feature
            {
                Classes = new[]
                {
                    Class.ForCountableCollection(odcmClass)
                },
                Interfaces = new[]
                {
                    Interface.ForCountableCollection(odcmClass)
                }
            };
        }

        public static Feature ForOdcmClassEntity(OdcmClass odcmClass)
        {
            return new Feature
            {
                Classes = new[]
                {
                    Class.ForConcrete(odcmClass),
                    Class.ForFetcher(odcmClass),
                    Class.ForCollection(odcmClass),
                },
                Interfaces = new[]
                {
                    Interface.ForConcrete(odcmClass),
                    Interface.ForFetcher(odcmClass),
                    Interface.ForCollection(odcmClass),
                }
            };
        }

        public static Feature ForOdcmClassComplex(OdcmClass odcmClass)
        {
            return new Feature
            {
                Classes = new[] {Class.ForComplex(odcmClass)}
            };
        }

        public static Feature ForOdcmClassService(OdcmClass odcmClass, OdcmModel model)
        {
            return new Feature
            {
                Classes = new[] {Class.ForEntityContainer(model, odcmClass)},
                Interfaces = new[] {Interface.ForEntityContainer(odcmClass)},
            };
        }
    }
}