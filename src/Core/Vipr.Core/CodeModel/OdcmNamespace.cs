// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel
{
    public class OdcmNamespace : OdcmAnnotatedObject
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

        public IEnumerable<OdcmClass> Classes
        {
            get
            {
                return ClassesOf<OdcmClass>();
            }
        }

        public IEnumerable<T> ClassesOf<T>() where T : OdcmClass
        {
            return from odcmType in Types where odcmType is T select odcmType as T;
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
