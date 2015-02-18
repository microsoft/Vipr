using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;

namespace ViprCliUnitTests
{
    public static class FileSystemHelpers
    {
        public static void WriteAsTextFiles(IDictionary<string, string> textFiles, string workingDirectory = null)
        {
            foreach (var textFile in textFiles)
            {
                var filePath = textFile.Key;

                filePath = GetFilePath(workingDirectory, filePath);
                
                File.WriteAllText(filePath, textFile.Value);
            }
        }

        public static void ValidateTextFiles(IDictionary<string, string> textFiles, string workingDirectory = null)
        {
            foreach (var fileName in textFiles.Keys)
            {
                var filePath = fileName;

                filePath = GetFilePath(workingDirectory, filePath);

                File.ReadAllText(filePath)
                    .Should().Be(textFiles[fileName]);
            }
        }

        public static void DeleteFiles(IDictionary<string, string> files, string workingDirectory = null)
        {
            foreach (var fileName in files.Keys)
            {
                var filePath = fileName;

                filePath = GetFilePath(workingDirectory, filePath);

                if(File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        private static string GetFilePath(string workingDirectory, string filePath)
        {
            if (!String.IsNullOrWhiteSpace(workingDirectory)) filePath = Path.Combine(workingDirectory, filePath);
            return filePath;
        }
    }
}