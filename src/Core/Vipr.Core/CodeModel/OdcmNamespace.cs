// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel
{
    public class OdcmNamespace : OdcmObject
    {
        public List<OdcmType> Types { get; private set; }

        public OdcmNamespace(string name)
            : base(name)
        {
            Types = new List<OdcmType>();
        }

        public IEnumerable<OdcmEnum> Enums
        {
            get
            {
                return from odcmType in Types where odcmType is OdcmEnum select odcmType as OdcmEnum;
            }
        }

        public IEnumerable<OdcmClass> Classes
        {
            get
            {
                return from odcmType in Types where odcmType is OdcmClass select odcmType as OdcmClass;
            }
        }

        public bool HasBoundOperations(string fullNamespace)
        {
            return Classes.Any(c => c.Methods.Any());
        }
    }
}
