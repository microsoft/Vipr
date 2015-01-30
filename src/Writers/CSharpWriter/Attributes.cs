// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class Attributes
    {
        public static IEnumerable<Attribute> ForConcrete(OdcmClass odcmClass)
        {
            return new[]
            {
                Attribute.ForMicrosoftOdataClientKey(odcmClass)
            };
        }
    }
}