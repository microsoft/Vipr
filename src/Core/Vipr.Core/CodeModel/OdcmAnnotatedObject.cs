// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmAnnotatedObject : OdcmObject
    {
        public string PreText { get; set; }

        public string InText { get; set; }

        protected OdcmAnnotatedObject(string name)
            : base(name)
        {
        }
    }
}
