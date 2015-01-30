// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Core
{
    public static class OdcmExtensions
    {
        public static bool IsCollection(this OdcmProperty odcmProperty)
        {
            return odcmProperty.Field != null && odcmProperty.Field.IsCollection;
        }

        public static IEnumerable<OdcmProperty> WhereIsNavigation(this IEnumerable<OdcmProperty> odcmProperties, bool isNavigation = true)
        {
            return odcmProperties
                .Where(p => isNavigation == (p.Type is OdcmClass && ((OdcmClass)p.Type).Kind == OdcmClassKind.Entity));
        }

        public static IEnumerable<OdcmProperty> NavigationProperties(this OdcmClass odcmClass)
        {
            return odcmClass.Properties.WhereIsNavigation();
        }

        public static IEnumerable<OdcmProperty> StructuralProperties(this OdcmClass odcmClass)
        {
            return odcmClass.Properties.WhereIsNavigation(false);
        }

        public static IEnumerable<OdcmProperty> GetKeyProperties(this OdcmClass odcmClass)
        {
            var keyProperties = Enumerable.Empty<OdcmProperty>();

            var baseClass = odcmClass.Base as OdcmClass;

            if (baseClass != null)
                keyProperties = baseClass.GetKeyProperties();

            return keyProperties
                .Concat(odcmClass.Properties.Where(p => odcmClass.Key.Contains(p.Field)));
        }

        public static IEnumerable<OdcmType> NestedDerivedTypes(this OdcmType odcmType)
        {
            var graph = new Queue<OdcmType>();
            graph.Enqueue(odcmType);
            while (graph.Count > 0)
            {
                var parent = graph.Dequeue();
                foreach (var child in parent.Derived)
                {
                    graph.Enqueue(child);
                    yield return child;
                }
            }
        }
    }
}
