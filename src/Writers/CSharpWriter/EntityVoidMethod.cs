// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class EntityVoidMethod : ServerMethod
    {
        public EntityVoidMethod(OdcmMethod odcmMethod) : base(odcmMethod)
        {
            ReturnType = new Type(new Identifier("System.Threading.Tasks", "Task"));
        }
    }
}