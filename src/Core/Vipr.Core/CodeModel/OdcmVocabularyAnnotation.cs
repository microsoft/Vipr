// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Vipr.Core.CodeModel
{
    /// <summary>
    /// Represents an instance of an OData Vocabulary Annotation. 
    /// OData Vocabulary Annotations are composed of a Term,
    /// which includes their namespace and their name, and their value. 
    /// Values may be enumerations, primitive types, or complex types. 
    /// As such; the value of the VocabularyAnnotation is reprsented as an object 
    /// and must be cast by the consumer of the OcdmVocabularyAnnotation object.  
    /// </summary>
    /// <remarks>
    /// Presently, support for annotations that are defined in the Core and 
    /// Capabilties vocabularies are supported most completely.  
    /// </remarks>
    public class OdcmVocabularyAnnotation
    {
        /// <summary>
        /// The fully qualified namespace the VocabularyAnnotation is defined within
        /// </summary>
        /// <example>Org.OData.Core.V1</example>
        /// <example>Org.OData.Capabilities.V1</example>
        public string Namespace { get; set; }

        /// <summary>
        /// The name of the type of the VocabularyAnnotation
        /// </summary>
        /// <example>Computed</example>
        /// <example>CallbackSupported</example>
        /// <example>InsertRestrictions</example>
        public string Name { get; set; }

        /// <summary>
        /// An instance of an object which represents the value of the VocabularyAnnotation.
        /// May be a C# primitive type, enumeration, complex type, or a collection. 
        /// If there is a mapping between the EDM model and a C# object, such as an instance of "InsertRestrictions", the result of that mapping will be stored as the value. 
        /// Otherwise, the most general deserialization of the value will be applied; or a NotImplementedException will be thrown in the case that such deserialization fails. 
        /// </summary>
        public object Value { get; set; }
    }
}