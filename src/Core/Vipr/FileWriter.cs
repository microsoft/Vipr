using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
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

        /**
         * Write the file to disk. If the file is locked for editing,
         * sleep until available
         */
        public static async Task WriteToDisk(string filePath, string output)
        {
            for (int tries = 0; tries < 10; tries++)
            {
                StreamWriter sw = null;
                try
                {
                    using (sw = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        await sw.WriteAsync(output);
                        break;
                    }
                }
                // If the file is currently locked for editing, sleep
                // This shouldn't be hit if the generator is running correctly,
                // however, files are currently being overwritten several times
                catch (IOException)
                {
                    Thread.Sleep(5);
                }
            }

        }
    }
}