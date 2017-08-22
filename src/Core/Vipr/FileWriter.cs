using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vipr.Core;
using NLog;

namespace Vipr
{
    internal static class FileWriter
    {
        private static ConcurrentDictionary<string, Lazy<AsyncLock>> lockDictionary = new ConcurrentDictionary<string, Lazy<AsyncLock>>();
        internal static Logger Logger => LogManager.GetLogger("FileWriter");

        /// <summary>
        /// Write all generated files to disk
        /// </summary>
        /// <param name="textFilesToWrite">A list of files to write</param>
        /// <param name="outputDirectoryPath">The directory to store the output to</param>
        public static void WriteAsync(IEnumerable<TextFile> textFilesToWrite, string outputDirectoryPath = null)
        {
            if (!string.IsNullOrWhiteSpace(outputDirectoryPath) && !Directory.Exists(outputDirectoryPath))
                Directory.CreateDirectory(outputDirectoryPath);

            /* Write asynchronously in batches of 50
            * This prevents I/O from slowing down
            * from an overload of requests
            */
            //TODO: Allow this value to be set in the program configuration
            var batchSize = 50;
            var batchCount = 0;
            List<Task> tasks = new List<Task>();

            foreach (var file in textFilesToWrite)
            {
                tasks = new List<Task>();
                var filePath = file.RelativePath;

                if (!string.IsNullOrWhiteSpace(outputDirectoryPath))
                    filePath = Path.Combine(outputDirectoryPath, filePath);

                if (!String.IsNullOrWhiteSpace(Environment.CurrentDirectory) &&
                    !Path.IsPathRooted(filePath))
                    filePath = Path.Combine(Environment.CurrentDirectory, filePath);

                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }
                    catch (IOException e)
                    {
                        Logger.Error("Failed to create directory for file", e);
                    }
                }

                // Create a new async task to write the file
                tasks.Add(WriteToDisk(filePath, file.Contents));
                batchCount++;

                // Reached the batch limit. Wait for all tasks to complete
                if (batchCount > batchSize)
                {
                    Task t = Task.WhenAll(tasks);
                    t.Wait();
                    batchCount = 0;
                }
            }

            // Final task wait
            if (tasks.Count > 0)
            {
                Task t = Task.WhenAll(tasks);
                t.Wait();
            }
        }

        /// <summary>
        /// Asynchronous method to write a file to disk
        /// </summary>
        /// <param name="filePath">The file path and name to write to</param>
        /// <param name="output">The content to write to the file</param>
        /// <returns>A write to disk task</returns>
        public static async Task WriteToDisk(string filePath, string output)
        {
            var lockItem = lockDictionary.GetOrAdd(filePath, new Lazy<AsyncLock>());
            using (await lockItem.Value.LockAsync())
            {
                using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    await sw.WriteAsync(output);
                }
            }
        }
    }
}