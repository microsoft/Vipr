// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    internal class Namespace
    {
        public string Name { get; set; }
        public IEnumerable<Feature> Features { get; set; }
        public IEnumerable<Class> Classes { get { return Features.SelectMany(f => f.Classes); } }
        public IEnumerable<Interface> Interfaces { get { return Features.SelectMany(f => f.Interfaces); } }
        public IEnumerable<Enum> Enums { get { return Features.SelectMany(f => f.Enums); } }

        public Namespace(OdcmNamespace @namespace, OdcmModel model)
        {
            Name = NamesService.GetNamespaceName(@namespace);
            //Features = @namespace.Enums.SelectMany(global::Vipr.Writer.CSharp.Features.ForOdcmEnum)
            //    .Concat(@namespace.Classes.SelectMany(global::Vipr.Writer.CSharp.Features.ForOdcmClass))
            //    .Concat(@namespace.Classes.SelectMany(c => global::Vipr.Writer.CSharp.Features.ForEntityContainer(c, model)));
            Features = @namespace.Enums.SelectMany(global::Vipr.Writer.CSharp.Features.ForOdcmEnum)
                .Concat(@namespace.Classes.OfType<OdcmComplexClass>().SelectMany(global::Vipr.Writer.CSharp.Features.ForOdcmClass))
                .Concat(@namespace.Classes.OfType<OdcmEntityClass>().SelectMany(global::Vipr.Writer.CSharp.Features.ForOdcmClass))
                .Concat(@namespace.Classes.OfType<OdcmServiceClass>().SelectMany(c => global::Vipr.Writer.CSharp.Features.ForEntityContainer(c, model)));
        }
    }
}