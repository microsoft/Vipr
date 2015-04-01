// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CSharpWriter
{
    public class ContainerGetUrlMethod : Method
    {
        public ContainerGetUrlMethod()
        {
            Visibility = Visibility.Private;
            Name = "GetUrl";
            Parameters = new Parameter[] { };
            ReturnType = new Type(new Identifier("System", "Uri"));
        }
    }
}