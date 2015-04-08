// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    public class ContainerNavigationCollectionProperty : FetcherNavigationCollectionProperty
    {
        protected ContainerNavigationCollectionProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
        }

        public static ContainerNavigationCollectionProperty ForService(OdcmProperty odcmProperty)
        {
            return new ContainerNavigationCollectionProperty(odcmProperty);
        }
    }
}