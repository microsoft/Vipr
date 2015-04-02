// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    public class NavigationProperty : Property
    {
        protected NavigationProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            ModelName = odcmProperty.Name;
        }

        public string ModelName { get; internal set; }
    }
}
