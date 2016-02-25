// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    internal class Feature
    {
        public string Name { get; set; }
        public IEnumerable<Enum> Enums { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public IEnumerable<Interface> Interfaces { get; set; }

        internal Feature()
        {
            Enums = global::Vipr.Writer.CSharp.Lite.Enums.Empty;
            Classes = global::Vipr.Writer.CSharp.Lite.Classes.Empty;
            Interfaces = global::Vipr.Writer.CSharp.Lite.Interfaces.Empty;
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
                Classes = global::Vipr.Writer.CSharp.Lite.Classes.ForOdcmClassEntity(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.Lite.Interfaces.ForOdcmClassEntity(odcmClass),
            };
        }

        public static Feature ForOdcmClassComplex(OdcmClass odcmClass)
        {
            return new Feature
            {
                Classes = global::Vipr.Writer.CSharp.Lite.Classes.ForOdcmClassComplex(odcmClass),
            };
        }

        public static Feature ForOdcmClassService(OdcmServiceClass odcmClass, OdcmModel model)
        {
            return new Feature
            {
                Classes = global::Vipr.Writer.CSharp.Lite.Classes.ForOdcmClassService(odcmClass, model),
                Interfaces = global::Vipr.Writer.CSharp.Lite.Interfaces.ForOdcmClassService(odcmClass),
            };
        }

        public static Feature ForUpcastMethods(OdcmEntityClass odcmClass)
        {
            return new Feature
            {
                Classes = global::Vipr.Writer.CSharp.Lite.Classes.ForUpcastMethods(odcmClass),
                Interfaces = global::Vipr.Writer.CSharp.Lite.Interfaces.ForUpcastMethods(odcmClass)
            };
        }
    }
}