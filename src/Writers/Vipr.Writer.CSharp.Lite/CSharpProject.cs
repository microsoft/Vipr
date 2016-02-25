// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp.Lite
{
    internal class CSharpProject
    {
        public IEnumerable<Namespace> Namespaces { get; set; }

        public CSharpProject(OdcmModel model)
        {
            Namespaces = global::Vipr.Writer.CSharp.Lite.Namespaces.ForModel(model);
        }
    }
}