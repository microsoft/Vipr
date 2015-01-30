// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class EntityVoidMethod : Method
    {
        public EntityVoidMethod(OdcmMethod odcmMethod)
        {
            IsAsync = true;
            ModelName = odcmMethod.Name;
            Name = odcmMethod.Name + "Async";
            Parameters = odcmMethod.Parameters.Select(Parameter.FromOdcmParameter);
            ReturnType = new Type(new Identifier("System.Threading.Tasks", "Task"));
        }
    }
}