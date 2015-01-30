// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;

namespace Vipr.Core
{
    public interface IConfigurationProvider
    {
        IImmutableDictionary<string, string> OdcmNamespaceToProxyNamespace { get; }

        IImmutableDictionary<string, IImmutableDictionary<string, string>> OdcmClassNameToProxyClassName { get; }

        string NamespacePrefix { get; }

        bool OmitFetcherCastMethods { get; }

        bool ForcePropertyPascalCasing { get; }
    }
}
