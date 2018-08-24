// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    /// <summary>
    /// Represents an OData entity set.
    /// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/cs01/odata-csdl-xml-v4.01-cs01.html#sec_EntitySet
    /// </summary>
    public class OdcmEntitySet : OdcmProperty
    {
        public OdcmEntitySet(string name) : this(name, null)
        {
        }

        public OdcmEntitySet(string name, Dictionary<string, string> navigationPropertyBindings) : base(name)
        {
            // Adding NavigationPropertyBindings for generation hints.
            // http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/cs01/odata-csdl-xml-v4.01-cs01.html#sec_NavigationPropertyBinding
            NavigationPropertyBindings = navigationPropertyBindings;
        }

        public Dictionary<string, string> NavigationPropertyBindings { get; private set; }
    }
}
