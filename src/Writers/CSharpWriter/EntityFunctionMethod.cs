// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class EntityFunctionMethod : ServerMethod
    {
        public Identifier InstanceName { get; set; }

        public EntityFunctionMethod(OdcmMethod odcmMethod) : base(odcmMethod)
        {
            InstanceName = NamesService.GetConcreteTypeName(odcmMethod.ReturnType);
            ReturnType = new Type(new Identifier("System.Threading.Tasks", "Task"), new Type(NamesService.GetPublicTypeName(odcmMethod.ReturnType)));
        }
    }
}