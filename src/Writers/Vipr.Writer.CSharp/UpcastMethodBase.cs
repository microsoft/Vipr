// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public abstract class UpcastMethodBase : Method
    {
        public OdcmType BaseType { get; private set; }
        public OdcmType DerivedType { get; private set; }

        public UpcastMethodBase(OdcmType baseType, OdcmType derivedType)
        {
            BaseType = baseType;
            DerivedType = derivedType;
            Visibility = Visibility.Public;
        }
    }
}