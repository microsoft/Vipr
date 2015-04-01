// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpWriter
{
    public class ContainerGetPathMethod : Method
    {
        public ContainerGetPathMethod()
        {
            Visibility = Visibility.Private;
            Name = "GetPath";
            Parameters = new[] { new Parameter(new Type(new Identifier("System", "String")), "propertyName"), };
            ReturnType = new Type(new Identifier("System", "String"));
        }
    }
}