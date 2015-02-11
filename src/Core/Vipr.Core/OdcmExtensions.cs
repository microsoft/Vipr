// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Core
{
    public static class OdcmExtensions
    {
        public static IEnumerable<OdcmProperty> WhereIsNavigation(this IEnumerable<OdcmProperty> odcmProperties, bool isNavigation = true)
        {
            return odcmProperties
                .Where(
                    p =>
                        isNavigation ==
                        (p.Type is OdcmClass &&
                         (((OdcmClass) p.Type).Kind == OdcmClassKind.Entity ||
                          ((OdcmClass) p.Type).Kind == OdcmClassKind.MediaEntity)));
        }

        public static IEnumerable<OdcmProperty> NavigationProperties(this OdcmClass odcmClass)
        {
            return odcmClass.Properties.WhereIsNavigation();
        }

        public static IEnumerable<OdcmProperty> StructuralProperties(this OdcmClass odcmClass)
        {
            return odcmClass.Properties.WhereIsNavigation(false);
        }

        public static IEnumerable<OdcmClass> NestedDerivedTypes(this OdcmClass odcmClass)
        {
            var graph = new Queue<OdcmClass>();
            graph.Enqueue(odcmClass);
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

        public static IEnumerable<OdcmParameter> UriParameters(this OdcmMethod odcmMethod)
        {
            return odcmMethod.Parameters
                .Where(p => p.CallingConvention == OdcmCallingConvention.InHttpRequestUri);
        }

        public static IEnumerable<OdcmParameter> BodyParameters(this OdcmMethod odcmMethod)
        {
            return odcmMethod.Parameters
                .Where(p => p.CallingConvention == OdcmCallingConvention.InHttpMessageBody);
        }
    }
}
