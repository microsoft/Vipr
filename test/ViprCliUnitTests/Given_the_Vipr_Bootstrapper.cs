// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Moq;
using Moq.Protected;
using Vipr;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;

namespace ViprCliUnitTests
{
    public class Given_the_Vipr_Bootstrapper
    {
        private readonly Mock<Bootstrapper> _bootstrapperMock;
        private readonly Mock<IOdcmReader> _readerMock;
        private readonly Mock<IOdcmWriter> _writerMock;
        private readonly string _workingDirectory;

        public Given_the_Vipr_Bootstrapper()
        {
            _bootstrapperMock = new Mock<Bootstrapper>(MockBehavior.Strict) { CallBase = true };
            _readerMock = new Mock<IOdcmReader>(MockBehavior.Strict);
            _writerMock = new Mock<IOdcmWriter>(MockBehavior.Strict);
            _workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        [Fact]
        public void When_edmx_is_on_filesystem_it_is_given_to_the_reader_and_the_writers_output_is_written_to_disk()
        {
            ValidateFromDiskGeneration();
        }

        [Fact]
        public void When_edmx_is_in_cloud_it_is_given_to_the_reader_and_the_writers_output_is_written_to_disk()
        {
            ValidateFromWebGeneration();
        }

        [Fact]
        public void When_modelexport_is_set_it_exports_the_model()
        {
            var exportPath = Path.Combine(_workingDirectory, Any.Word());

            ValidateFromDiskGeneration(string.Format(" --modelexport {0}", exportPath));

            File.Exists(exportPath).Should().BeTrue("Because the model was to be exported.");

            File.Delete(exportPath);
        }

        [Fact]
        public void When_a_reader_implements_IConfigurable_then_SetConfigurationProvider_is_called()
        {
            var configurableReaderMock = _readerMock.As<IConfigurable>();
            configurableReaderMock.Setup(c => c.SetConfigurationProvider(It.IsAny<IConfigurationProvider>()));

            ValidateFromWebGeneration();

            configurableReaderMock.Verify(c => c.SetConfigurationProvider(It.IsAny<IConfigurationProvider>()), Times.Once);
        }

        [Fact]
        public void When_a_writer_implements_IConfigurable_then_SetConfigurationProvider_is_called()
        {
            var configurableWriterMock = _writerMock.As<IConfigurable>();
            configurableWriterMock.Setup(c => c.SetConfigurationProvider(It.IsAny<IConfigurationProvider>()));

            ValidateFromWebGeneration();

            configurableWriterMock.Verify(c => c.SetConfigurationProvider(It.IsAny<IConfigurationProvider>()), Times.Once);
        }

        private void ValidateFromDiskGeneration(string optionalCommandLine = "")
        {
            var inputFiles = Any.FileAndContentsDictionary(1);
            var metadataPath = Path.Combine(_workingDirectory, inputFiles.First().Key);
            var metadata = inputFiles.First().Value;

            try
            {
                WriteAsTextFiles(inputFiles);

                var commandLine = String.Format("compile fromdisk {0} to {1}{2}", metadataPath, _workingDirectory,
                    optionalCommandLine);

                ValidateProxyGeneration(metadata, commandLine);
            }
            finally
            {
                DeleteFiles(inputFiles);
            }
        }

        private void ValidateFromWebGeneration()
        {
            var metadataPath = Any.UriPath(1);
            var metadata = Any.String(1);

            using (var mockService = new MockService()
                .Setup(r => r.Request.Path.ToString() == "/" + metadataPath &&
                            r.Request.Method == "GET",
                    r => r.Response.Write(metadata))
                .Start())
            {
                var metadataUri = mockService.GetBaseAddress() + metadataPath;
                var commandLine = String.Format("compile fromweb {0} to {1}", metadataUri, _workingDirectory);

                ValidateProxyGeneration(metadata, commandLine);
            }
        }
        
        private void ValidateProxyGeneration(string metadata, string commandLine)
        {
            var outputFiles = Any.FileAndContentsDictionary();

            try
            {
                var model = Any.OdcmModel();

                ConfigureMocks(metadata, model, outputFiles);

                _bootstrapperMock.Object.Start(commandLine.Split(' '));

                ValidateTextFiles(outputFiles);
            }
            finally
            {
                DeleteFiles(outputFiles);
            }
        }

        private void WriteAsTextFiles(IDictionary<string, string> textFiles)
        {
            foreach (var textFile in textFiles)
            {
                var filePath = Path.Combine(_workingDirectory, textFile.Key);
                File.WriteAllText(filePath, textFile.Value);
            }
        }

        private void ValidateTextFiles(IDictionary<string, string> textFiles)
        {
            foreach (var fileName in textFiles.Keys)
            {
                var filePath = Path.Combine(_workingDirectory, fileName);
                File.ReadAllText(filePath)
                    .Should().Be(textFiles[fileName]);
            }
        }

        private void DeleteFiles(IDictionary<string, string> files)
        {
            foreach (var fileName in files.Keys)
            {
                var filePath = Path.Combine(_workingDirectory, fileName);

                if(File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        private void ConfigureMocks(string metadata, OdcmModel model, IDictionary<string, string> outputFiles)
        {
            ConfigureReaderMock(metadata, model);

            ConfigureWriterMock(model, outputFiles);

            ConfigureBootstrapperMock();
        }

        private void ConfigureBootstrapperMock()
        {
            _bootstrapperMock.Protected()
                .Setup<IOdcmReader>("GetOdcmReader")
                .Returns(_readerMock.Object);

            _bootstrapperMock.Protected()
                .Setup<IOdcmWriter>("GetOdcmWriter")
                .Returns(_writerMock.Object);
        }

        private void ConfigureWriterMock(OdcmModel model, IDictionary<string, string> outputFiles)
        {
            _writerMock
                .Setup(w => w.GenerateProxy(It.Is<OdcmModel>(m => m == model), null))
                .Returns(outputFiles);
        }

        private void ConfigureReaderMock(string metadata, OdcmModel model)
        {
            _readerMock
                .Setup(
                    r =>
                        r.GenerateOdcmModel(
                            It.Is<Dictionary<string, string>>(
                                d => d.ContainsKey("$metadata") && d["$metadata"].Equals(metadata))))
                .Returns(model);
        }
    }
}
