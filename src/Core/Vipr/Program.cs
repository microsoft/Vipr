// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NLog;
using System;

namespace Vipr
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();

            try
            {
                bootstrapper.Start(args);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("VIPR").Error(ex);
            }
        }
    }
}
