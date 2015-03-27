// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class FetcherNavigationCollectionProperty : FetcherNavigationProperty
    {
        public Type CollectionType { get; private set; }

        protected FetcherNavigationCollectionProperty(OdcmProperty odcmProperty)
            : base(odcmProperty)
        {
            CollectionType = new Type(NamesService.GetCollectionTypeName((OdcmClass)odcmProperty.Type));
            FieldName = NamesService.GetFetcherCollectionFieldName(odcmProperty);
            InstanceType = NamesService.GetConcreteTypeName(odcmProperty.Type);
            Type = new Type(NamesService.GetCollectionInterfaceName((OdcmClass)odcmProperty.Type));
        }

        public new static FetcherNavigationProperty ForConcrete(OdcmProperty odcmProperty)
        {
            return new FetcherNavigationCollectionProperty(odcmProperty)
            {
                DefiningInterface = NamesService.GetFetcherInterfaceName(odcmProperty.Class),
            };
        }

        public new static FetcherNavigationProperty ForFetcher(OdcmProperty odcmProperty)
        {
            return new FetcherNavigationCollectionProperty(odcmProperty);
        }
    }
}
