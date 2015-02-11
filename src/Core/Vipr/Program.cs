// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Vipr.Core;
using Mono.Options;

namespace Vipr
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ReaderConfigurationProvider readerConfig = new ReaderConfigurationProvider();
            WriterConfigurationProvider writerConfig = new WriterConfigurationProvider();
            OptionSet readerParser = new OptionSet() {
                // TODO: figure out how to not require assembly qualified names...
                {"reader:", v => readerConfig.Reader = (IViprReader) Type.GetType(v) },
                {"writer:", v => readerConfig.Writer = (IViprWriter) Type.GetType(v) },
                {"r", (k,v) => readerConfig.Options[k] = v}
            };

            OptionSet writerParser = new OptionSet() {
                // TODO: figure out how to not require assembly qualified names...
                {"reader:", v => writerConfig.Reader = (IViprReader) Type.GetType(v) },
                {"writer:", v => writerConfig.Writer = (IViprWriter) Type.GetType(v) },
                {"w", (k,v) => writerConfig.Options[k] = v },  // writer options
            };

            readerConfig.ValueOnlyOptions = (ISet<string>) readerParser.Parse(args);
            writerConfig.ValueOnlyOptions = (ISet<string>) writerParser.Parse(args);

            IViprReader reader = (IViprReader) Activator.CreateInstance((Type) readerConfig.Reader);
            IViprWriter writer = (IViprWriter) Activator.CreateInstance((Type) writerConfig.Writer);

            writer.GenerateProxy(reader.GenerateModel(readerConfig), writerConfig);
        }
    }
}
