// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CSharp;
using Microsoft.Its.Recipes;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace CSharpWriterUnitTests
{
    public class XMLDocumentTestBase : CodeGenTestBase
    {
        public string GetProxyXmlDocumentContent(OdcmModel model, IConfigurationProvider configurationProvider = null, IEnumerable<string> internalsVisibleTo = null)
        {
            var proxySources = GetProxySources(model, configurationProvider, internalsVisibleTo);
            return GetXmlDocumentContent(ReferencedAssemblies, proxySources.Select(f => f.Contents).ToArray());
        }

        private string GetXmlDocumentContent(IEnumerable<string> referencedAssemblies, params string[] cSharpSources)
        {
            string xmlDocument = Path.ChangeExtension(Any.Word(), "xml");
            if (File.Exists(xmlDocument))
            {
                File.Delete(xmlDocument);
            }

            var compilerParams = GetCompilerParameters();
            compilerParams.CompilerOptions += " /doc:" + xmlDocument;
            compilerParams.TreatWarningsAsErrors = false;
            compilerParams.ReferencedAssemblies.AddRange(new[] { "System.dll" });

            if (referencedAssemblies != null)
            {
                compilerParams.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());
            }

            var provider = new CSharpCodeProvider();
            var compile = provider.CompileAssemblyFromSource(compilerParams, cSharpSources);

            if (!compile.Errors.HasErrors)
            {
                if (!File.Exists(xmlDocument))
                {
                    throw new Exception("XML Document Doc was not created by the compiler for a successful compilation.");
                }

                var xmlContent = File.ReadAllText(xmlDocument);
                File.Delete(xmlDocument);
                return xmlContent;
            }

            var text = compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (c, ce) => c + ("\r\n" + ce.ToString()));
            throw new Exception(text);
        }
    }
}
