// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class CSharpWriter : IOdcmWriter
    {
        public CSharpWriter()
        {
        }

        public IDictionary<string, string> GenerateProxy(OdcmModel model,
            IConfigurationProvider configurationProvider = null)
        {
            ConfigurationService.Initialize(configurationProvider);

            var csProject = new CSharpProject(model);

            var codeGenerator = new SourceCodeGenerator(model.ServiceType);

            return codeGenerator.Generate(csProject);
        }
    }
}
