using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.Its.Recipes;
using Microsoft.MockService;
using Vipr;
using Xunit;

namespace ViprCliUnitTests
{
    public class Given_a_MetadataResolver
    {
        private string _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;

        [Fact]
        public void When_the_path_is_a_Uri_it_requests_metadata_from_the_cloud()
        {
            var metadataPath = Any.UriPath(1);
            var metadata = Any.String(1);

            using (var mockService = new MockService()
                .OnRequest(c => c.Request.Path.ToString() == "/" + metadataPath && c.Request.Method == "GET")
                .RespondWith((c, b) => c.Response.Write(metadata)))
            {
                var metadataUri = mockService.GetBaseAddress() + metadataPath;

                MetadataResolver.GetMetadata(metadataUri)
                    .Should().Be(metadata);
            }
        }

        [Fact]
        public void When_the_path_is_local_it_gets_metadata_from_the_filesystem()
        {
            var workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var inputFiles = Any.FileAndContentsDictionary(1);
            var metadataPath = Path.Combine(workingDirectory, inputFiles.First().Key);
            var metadata = inputFiles.First().Value;

            try
            {
                WriteAsTextFiles(inputFiles);

                MetadataResolver.GetMetadata(metadataPath)
                    .Should().Be(metadata);
            }
            finally
            {
                DeleteFiles(inputFiles);
            }
        }

        private void WriteAsTextFiles(IDictionary<string, string> textFiles, string workingDirectory = null)
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

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}
