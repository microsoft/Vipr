// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Vipr.Core.CodeModel
{
    public abstract class OdcmObject
    {
        public string Name { get; private set; }

        /// <summary>
        /// A collection of VocabularyAnnotations that have been applied to this OdcmAnnotatedObject.
        /// </summary>
        public List<OdcmVocabularyAnnotation> Annotations { get; set; }

        public OdcmObject(string name)
        {
            Name = name;
            Annotations = new List<OdcmVocabularyAnnotation>();
        }
    }
}
