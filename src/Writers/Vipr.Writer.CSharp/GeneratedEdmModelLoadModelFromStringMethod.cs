// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Writer.CSharp
{
    public class GeneratedEdmModelLoadModelFromStringMethod : Method
    {
        public GeneratedEdmModelLoadModelFromStringMethod()
        {
            Visibility = Visibility.Private;
            IsStatic = true;
            Name = "LoadModelFromString";
            ReturnType = new Type(new Identifier("global::Microsoft.OData.Edm", "IEdmModel"));
        }
    }
}