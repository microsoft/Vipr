// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
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
            Enums = global::CSharpWriter.Enums.Empty;
            Classes = global::CSharpWriter.Classes.Empty;
            Interfaces = global::CSharpWriter.Interfaces.Empty;
        }

        public static Feature ForCountableCollection(OdcmEntityClass odcmClass)
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

        public static Feature ForOdcmClassEntity(OdcmEntityClass odcmClass)
        {
            return new Feature
            {
                Classes = global::CSharpWriter.Classes.ForOdcmClassEntity(odcmClass),
                Interfaces = global::CSharpWriter.Interfaces.ForOdcmClassEntity(odcmClass),
            };
        }

        public static Feature ForOdcmClassComplex(OdcmClass odcmClass)
        {
            return new Feature
            {
                Classes = global::CSharpWriter.Classes.ForOdcmClassComplex(odcmClass),
            };
        }

        public static Feature ForOdcmClassService(OdcmServiceClass odcmClass, OdcmModel model)
        {
            return new Feature
            {
                Classes = global::CSharpWriter.Classes.ForOdcmClassService(odcmClass, model),
                Interfaces = global::CSharpWriter.Interfaces.ForOdcmClassService(odcmClass),
            };
        }

        public static Feature ForUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Feature
            {
                Classes = global::CSharpWriter.Classes.ForUpcastMethods(odcmClass),
                Interfaces = global::CSharpWriter.Interfaces.ForUpcastMethods(odcmClass)
            };
        }
    }
}