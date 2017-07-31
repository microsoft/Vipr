using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core;

namespace Vipr
{
    internal static class FileWriter
    {
        public static async void WriteAsync(IEnumerable<TextFile> textFilesToWrite, string outputDirectoryPath = null)
        {
            if (!string.IsNullOrWhiteSpace(outputDirectoryPath) && !Directory.Exists(outputDirectoryPath))
                Directory.CreateDirectory(outputDirectoryPath);

            var fileTasks = new List<Task>();

            foreach (var file in textFilesToWrite)
            {
                var filePath = file.RelativePath;

                if (!string.IsNullOrWhiteSpace(outputDirectoryPath))
                    filePath = Path.Combine(outputDirectoryPath, filePath);

                if (!String.IsNullOrWhiteSpace(Environment.CurrentDirectory) &&
                    !Path.IsPathRooted(filePath))
                    filePath = Path.Combine(Environment.CurrentDirectory, filePath);

                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                fileTasks.Add(WriteToDisk(filePath, file.Contents));
            }
            await Task.WhenAll(fileTasks);
        }

        public static async Task WriteToDisk(string filePath, string output)
        {
            StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
            await sw.WriteAsync(output);
            sw.Close();
        }
    }
}