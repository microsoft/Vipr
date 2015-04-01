// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;


namespace CSharpWriter
{
    class AddAsyncMediaMethod : Method
    {
        public AddAsyncMediaMethod(OdcmClass odcmClass)
        {
            Name = "Add" + odcmClass.Name + "Async";

            Parameters = new[]
            {
                new Parameter(new Type(NamesService.GetConcreteInterfaceName(odcmClass)), "item"),
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("IStream")), "stream"),
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("String")), "contentType"),
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("Boolean")), "deferSaveChanges", "false"),
                new Parameter(new Type(NamesService.GetPrimitiveTypeName("Boolean")), "closeStream", "false")
            };

            ReturnType = new Type(Identifier.Task);

            Visibility = ConfigurationService.Settings.MediaEntityAddAsyncVisibility;
        }
    }
}
