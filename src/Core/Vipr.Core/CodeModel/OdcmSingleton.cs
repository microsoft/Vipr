// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    /// <summary>
    /// Represents an OData singleton.
    /// http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/cs01/odata-csdl-xml-v4.01-cs01.html#sec_Singleton
    /// </summary>
    public class OdcmSingleton : OdcmProperty
    {
        public OdcmSingleton(string name) : this(name, null)
        {

        }

        public OdcmSingleton(string name, Dictionary<string, string> navigationPropertyBindings) : base(name)
        {
            // Adding NavigationPropertyBindings for generation hints.
            // http://docs.oasis-open.org/odata/odata-csdl-xml/v4.01/cs01/odata-csdl-xml-v4.01-cs01.html#sec_NavigationPropertyBinding
            NavigationPropertyBindings = navigationPropertyBindings;
        }

        public Dictionary<string, string> NavigationPropertyBindings { get; private set; }
    }
}