// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//Removing for now. Fields do not belong in odcm

//using FluentAssertions;
//using Microsoft.Its.Recipes;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using Vipr.Core.CodeModel;

//namespace CSharpWriterUnitTests
//{
//    /// <summary>
//    /// Summary description for Given_an_OdcmModel
//    /// </summary>
//    [TestClass]
//    public class Given_OdcmFields : CodeGenTestBase
//    {
//        private OdcmModel _model;
//        private OdcmNamespace _namespace;
//        private OdcmClass _class;
//        private BindingFlags _allFlags = BindingFlags.Public
//            | BindingFlags.NonPublic
//            | BindingFlags.Static
//            | BindingFlags.Instance
//            | BindingFlags.DeclaredOnly;

//        [TestInitialize]
//        public void Init()
//        {
//            _model = new OdcmModel();

//            _namespace = Any.OdcmNamespace();

//            _model.Namespaces.Add(_namespace);

//            _class = Any.OdcmClass(e => e.Namespace = _namespace.Name);
//        }

//        [TestMethod]
//        public void When_generated_it_is_private()
//        {
//            var field = Any.OdcmField();

//            field.TypeIdentifier = _class;

//            _class.Fields.Add(field);

//            _model.AddType(_class);

//            var proxy = GetProxy(_model);

//            var classType = proxy.GetClass(_class.Namespace, _class.Name);

//            classType.GetField(field.Name, _allFlags).IsPrivate.Should().BeTrue();
//        }

//        [TestMethod]
//        public void When_the_model_specifies_a_structured_type_then_the_proxy_implements_the_specified_type()
//        {
//            var field = Any.OdcmField();

//            field.TypeIdentifier = _class;

//            _class.Fields.Add(field);

//            _model.AddType(_class);

//            var proxy = GetProxy(_model);

//            var classType = proxy.GetClass(_class.Namespace, _class.Name);

//            classType.GetField(field.Name, _allFlags).FieldType.Should().Be(classType);
//        }

//        [TestMethod]
//        public void When_the_model_specifies_a_Clr_primitive_type_then_the_proxy_implements_the_Clr_primitive_equivalent
//            ()
//        {
//            var edmToClrTypeMap = new Dictionary<string, TypeIdentifier>
//            {
//                {"Binary", typeof (byte[])},
//                {"Boolean", typeof (bool)},
//                {"Byte", typeof (byte)},
//                {"Date", typeof (DateTimeOffset)},
//                {"DateTimeOffset", typeof (DateTimeOffset)},
//                {"Decimal", typeof (decimal)},
//                {"Double", typeof (double)},
//                {"Duration", typeof (TimeSpan)},
//                {"Guid", typeof (Guid)},
//                {"Int16", typeof (short)},
//                {"Int32", typeof (int)},
//                {"Int64", typeof (long)},
//                {"SByte", typeof (sbyte)},
//                {"Single", typeof (float)},
//                {"Stream", typeof (Stream)},
//                {"String", typeof (string)},
//                {"TimeOfDay", typeof (DateTimeOffset)},
//            };

//            foreach (var edmTypeName in edmToClrTypeMap.Keys)
//            {
//                var field = new OdcmField("field_" + edmTypeName) {TypeIdentifier = new OdcmPrimitiveType(edmTypeName, "Edm")};

//                _class.Fields.Add(field);
//            }

//            _model.AddType(_class);

//            var proxy = GetProxy(_model);

//            var classType = proxy.GetClass(_class.Namespace, _class.Name);

//            foreach (var edmTypeName in edmToClrTypeMap.Keys)
//            {
//                classType.GetField("field_" + edmTypeName, _allFlags).FieldType
//                    .Should().Be(edmToClrTypeMap[edmTypeName]);
//            }
//        }
//    }
//}
