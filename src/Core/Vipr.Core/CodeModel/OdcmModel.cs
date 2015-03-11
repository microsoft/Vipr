// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Vipr.Core.CodeModel
{
    public class OdcmModel
    {
        private List<OdcmType> _Types = new List<OdcmType>();

        public List<OdcmNamespace> Namespaces { get; private set; }

        public IDictionary<string, string> ServiceMetadata { get; private set; } 

        public OdcmClass EntityContainer
        {
            get
            {
                return Namespaces
                    .SelectMany(n => n.Classes)
                    .FirstOrDefault(c => c is OdcmServiceClass);
            }
        }

        public IEnumerable<OdcmVocabularyAnnotation> VocabularyAnnotations { get; private set; }

        public ServiceType ServiceType { get; private set; }

        public OdcmModel(TextFileCollection serviceMetadata, ServiceType serviceType = ServiceType.ODataV4)
        {
            Namespaces = new List<OdcmNamespace>();
            ServiceMetadata = serviceMetadata.ToDictionary(f => f.RelativePath, f => f.Contents);
            ServiceType = serviceType;
        }

        public void AddNamespace(string @namespace)
        {
            OdcmNamespace ns;

            if (!TryResolveNamespace(@namespace, out ns))
            {
                Namespaces.Add(new OdcmNamespace(@namespace));
            }
        }

        public void AddType(OdcmType type)
        {
            string @namespace = type.Namespace.Name;
            OdcmNamespace odcmNamespace = null;
            foreach (OdcmNamespace candidate in Namespaces)
            {
                if (string.Equals(candidate.Name, @namespace))
                {
                    odcmNamespace = candidate;
                    break;
                }
            }
            if (odcmNamespace == null)
            {
                odcmNamespace = new OdcmNamespace(@namespace);
                Namespaces.Add(odcmNamespace);
            }

            _Types.Add(type);

            odcmNamespace.Types.Add(type);
        }

        public bool TryResolveType<T>(string name, string @namespace, out T type) where T : OdcmType
        {
            string canonicalName = OdcmObject.MakeCanonicalName(name, @namespace);

            foreach (OdcmType candidate in _Types)
            {
                if (candidate.CanonicalName().Equals(canonicalName, StringComparison.InvariantCulture))
                {
                    type = candidate as T;
                    return true;
                }
            }

            type = null;

            return false;
        }

        public bool TryResolveNamespace(string @namespace, out OdcmNamespace odcmNamespace)
        {
            foreach (OdcmNamespace candidate in Namespaces)
            {
                if (candidate.CanonicalName().Equals(@namespace, StringComparison.InvariantCulture))
                {
                    odcmNamespace = candidate;
                    return true;
                }
            }

            odcmNamespace = null;

            return false;
        }
    }

    public enum ServiceType
    {
        ODataV3,
        ODataV4
    }
}
