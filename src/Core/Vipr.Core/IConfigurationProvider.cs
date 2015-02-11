// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core
{
    public interface IConfigurationProvider
    {
        IViprReader Reader { get; set; }

        IViprWriter Writer { get; set; }

        /// <summary>
        /// Options of type key=value, specified on command line as '-k v' or '--key value'
        /// Also options of type key only, specified on command line as '-k' or '--key' and with value null.
        /// </summary>
        IDictionary<string, string> Options { get; }


        /// <summary>
        /// Options of type value, specified on command line as 'value' with no preceding key.
        /// </summary>
        ISet<string> ValueOnlyOptions { get; }
        
    }
}
