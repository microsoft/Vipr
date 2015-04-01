// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
namespace Vipr
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();

            bootstrapper.Start(args);
        }
    }
}
