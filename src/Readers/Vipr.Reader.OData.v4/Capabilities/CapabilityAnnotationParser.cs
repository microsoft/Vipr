// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Annotations;
using Vipr.Core.CodeModel;
using System.IO;
using System.Xml;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Csdl;

namespace Vipr.Reader.OData.v4.Capabilities
{
    public abstract class CapabilityAnnotationParser
    {
        protected PropertyCapabilitiesCache _propertyCapabilitiesCache;

        protected CapabilityAnnotationParser(PropertyCapabilitiesCache propertyCapabilitiesCache)
        {
            _propertyCapabilitiesCache = propertyCapabilitiesCache;
        }

        private static IEdmModel _capabilitiesModel;

        public static IEdmModel CapabilitiesModel
        {
            get
            {
                if (_capabilitiesModel == null)
                {
                    using (var reader = new StringReader(Properties.Resources.CapabilitiesVocabularies))
                    {
                        IEnumerable<EdmError> errors;
                        if (!CsdlReader.TryParse(new[] { XmlReader.Create(reader) }, out _capabilitiesModel, out errors))
                        {
                            throw new InvalidOperationException("Could not load capabilities vocabulary from resources");
                        }
                    }
                }

                return _capabilitiesModel;
            }
        }

        public abstract void ParseCapabilityAnnotationForEntitySet(OdcmProperty odcmEntitySet,
            IEdmValueAnnotation annotation);
    }
}