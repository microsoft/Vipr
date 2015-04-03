// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using FluentAssertions;

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Library;
using Microsoft.OData.Edm.Library.Values;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Values;

using Xunit;

using Vipr.Reader.OData.v4;

using Vipr.Core.CodeModel;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace ODataReader.v4UnitTests
{
    public class Given_An_ODataVocabularyParser
    {
        public Given_An_ODataVocabularyParser()
        {
        }

        [Xunit.Fact]
        public void Boolean_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> booleanConstantPredicate = o => new EdmBooleanConstant((bool) o);

            ConstantValueShouldRoundtrip(true, booleanConstantPredicate);
            ConstantValueShouldRoundtrip(false, booleanConstantPredicate);
        }

        [Fact]
        public void String_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> stringPredicate = o => new EdmStringConstant((string) o);
            ConstantValueShouldRoundtrip("♪♪ unicode test string ♪♪", stringPredicate);
        }

        [Fact]
        public void Guid_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> guidConstantPredicate = o => new EdmGuidConstant((Guid) o);
            ConstantValueShouldRoundtrip(Guid.NewGuid(), guidConstantPredicate);
        }

        [Fact]
        public void Binary_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> binaryConstantPredicate = o => new EdmBinaryConstant((byte[]) o);
            ConstantValueShouldRoundtrip(new byte[] {1, 3, 3, 7}, binaryConstantPredicate);
        }

        [Fact]
        public void Date_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> dateConstantPredicate = o => new EdmDateConstant((Date) o);
            ConstantValueShouldRoundtrip(Date.Now, dateConstantPredicate);
        }

        [Fact]
        public void DateOffset_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> dateTimeOffsetConstantPredicate =
                o => new EdmDateTimeOffsetConstant((DateTimeOffset) o);
            ConstantValueShouldRoundtrip(DateTimeOffset.Now, dateTimeOffsetConstantPredicate);
            ConstantValueShouldRoundtrip(DateTimeOffset.UtcNow, dateTimeOffsetConstantPredicate);
        }

        [Fact]
        public void Decimal_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> decimalConstantValuePredicate = o => new EdmDecimalConstant((decimal) o);
            ConstantValueShouldRoundtrip((decimal) 1.234, decimalConstantValuePredicate);
        }

        [Fact]
        public void Floating_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> floatingConstantValuePredicate = o => new EdmFloatingConstant((double) o);
            ConstantValueShouldRoundtrip(1.234d, floatingConstantValuePredicate);
        }

        [Fact]
        public void Integer_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> integerConstantValuePredicate = o => new EdmIntegerConstant((long) o);
            ConstantValueShouldRoundtrip(123400L, integerConstantValuePredicate);
        }

        [Fact]
        public void Duration_constant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> durationConstantValuePredicate = o => new EdmDurationConstant((TimeSpan) o);
            ConstantValueShouldRoundtrip(new TimeSpan(1, 2, 3, 4), durationConstantValuePredicate);
        }

        [Fact]
        public void TimeOfDay_contant_values_map_to_their_clr_values()
        {
            Func<object, IEdmValue> timeOfDayConstantValuePredicate = o => new EdmTimeOfDayConstant((TimeOfDay) o);
            ConstantValueShouldRoundtrip(TimeOfDay.Now, timeOfDayConstantValuePredicate);
        }

        private void ConstantValueShouldRoundtrip(object originalValue, Func<object, IEdmValue> valuePredicate)
        {
            // Map our original value to an IEdmValue with the supplied predicate
            var iEdmValue = valuePredicate(originalValue);
            var result = ODataVocabularyReader.MapToClr(iEdmValue);
            result.Should()
                .Be(originalValue, "because a given clr value should roundtrip through its appropriate edm value ");
        }

        [Fact]
        public void Validly_annotated_Edm_will_correctly_parse_insert_restrictions()
        {
            var annotations = GetAnnotationsFromOneNoteSampleEntitySet("sections");

            annotations.Should().HaveCount(4, "because sections have four annotations");

            annotations.Should().Contain(x => x.Name == "InsertRestrictions");

            var insertRestrictions = annotations.First(x => x.Name == "InsertRestrictions");

            insertRestrictions.Namespace.Should().Be("Org.OData.Capabilities.V1");
            insertRestrictions.Value.Should().BeOfType<InsertRestrictionsType>();

            var insertValue = insertRestrictions.Value as InsertRestrictionsType;
            insertValue.Insertable.Should().BeFalse();
            insertValue.NonInsertableNavigationProperties.Should().HaveCount(2);
            insertValue.NonInsertableNavigationProperties.Should().Contain("parentNotebook");
            insertValue.NonInsertableNavigationProperties.Should().Contain("parentSectionGroup");
        }

        [Fact]
        public void Validly_annotated_edm_will_correctly_parse_update_restrictions()
        {
            var annotations = GetAnnotationsFromOneNoteSampleEntitySet("sections");
            annotations.Should().Contain(x => x.Name == "UpdateRestrictions");

            var update = annotations.First(x => x.Name == "UpdateRestrictions");

            update.Namespace.Should().Be("Org.OData.Capabilities.V1");
            update.Value.Should().BeOfType<UpdateRestrictionsType>();

            var updateValue = update.Value as UpdateRestrictionsType;
            updateValue.Updatable.Should().BeFalse();
            updateValue.NonUpdatableNavigationProperties.Should().HaveCount(3);
            updateValue.NonUpdatableNavigationProperties.Should().Contain("pages");
            updateValue.NonUpdatableNavigationProperties.Should().Contain("parentNotebook");
            updateValue.NonUpdatableNavigationProperties.Should().Contain("parentSectionGroup");
        }

        [Fact]
        public void Validly_annotated_edm_will_correctly_parse_delete_restrictions()
        {
            var annotations = GetAnnotationsFromOneNoteSampleEntitySet("sections");
            annotations.Should().Contain(x => x.Name == "DeleteRestrictions");

            var delete = annotations.First(x => x.Name == "DeleteRestrictions");

            delete.Namespace.Should().Be("Org.OData.Capabilities.V1");
            delete.Value.Should().BeOfType<DeleteRestrictionsType>();

            var deleteValue = delete.Value as DeleteRestrictionsType;
            deleteValue.Deletable.Should().BeFalse();
            deleteValue.NonDeletableNavigationProperties.Should().HaveCount(3);
            deleteValue.NonDeletableNavigationProperties.Should().Contain("pages");
            deleteValue.NonDeletableNavigationProperties.Should().Contain("parentNotebook");
            deleteValue.NonDeletableNavigationProperties.Should().Contain("parentSectionGroup");
        }

        [Fact]
        public void Validly_annotated_edm_will_correctly_parse_annotations_with_primitive_values()
        {
            var annotations = GetAnnotationsFromOneNoteSampleEntityContainer();
            annotations.Should().HaveCount(3);
            annotations.Should().Contain(x => x.Name == "BatchSupported");
            annotations.Should().Contain(x => x.Name == "AsynchronousRequestsSupported");
            annotations.Should().Contain(x => x.Name == "BatchContinueOnErrorSupported");

            // In this case, all of the value and namespaces for these annotaion are the same
            // We'll loop over them for brevity. 

            foreach (var annotation in annotations)
            {
                annotation.Namespace.Should().Be("Org.OData.Capabilities.V1");
                annotation.Value.Should().BeOfType<bool>();
                ((bool) annotation.Value).Should().BeFalse();
            }
        }

        private List<OdcmVocabularyAnnotation> GetAnnotationsFromOneNoteSampleEntitySet(string entitySet)
        {
            IEdmModel model = SampleParsedEdmModel;
            IEdmEntitySet sampleEntitySet = model.FindEntityContainer("OneNoteApi").FindEntitySet(entitySet);

            return ODataVocabularyReader.GetOdcmAnnotations(model, sampleEntitySet).ToList();
        }

        private List<OdcmVocabularyAnnotation> GetAnnotationsFromOneNoteSampleEntityContainer()
        {
            IEdmModel model = SampleParsedEdmModel;
            var sampleContainer = model.FindEntityContainer("OneNoteApi");

            return ODataVocabularyReader.GetOdcmAnnotations(model, sampleContainer).ToList();
        }

        #region Helper methods

        private static readonly IEdmModel SampleParsedEdmModel = GetSampleParsedEdmModel();

        private static IEdmModel GetSampleParsedEdmModel()
        {
            IEdmModel edmModel;
            IEnumerable<EdmError> errors;
            if (!EdmxReader.TryParse(XmlReader.Create(new StringReader(ODataReader.v4UnitTests.Properties.Resources.OneNoteExampleEdmx)), out edmModel, out errors))
            {
                throw new InvalidOperationException("Failed to parse Edm model");
            }
            return edmModel;
        }
    }

    #endregion
}