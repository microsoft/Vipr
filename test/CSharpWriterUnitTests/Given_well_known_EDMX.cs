// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using CSharpWriterUnitTests.Properties;
using FluentAssertions;
using ODataReader.v4;
using System;
using System.Reflection;
using System.Xml.Linq;
using Vipr.Core;
using Xunit;

namespace CSharpWriterUnitTests
{
    public class Given_well_known_EDMX : CodeGenTestBase
    {
        [Fact]
        public void ActiveDirectory_Edmx_produces_compiling_sourcecode()
        {
            new Action(() => ODataToAssembly<OdcmReader>(Resources.ActiveDirectory_ODataV3))
                .ShouldThrow<InvalidOperationException>("Because AAD Metadata is OData v3");
        }

        [Fact]
        public void CRM_Edmx_produces_compiling_sourcecode()
        {
            new Action(() => ODataToAssembly<OdcmReader>(Resources.CRM_ODataV1))
                .ShouldThrow<InvalidOperationException>("Because CRM Metadata is OData v1");
        }

        [Fact]
        public void Exchange_Edmx_produces_compiling_sourcecode()
        {
            ODataToAssembly<OdcmReader>(Resources.Exchange_ODataV4);
        }

        [Fact]
        public void OneNote_Edmx_produces_compiling_sourcecode()
        {
            ODataToAssembly<OdcmReader>(Resources.OneNote_ODataV4);
        }

        [Fact]
        public void SharePoint_Edmx_produces_compiling_sourcecode()
        {
            ODataToAssembly<OdcmReader>(Resources.SharePoint_ODataV4);
        }

        private Assembly ODataToAssembly<T>(string edmxString) where T : IOdcmReader, new()
        {
            var edmx = XElement.Parse(edmxString);
            var reader = new T();
            var model = reader.GenerateOdcmModel(new List<TextFile> { new TextFile("$metadata", edmxString) });
            return GetProxy(model);
        }
    }
}
