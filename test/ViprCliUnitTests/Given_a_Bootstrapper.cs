// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Moq;
using Moq.Protected;
using Vipr;
using Vipr.Core;
using Vipr.Core.CodeModel;
using Xunit;
using TestConstants;

namespace ViprCliUnitTests
{
    public class Given_a_Bootstrapper
    {
        private readonly Mock<Bootstrapper> _bootstrapperMock;
        private readonly Mock<IOdcmReader> _readerMock;
        private readonly Mock<IOdcmWriter> _writerMock;
        private readonly string _workingDirectory;


        public Given_a_Bootstrapper()
        {
            _bootstrapperMock = new Mock<Bootstrapper>(MockBehavior.Strict) { CallBase = true };
            _readerMock = new Mock<IOdcmReader>(MockBehavior.Strict);
            _writerMock = new Mock<IOdcmWriter>(MockBehavior.Strict);
            _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
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

            try
            {
                ValidateFromDiskGeneration(string.Format(" --modelExport {0}", exportPath));

                File.Exists(exportPath).Should().BeTrue("Because the model was to be exported.");
            }
            finally
            {
                if(File.Exists(exportPath))
                    File.Delete(exportPath);
            }
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

        [Fact]
        public void When_a_custom_reader_is_specified_then_it_is_used()
        {
            var metadata = ODataV4.EmptyEdmx;

            WithWebMetadata(metadata, metadataUri =>
            {
                var commandLine = String.Format("{0} --reader=TestReaderWriter", metadataUri);

                var bootstrapper = new Bootstrapper();

                bootstrapper.Start(commandLine.Split(' '));

                bootstrapper.OdcmReader.GetType().Should().Be(typeof (TestReaderWriter.TestReaderWriter));

                bootstrapper.OdcmWriter.GetType().Should().Be(typeof(Vipr.Writer.CSharp.CSharpWriter));

                if(Directory.Exists(FileSystem.DEFAULT_OUTPUT_DIRECTORY)) Directory.Delete(FileSystem.DEFAULT_OUTPUT_DIRECTORY, true);
            });
        }


        [Fact]
        public void When_a_custom_writer_is_specified_then_it_is_used()
        {
            var metadata = ODataV4.EmptyEdmx;

            WithWebMetadata(metadata, metadataUri =>
            {
                var commandLine = String.Format("{0} --writer=TestReaderWriter", metadataUri);

                var bootstrapper = new Bootstrapper();

                bootstrapper.Start(commandLine.Split(' '));

                bootstrapper.OdcmReader.GetType().Should().Be(typeof (Vipr.Reader.OData.v4.OdcmReader));

                bootstrapper.OdcmWriter.GetType().Should().Be(typeof (TestReaderWriter.TestReaderWriter));
            });
        }

        [Fact]
        public void When_custom_reader_and_writer_are_not_specified_then_defaults_are_used()
        {
            var metadata = ODataV4.EmptyEdmx;

            WithWebMetadata(metadata, metadataUri =>
            {
                var commandLine = String.Format("{0}", metadataUri);

                var bootstrapper = new Bootstrapper();

                bootstrapper.Start(commandLine.Split(' '));

                if (Directory.Exists(FileSystem.DEFAULT_OUTPUT_DIRECTORY)) Directory.Delete(FileSystem.DEFAULT_OUTPUT_DIRECTORY, true);

                bootstrapper.OdcmReader.GetType().Should().Be(typeof (Vipr.Reader.OData.v4.OdcmReader));

                bootstrapper.OdcmWriter.GetType().Should().Be(typeof (Vipr.Writer.CSharp.CSharpWriter));
            });
        }

        [Fact]
        public void When_custom_outputPath_is_not_specified_then_defaults_are_used()
        {
            var metadata = ODataV4.EmptyEdmx;

            WithWebMetadata(metadata, metadataUri =>
            {
                var commandLine = String.Format("{0}", metadataUri);

                var bootstrapper = new Bootstrapper();

                bootstrapper.Start(commandLine.Split(' '));

                var pathToOneExpectedOutputFile = Path.Combine(FileSystem.DEFAULT_OUTPUT_DIRECTORY, FileSystem.CSHARP_WRITER_OUTPUT);

                File.Exists(pathToOneExpectedOutputFile).Should().BeTrue("Because one expected output file was created in default output directory.");

                if (Directory.Exists(FileSystem.DEFAULT_OUTPUT_DIRECTORY)) Directory.Delete(FileSystem.DEFAULT_OUTPUT_DIRECTORY, true);
            });
        }

        [Fact]
        public void When_custom_outputPath_is_specified_then_it_is_used()
        {
            var metadata = ODataV4.EmptyEdmx;

            WithWebMetadata(metadata, metadataUri =>
            {
                var outputPath = Any.Word();

                var commandLine = String.Format("{0} --outputPath={1}", metadataUri, outputPath);

                var bootstrapper = new Bootstrapper();

                bootstrapper.Start(commandLine.Split(' '));

                var filePath = Path.Combine(outputPath, FileSystem.CSHARP_WRITER_OUTPUT);

                File.Exists(filePath).Should().BeTrue("Because one expected output file was found in the specified output directory.");

                if (Directory.Exists(outputPath)) Directory.Delete(outputPath, true);
            });
        }
        
        private void ValidateFromDiskGeneration(string optionalCommandLine = "")
        {
            var metadata = Any.String(1);

            WithDiskMetadata(metadata, metadataPath =>
            {
                var commandLine = String.Format("{0} --outputPath={1}{2}", metadataPath, _workingDirectory,
                    optionalCommandLine);

                ValidateProxyGeneration(metadata, commandLine);
            });
        }

        private void ValidateFromWebGeneration(string optionalCommandLine = "")
        {
            var metadata = Any.String(1);

            WithWebMetadata(metadata, metadataUri =>
            {
                var commandLine = String.Format("{0} --outputPath={1}{2}", metadataUri, _workingDirectory,
                    optionalCommandLine);

                ValidateProxyGeneration(metadata, commandLine);
            });
        }
        
        private void ValidateProxyGeneration(string metadata, string commandLine)
        {
            var outputFiles = Any.IEnumerable<TextFile>().ToList();

            try
            {
                var model = Any.OdcmModel();

                ConfigureMocks(metadata, model, outputFiles);

                _bootstrapperMock.Object.Start(commandLine.Split(' '));

                FileSystemHelpers.ValidateTextFiles(outputFiles);
            }
            finally
            {
                FileSystemHelpers.DeleteFiles(outputFiles);
            }
        }

        private void ConfigureMocks(string metadata, OdcmModel model, IEnumerable<TextFile> outputTextFiles)
        {
            ConfigureReaderMock(metadata, model);

            ConfigureWriterMock(model, outputTextFiles);

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

        private void ConfigureWriterMock(OdcmModel model, IEnumerable<TextFile> outputTextFiles)
        {
            _writerMock
                .Setup(w => w.GenerateProxy(It.Is<OdcmModel>(m => m == model)))
                .Returns(outputTextFiles);
        }

        private void ConfigureReaderMock(string metadata, OdcmModel model)
        {
            _readerMock
                .Setup(
                    r =>
                        r.GenerateOdcmModel(
                            It.Is<IEnumerable<TextFile>>(
                                g => g.Any(f => f.RelativePath.Equals("$metadata") && f.Contents.Equals(metadata)))))
                .Returns(model);
        }

        private void WithWebMetadata(string metadata, Action<string> action)
        {
            var metadataPath = Any.UriPath(1);

            using (var mockService = new MockService()
                .OnRequest(c => c.Request.Path.ToString() == "/" + metadataPath && c.Request.Method == "GET")
                .RespondWith(c => c.Response.Write(metadata)))
            {
                action(mockService.GetBaseAddress() + metadataPath);
            }
        }

        private void WithDiskMetadata(string metadata, Action<string> action)
        {

            var fileName = Any.Word();
            var metadataPath = Path.Combine(_workingDirectory, fileName);
            var fileGroup = new List<TextFile> {new TextFile(fileName, metadata)};

            try
            {
                FileSystemHelpers.WriteFiles(fileGroup);

                action(metadataPath);
            }
            finally
            {
                FileSystemHelpers.DeleteFiles(fileGroup);
            }
        }
    }
}
