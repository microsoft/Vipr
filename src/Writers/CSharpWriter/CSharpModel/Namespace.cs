// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel;

namespace CSharpWriter.CSharpModel
{
    public class Namespace
    {
        public IEnumerable<Namespace> Dependencies { get; private set; }

        public IEnumerable<Class> Classes { get; private set; }

        public IEnumerable<Enum> Enums { get; private set; }

        public Identifier Identifier { get; private set; }

        public static Namespace Map(OdcmNamespace odcmNamespace)
        {
            return new Namespace(odcmNamespace.Name)
            {
                Enums = odcmNamespace.Enums.Select(Enum.Map)
            };
        }

        private Namespace()
        {
            Classes = new List<Class>();
            Dependencies = CommonDependencies;
        }

        private Namespace(string fullName) : this()
        {
            Identifier = new Identifier(string.Empty, fullName);
        }

        private Namespace(string fullName, IEnumerable<Namespace> dependencies) : this(fullName)
        {
            Dependencies.Concat(dependencies);
        }

        private IEnumerable<Namespace> CommonDependencies
        {
            get
            {
                return new List<Namespace>
                {
                    new Namespace("global::Microsoft.OData.Client"),
                    new Namespace("global::Microsoft.OData.Core"),
                    new Namespace("System"),
                    new Namespace("System.Collections.Generic"),
                    new Namespace("System.ComponentModel"),
                    new Namespace("System.Linq"),
                    new Namespace("System.Reflection"),
                    new Namespace(NamesService.ExtensionNamespace)
                };
            }
        }
    }
}
