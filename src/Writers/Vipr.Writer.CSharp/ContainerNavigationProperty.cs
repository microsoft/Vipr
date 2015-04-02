// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class ContainerNavigationProperty : FetcherNavigationProperty
    {
        protected ContainerNavigationProperty(OdcmProperty odcmProperty) : base(odcmProperty)
        {
            PrivateSet = true;
        }

        public static ContainerNavigationProperty ForService(OdcmProperty odcmProperty)
        {
            return new ContainerNavigationProperty(odcmProperty);
        }
    }
}