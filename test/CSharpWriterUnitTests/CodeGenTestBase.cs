// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Its.Recipes;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Moq;

namespace CSharpWriterUnitTests
{
    public class CodeGenTestBase
    {
        public const BindingFlags PermissiveBindingFlags =
            BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.Static
            | BindingFlags.Instance
            | BindingFlags.FlattenHierarchy;

        public const BindingFlags ExplicitBindingFlags =
              BindingFlags.NonPublic
            | BindingFlags.Instance;

        public static readonly IDictionary<string, Type> EdmToClrTypeMap =
            new Dictionary<string, Type>
            {
                {"Binary", typeof (byte[])},
                {"Boolean", typeof (bool)},
                {"Byte", typeof (byte)},
                {"Date", typeof (DateTimeOffset)},
                {"DateTimeOffset", typeof (DateTimeOffset)},
                {"Decimal", typeof (decimal)},
                {"Double", typeof (double)},
                {"Duration", typeof (TimeSpan)},
                {"Guid", typeof (Guid)},
                {"Int16", typeof (short)},
                {"Int32", typeof (int)},
                {"Int64", typeof (long)},
                {"SByte", typeof (sbyte)},
                {"Single", typeof (float)},
                {"Stream", typeof (Microsoft.OData.Client.DataServiceStreamLink)},
                {"String", typeof (string)},
                {"TimeOfDay", typeof (DateTimeOffset)},
            };

        protected List<string> ReferencedAssemblies = new List<string>
        {
                typeof(Microsoft.OData.Client.BaseEntityType).Assembly.Location,
                typeof(Microsoft.OData.Edm.EdmConcurrencyMode).Assembly.Location,
                typeof(Microsoft.OData.ProxyExtensions.LowerCasePropertyAttribute).Assembly.Location,
                "System.dll",
                "System.Core.dll",
                "System.Diagnostics.Debug.dll",
                "System.IO.dll",
                "System.Linq.dll",
                "System.Linq.Expressions.dll",
                "System.ObjectModel.dll",
                "System.Reflection.dll",
                "System.Runtime.dll",
                "System.Threading.Tasks.dll",
                "System.Xml.ReaderWriter.dll",
                "System.Xml.dll"
            };

        public Assembly GetProxy(OdcmModel model, IConfigurationProvider configurationProvider = null, IEnumerable<string> internalsVisibleTo = null)
        {
            var proxySources = GetProxySources(model, configurationProvider, internalsVisibleTo);
            return CompileText(ReferencedAssemblies, proxySources.Select(f => f.Contents).ToArray());
        }

        private static void WriteProxySource(TextFileCollection proxySources)
        {
            if(Debugger.IsAttached)
                foreach (var sourceFile in proxySources)
                {
                    Debug.WriteLine("-------- {0} ------", sourceFile.RelativePath);
                    Debug.WriteLine(sourceFile.Contents);
                    Debug.WriteLine("-------------------", sourceFile.RelativePath);
                }
        }

        protected TextFileCollection GetProxySources(OdcmModel model, IConfigurationProvider configurationProvider = null, IEnumerable<string> internalsVisibleTo = null)
        {
            if (model.ServiceMetadata["$metadata"] == TestConstants.ODataV4.EmptyEdmx)
                model.ServiceMetadata["$metadata"] = model.ToEdmx(true);

            var writer = new CSharpWriter.CSharpWriter();

            writer.SetConfigurationProvider(configurationProvider);

            var proxySources = writer.GenerateProxy(model);

            if (internalsVisibleTo != null && internalsVisibleTo.Any())
            {
                var internalsHeader =
                    "using System.Runtime.CompilerServices;\n\n" +
                    internalsVisibleTo
                        .Select(s => string.Format("[assembly: InternalsVisibleTo(\"{0}\")]\n\n", s))
                        .Aggregate((a, i) => a + i);

                var originalProxySources = proxySources;

                proxySources = new TextFileCollection();
                proxySources.AddRange(originalProxySources.Select(file => new TextFile(file.RelativePath, internalsHeader + file.Contents)));
            }

            WriteProxySource(proxySources);
            return proxySources;
        }

        public Assembly CompileText(IEnumerable<string> referencedAssemblies, params string[] cSharpSources)
        {
            var compilerParams = GetCompilerParameters();

            var profileDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "Reference Assemblies\\Microsoft\\Framework\\.NETPortable\\v4.5\\Profile\\Profile259\\");

            if (referencedAssemblies != null) compilerParams.ReferencedAssemblies.AddRange(referencedAssemblies.Select(r => Path.IsPathRooted(r) ? r : profileDir + r).ToArray());

            var provider = new CSharpCodeProvider();

            var compile = provider.CompileAssemblyFromSource(compilerParams, cSharpSources);

            if (!compile.Errors.HasErrors) return compile.CompiledAssembly;

            var text = compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (c, ce) => c + ("\r\n" + ce.ToString()));

            throw new Exception(text);
        }

        protected static CompilerParameters GetCompilerParameters()
        {
            if (Debugger.IsAttached)
            {
                return new CompilerParameters
                {
                    CompilerOptions = "/debug:pdbonly",
                    GenerateInMemory = false,
                    IncludeDebugInformation = true,
                    TempFiles = new TempFileCollection(AppDomain.CurrentDomain.BaseDirectory, true),
                    TreatWarningsAsErrors = false,
                };
            }
            else
            {
                return new CompilerParameters
                {
                    CompilerOptions = "/optimize",
                    GenerateExecutable = false,
                    GenerateInMemory = true,
                    TreatWarningsAsErrors = true,
                };
            }
        }

        protected static T GetValue<T>(Assembly asm, string methodName, string typeName, string @namespace) where T : class
        {
            return GetValue<T>(asm, methodName, @namespace + "." + typeName);
        }

        protected static T GetValue<T>(Assembly asm, string methodName, string typeName) where T : class
        {
            var mt = asm.GetType(typeName);
            var methInfo = mt.GetMethod(methodName);
            var ci = mt.GetConstructor(new Type[] { });

            var instance = ci.Invoke(new object[] { });

            return methInfo.Invoke(instance, new object[] { }) as T;
        }
    }
}
