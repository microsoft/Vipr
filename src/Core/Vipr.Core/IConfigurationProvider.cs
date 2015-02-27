// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core
{
    public interface IConfigurationProvider
    {
        T GetConfiguration<T>();
    }
}
