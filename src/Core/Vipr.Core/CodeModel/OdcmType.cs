// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmType : OdcmObject
    {
        private List<OdcmProjection> _projections;

        public OdcmNamespace Namespace { get; set; }

        /// <summary>
        /// Cache of OdcmProjections for this OdcmType
        /// </summary>
        public IEnumerable<OdcmProjection> Projections
        {
            get { return _projections.AsEnumerable(); }
        }

        public OdcmProjection DefaultProjection { get; private set; }

        protected OdcmType(string name, OdcmNamespace @namespace)
            : base(name)
        {
            Namespace = @namespace;
            _projections = new List<OdcmProjection>();
            DefaultProjection = new OdcmProjection()
            {
                Type = this,
                Capabilities = OdcmCapability.DefaultOdcmCapabilities
            };
            _projections.Add(DefaultProjection);
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
        /// OdcmType maintains a cache of its Projections. 
        /// This method will return a Projection from the cache with the same capabilities.
        /// If the cache does not have the required Projection, a new Projection is created and add to the cache.
        /// </summary>
        /// <param name="capabilities">A list of capabilities</param>
        /// <returns>A Projection of this OdcmType with the given capabilities</returns>
        public OdcmProjection GetProjection(IEnumerable<OdcmCapability> capabilities)
        {
            //Find if we already have a 'Projection' for the given capabilities.
            OdcmProjection projection =
                _projections.SingleOrDefault(
                    odcmProjection => odcmProjection.ContainsAllCapabilities(capabilities));

            if (projection == null)
            {
                projection = new OdcmProjection()
                {
                    Type = this,
                    Capabilities = capabilities
                };
                _projections.Add(projection);
            }

            return projection;
        }
    }
}