// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal static class Namespaces
    {
        internal static IEnumerable<Namespace> ForModel(OdcmModel model)
        {
            return model.Namespaces.Where(n => !n.Name.Equals("edm", StringComparison.OrdinalIgnoreCase))
                .Select(n => new Namespace(n, model));
        }
    }
}