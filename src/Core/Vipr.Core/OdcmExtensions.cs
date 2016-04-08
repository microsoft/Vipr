// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

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

        public static bool TryFindProperty(this OdcmClass odcmClass, string propertyPath, out OdcmProperty odcmProperty)
        {
            while (odcmClass != null)
            {
                odcmProperty = odcmClass.Properties.SingleOrDefault(p => p.Name == propertyPath);
                if (odcmProperty != null)
                {
                    return true;
                }
                odcmClass = odcmClass.Base;
            }
            odcmProperty = null;
            return false;
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

        public static bool SupportsInsert(this OdcmObject odcmObject)
        {
            return odcmObject.Projection.SupportsInsert();
        }

        public static bool SupportsUpdate(this OdcmObject odcmObject)
        {
            return odcmObject.Projection.SupportsUpdate();
        }

        public static bool SupportsUpdateLink(this OdcmObject odcmObject)
        {
            return odcmObject.Projection.SupportsUpdateLink();
        }

        public static bool SupportsDelete(this OdcmObject odcmObject)
        {
            return odcmObject.Projection.SupportsDelete();
        }

        public static bool SupportsDeleteLink(this OdcmObject odcmObject)
        {
            return odcmObject.Projection.SupportsDeleteLink();
        }

        public static bool SupportsExpand(this OdcmObject odcmObject)
        {
            return odcmObject.Projection.SupportsExpand();
        }

        public static bool? Supports(this OdcmObject odcmObject, string term)
        {
            return odcmObject.Projection.Supports(term);
        }

        public static bool IsOneOf(this OdcmObject odcmObject, string term)
        {
            return odcmObject.Projection.IsOneOf(term);
        }

        public static bool? BooleanValueOf(this OdcmObject odcmObject, string term)
        {
            return odcmObject.Projection.BooleanValueOf(term, odcmObject);
        }

        public static IEnumerable<string> EnumValueOf(this OdcmObject odcmObject, string term)
        {
            return odcmObject.Projection.EnumValueOf(term, odcmObject);
        }

        public static IEnumerable<string> StringCollectionValueOf(this OdcmObject odcmObject, string term)
        {
            return odcmObject.Projection.StringCollectionValueOf(term, odcmObject);
        }
    }
}
