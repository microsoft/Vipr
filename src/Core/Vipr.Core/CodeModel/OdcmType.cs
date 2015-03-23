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

        protected OdcmType(string name, OdcmNamespace @namespace)
            : base(name)
        {
            Namespace = @namespace;
            _projections = new List<OdcmProjection>();
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
        /// Given a list of capabilities return an OdcmProjection for this OdcmType
        /// </summary>
        /// <param name="capabilities"></param>
        /// <returns></returns>
        public OdcmProjection GetProjection(List<OdcmCapability> capabilities)
        {
            //Find if we already have a 'Projection' for the given capabilities.
            OdcmProjection projection =
                _projections.Find(
                    odcmProjection =>
                        capabilities.Count == odcmProjection.Capabilities.Count &&
                        odcmProjection.Capabilities.All(capabilities.Contains));

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