// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel
{
    public class OdcmNamespace : OdcmObject
    {
        private static readonly OdcmNamespace EdmNamespace = new OdcmNamespace("Edm");

        public List<OdcmType> Types { get; private set; }

        public OdcmNamespace(string name)
            : base(name)
        {
            Types = new List<OdcmType>();
        }

        public IEnumerable<OdcmEnum> Enums
        {
            get
            {
                return from odcmType in Types where odcmType is OdcmEnum select odcmType as OdcmEnum;
            }
        }

        public IEnumerable<OdcmTypeDefinition> TypeDefinitions
        {
            get
            {
                return from odcmTypeDefinition in Types where odcmTypeDefinition is OdcmTypeDefinition select odcmTypeDefinition as OdcmTypeDefinition;
            }
        }

        public IEnumerable<OdcmClass> Classes
        {
            get
            {
                return from odcmType in Types where odcmType is OdcmClass select odcmType as OdcmClass;
            }
        }

        public static OdcmNamespace Edm { get { return EdmNamespace; } }

        public bool HasBoundOperations(string fullNamespace)
        {
            return Classes.Any(c => c.Methods.Any());
        }

        public static OdcmNamespace GetWellKnownNamespace(string @namespace)
        {
            switch (@namespace)
            {
                case "Edm":
                    return EdmNamespace;
                default:
                    throw new InvalidOperationException(String.Format("The namespace {0} is not known.", @namespace));
            }
        }
    }
}
