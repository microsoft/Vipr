using System;
using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    internal class CSharpProject
    {
        public IEnumerable<Namespace> Namespaces { get; set; }

        public CSharpProject(OdcmModel model)
        {
            Namespaces = global::CSharpWriter.Namespaces.ForModel(model);
        }
    }
}