// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Annotations;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Expressions;
using Microsoft.OData.Edm.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace ODataReader.v4
{
    internal static class ODataCapabilitiesReader
    {
        private static IEdmModel s_capabilitiesModel;
        private static readonly IEdmValueTerm s_insertRestrictionsTerm;
        private static readonly IEdmValueTerm s_updateRestrictionsTerm;
        private static readonly IEdmValueTerm s_deleteRestrictionsTerm;
        private static readonly IEdmValueTerm s_expandRestrictionsTerm;

        const string CapabilitiesInsertRestrictions = "Org.OData.Capabilities.V1.InsertRestrictions";
        const string CapabilitiesUpdateRestrictions = "Org.OData.Capabilities.V1.UpdateRestrictions";
        const string CapabilitiesDeleteRestrictions = "Org.OData.Capabilities.V1.DeleteRestrictions";
        const string CapabilitiesExpandRestrictions = "Org.OData.Capabilities.V1.ExpandRestrictions";
        
        static ODataCapabilitiesReader()
        {
            using(var reader = new StringReader(Properties.Resources.CapabilitiesVocabularies))
            {
                IEnumerable<EdmError> errors;
                if (!CsdlReader.TryParse(new[] { XmlReader.Create(reader) }, out s_capabilitiesModel, out errors))
                {
                    throw new InvalidOperationException("Could not load capabilities vocabulary from resources");
                }
            }

            s_insertRestrictionsTerm = s_capabilitiesModel.FindDeclaredValueTerm(CapabilitiesInsertRestrictions);
            s_updateRestrictionsTerm = s_capabilitiesModel.FindDeclaredValueTerm(CapabilitiesUpdateRestrictions);
            s_deleteRestrictionsTerm = s_capabilitiesModel.FindDeclaredValueTerm(CapabilitiesDeleteRestrictions);
            s_expandRestrictionsTerm = s_capabilitiesModel.FindDeclaredValueTerm(CapabilitiesExpandRestrictions);
        }

        internal static void GetCapabilitiesForEntitySet(OdcmProperty odcmProperty, IEdmModel edmModel, IEdmEntitySet entitySet)
        {
            if (odcmProperty == null) throw new ArgumentNullException("odcmProperty");
            if (edmModel == null) throw new ArgumentNullException("edmModel");
            if (entitySet == null) throw new ArgumentNullException("entitySet");

            GetInsertRestrictions(odcmProperty, edmModel, entitySet);
            GetUpdateRestrictions(odcmProperty, edmModel, entitySet);
            GetDeleteRestrictions(odcmProperty, edmModel, entitySet);
            GetExpandRestrictions(odcmProperty, edmModel, entitySet);
        }

        private static void GetInsertRestrictions(OdcmProperty odcmProperty, IEdmModel model, IEdmEntitySet entitySet)
        {
            IEdmValueAnnotation insertAnnotation = FindVocabularyAnnotation(model, entitySet, s_insertRestrictionsTerm);
            if(insertAnnotation == null)
            {
                return;
            }
            
            bool insertable;
            IEnumerable<string> nonInsertableNavigationProperties;
            GetBooleanAndPathCollection(insertAnnotation, "Insertable", "NonInsertableNavigationProperties", out insertable, out nonInsertableNavigationProperties);
            odcmProperty.Projection.EnsureCapabilities().Insertable = insertable;

            foreach (var propertyPath in nonInsertableNavigationProperties)
            {
                OdcmProperty navProperty;
                if (!(odcmProperty.Type as OdcmClass).TryFindProperty(propertyPath, out navProperty))
                {
                    throw new InvalidOperationException();
                }
                navProperty.Projection.EnsureCapabilities().Insertable = false;
            }
        }

        private static void GetUpdateRestrictions(OdcmProperty odcmProperty, IEdmModel model, IEdmEntitySet entitySet)
        {
            IEdmValueAnnotation updateAnnotation = FindVocabularyAnnotation(model, entitySet, s_updateRestrictionsTerm);
            if (updateAnnotation == null)
            {
                return;
            }

            bool updatable;
            IEnumerable<string> nonupdatableNavigationProperties;
            GetBooleanAndPathCollection(updateAnnotation, "Updatable", "NonUpdatableNavigationProperties", out updatable, out nonupdatableNavigationProperties);
            odcmProperty.Projection.EnsureCapabilities().Updateable = updatable;

            foreach (var propertyPath in nonupdatableNavigationProperties)
            {
                OdcmProperty navProperty;
                if (!(odcmProperty.Type as OdcmClass).TryFindProperty(propertyPath, out navProperty))
                {
                    throw new InvalidOperationException();
                }
                navProperty.Projection.EnsureCapabilities().Updateable = false;
            }
        }

        private static void GetDeleteRestrictions(OdcmProperty odcmProperty, IEdmModel model, IEdmEntitySet entitySet)
        {
            IEdmValueAnnotation deleteAnnotation = FindVocabularyAnnotation(model, entitySet, s_deleteRestrictionsTerm);
            if (deleteAnnotation == null)
            {
                return;
            }

            bool deletable;
            IEnumerable<string> nonDeletableNavigationProperties;
            GetBooleanAndPathCollection(deleteAnnotation, "Deletable", "NonDeletableNavigationProperties", out deletable, out nonDeletableNavigationProperties);
            odcmProperty.Projection.EnsureCapabilities().Deleteable = deletable;

            foreach (var propertyPath in nonDeletableNavigationProperties)
            {
                OdcmProperty navProperty;
                if (!(odcmProperty.Type as OdcmClass).TryFindProperty(propertyPath, out navProperty))
                {
                    throw new InvalidOperationException();
                }
                navProperty.Projection.EnsureCapabilities().Deleteable = false;
            }
        }

        private static void GetExpandRestrictions(OdcmProperty odcmProperty, IEdmModel model, IEdmEntitySet entitySet)
        {
            IEdmValueAnnotation expandAnnotation = FindVocabularyAnnotation(model, entitySet, s_expandRestrictionsTerm);
            if (expandAnnotation == null)
            {
                return;
            }

            bool expandable;
            IEnumerable<string> nonExpandableProperties;
            GetBooleanAndPathCollection(expandAnnotation, "Expandable", "NonExpandableProperties", out expandable, out nonExpandableProperties);
            odcmProperty.Projection.EnsureCapabilities().Expandable = expandable;

            foreach (var propertyPath in nonExpandableProperties)
            {
                OdcmProperty navProperty;
                if (!(odcmProperty.Type as OdcmClass).TryFindProperty(propertyPath, out navProperty))
                {
                    throw new InvalidOperationException();
                }
                navProperty.Projection.EnsureCapabilities().Expandable = false;
            }
        }

        private static void GetBooleanAndPathCollection(IEdmValueAnnotation annotation, string booleanPropertyName, string pathsPropertyName, out bool boolean, out IEnumerable<string> paths)
        {
            paths = new List<string>();            

            var recordExpression = (IEdmRecordExpression)annotation.Value;
            var booleanExpression = (IEdmBooleanConstantExpression)recordExpression.Properties.Single(p => p.Name == booleanPropertyName).Value;
            var collectionExpression = (IEdmCollectionExpression)recordExpression.Properties.Single(p => p.Name == pathsPropertyName).Value;

            foreach (IEdmPathExpression pathExpression in collectionExpression.Elements)
            {
                var pathBuilder = new StringBuilder();
                foreach (var path in pathExpression.Path)
                {
                    pathBuilder.AppendFormat("{0}.", path);
                }

                pathBuilder.Remove(pathBuilder.Length - 1, 1);

                ((List<string>)paths).Add(pathBuilder.ToString());
            }

            boolean = booleanExpression.Value;
        }

        private static IEdmValueAnnotation FindVocabularyAnnotation(IEdmModel model, IEdmVocabularyAnnotatable target, IEdmValueTerm term)
        {
            var result = default(IEdmValueAnnotation);

            var annotations = model.FindVocabularyAnnotations(target);
            if (annotations != null)
            {
                var annotation = annotations.FirstOrDefault(a => a.Term.Namespace == term.Namespace && a.Term.Name == term.Name);
                result = (IEdmValueAnnotation)annotation;
            }

            return result;
        }
    }
}
