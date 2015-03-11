// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
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
                .Where(p => isNavigation == (p.Type is OdcmEntityClass || p.Type is OdcmMediaClass));
        }

        public static IEnumerable<OdcmProperty> NavigationProperties(this OdcmClass odcmClass, bool? isCollection = null)
        {
            return odcmClass.Properties.WhereIsNavigation().Where(p => isCollection == null || p.IsCollection == isCollection);
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
