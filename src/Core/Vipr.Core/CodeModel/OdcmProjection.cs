// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Core.CodeModel
{
    public class OdcmProjection
    {
        public OdcmType Type { get; set; }

        public IList<OdcmCapability> Capabilities { get; set; }
    }
}
