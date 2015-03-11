// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public abstract class Method : MethodSignature
    {
        public string ModelName { get; protected set; }

        public static Method ForEntityType(OdcmMethod odcmMethod)
        {
            return odcmMethod.ReturnType == null
                ? (Method) new EntityVoidMethod(odcmMethod)
                : odcmMethod.IsCollection
                    ? (Method) new EntityCollectionFunctionMethod(odcmMethod)
                    : (Method) new EntityInstanceFunctionMethod(odcmMethod);
        }
    }
}
