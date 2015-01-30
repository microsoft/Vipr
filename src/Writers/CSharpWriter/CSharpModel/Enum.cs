// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;

namespace CSharpWriter.CSharpModel
{
    public class Enum
    {
        public Identifier Identifier;

        public static Enum Map(OdcmEnum odcmEnum)
        {
            return new Enum
            {
            };
        }
    }
}
