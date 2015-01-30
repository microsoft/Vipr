// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class EntityFunctionMethod : Method
    {
        public EntityFunctionMethod(OdcmMethod odcmMethod)
        {
            InstanceName = NamesService.GetConcreteTypeName(odcmMethod.ReturnType);
            IsAsync = true;
            ModelName = odcmMethod.Name;
            Name = odcmMethod.Name + "Async";
            Parameters = odcmMethod.Parameters.Select(Parameter.FromOdcmParameter);
            ReturnType = new Type(new Identifier("System.Threading.Tasks", "Task"), new Type(NamesService.GetPublicTypeName(odcmMethod.ReturnType)));
        }

        public Identifier InstanceName { get; set; }
    }
}