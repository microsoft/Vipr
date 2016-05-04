// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmType : OdcmObject
    {
        private IDictionary<string, OdcmProjection> _projectionHash = new Dictionary<string, OdcmProjection>();

        public OdcmNamespace Namespace { get; set; }

        /// <summary>
        /// Cache of distinct OdcmProjections for this OdcmType
        /// </summary>
        public IEnumerable<OdcmProjection> Projections
        {
              get { return _projectionHash.Values; }
        }

        public IEnumerable<string> ProjectionKeys
        {
            get { return _projectionHash.Keys; }
        }

        public OdcmProjection DefaultProjection { get; private set; }

        protected OdcmType(string name, OdcmNamespace @namespace)
            : base(name)
        {
            Namespace = @namespace;
            DefaultProjection = new OdcmProjection()
            {
                Type = this,
                Capabilities = OdcmCapability.DefaultOdcmCapabilities
            };

            AddProjection(OdcmCapability.DefaultOdcmCapabilities);
        }

        public string FullName
        {
            get { return Namespace.Name + "." + Name; }
        }

        public override string CanonicalName()
        {
            return MakeCanonicalName(base.CanonicalName(), Namespace);
        }

        /// <summary>
        /// </summary>
        /// <param name="capabilities">A list of capabilities</param>
        /// <returns>A Projection of this OdcmType with the given capabilities</returns>
        public OdcmProjection GetProjection(ICollection<OdcmCapability> capabilities)
        {
            return new OdcmProjection()
            {
                Type = this,
                Capabilities = capabilities
            };
        }

        public void AddProjection(ICollection<OdcmCapability> capabilities, OdcmObject odcmObject = null)
        {
            if (this is OdcmPrimitiveType || this is OdcmMethod) return;

            var knownCapabilities = OdcmProjection.GetWellKnownCapabilities(odcmObject, capabilities).ToList();

            var name = OdcmProjection.GetUniqueProjectionName(knownCapabilities, odcmObject);

            OdcmProjection projection;
            if (!_projectionHash.TryGetValue(name, out projection))
            {
                _projectionHash[name] = new OdcmProjection()
                {
                    Type = this,
                    Capabilities = knownCapabilities,
                    BackLink = odcmObject
                };
            }
        }
    }
}