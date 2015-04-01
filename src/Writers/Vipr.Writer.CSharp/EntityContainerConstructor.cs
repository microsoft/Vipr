// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class EntityContainerConstructor : Constructor
    {
        public EntityContainerConstructor(OdcmClass odcmContainer)
        {
            Name = NamesService.GetContainerName(odcmContainer).Name;
        }
    }
}