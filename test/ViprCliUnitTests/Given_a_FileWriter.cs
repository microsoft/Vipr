using System.IO;
using Microsoft.Its.Recipes;
using Vipr;
using Vipr.Core;
using Xunit;

namespace ViprCliUnitTests
{
    public class Given_a_FileWriter
    {
        [Fact]
        public void When_no_files_are_specified_it_returns()
        {
            FileWriter.Write(new TextFileCollection());
        }

        [Fact]
        public void When_outputDirectoryPath_is_not_specified_then_it_writes_the_files_to_the_working_directory()
        {
            var files = Any.TextFileCollection();

            try
            {
                FileWriter.Write(files);

                FileSystemHelpers.ValidateTextFiles(files);
            }
            finally
            {
                FileSystemHelpers.DeleteFiles(files);
            }
        }

        [Fact]
        public void When_outputDirectoryPath_is_specified_and_exitst_then_it_writes_the_files_to_the_outputDirectoryPath()
        {
            var files = Any.TextFileCollection();

            var outputDirectoryPath = Any.Word();

            Directory.CreateDirectory(outputDirectoryPath);

            try
            {
                FileWriter.Write(files, outputDirectoryPath);

                FileSystemHelpers.ValidateTextFiles(files, outputDirectoryPath);
            }
            finally
            {
                FileSystemHelpers.DeleteFiles(files, outputDirectoryPath);

                Directory.Delete(outputDirectoryPath);
            }
        }

        [Fact]
        public void When_outputDirectoryPath_is_specified_and_does_not_exitst_then_it_creates_the_directory_and_writes_the_files()
        {
            var files = Any.TextFileCollection();

            var outputDirectoryPath = Any.Word();

            try
            {
                FileWriter.Write(files, outputDirectoryPath);

                FileSystemHelpers.ValidateTextFiles(files, outputDirectoryPath);
            }
            finally
            {

                FileSystemHelpers.DeleteFiles(files, outputDirectoryPath);

                Directory.Delete(outputDirectoryPath);
            }
        }
    }
}
