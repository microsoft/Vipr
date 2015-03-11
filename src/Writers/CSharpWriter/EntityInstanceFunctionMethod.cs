// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class EntityInstanceFunctionMethod : ServerMethod
    {
        public Identifier InstanceName { get; set; }

        public EntityInstanceFunctionMethod(OdcmMethod odcmMethod) : base(odcmMethod)
        {
            InstanceName = NamesService.GetConcreteTypeName(odcmMethod.ReturnType);
            ReturnType = Type.TaskOf(new Type(NamesService.GetPublicTypeName(odcmMethod.ReturnType)));
        }
    }
}