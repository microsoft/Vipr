using System.Collections.Generic;
using System.IO;

namespace Vipr
{
    public class FileWriter
    {
        public static void Write(IDictionary<string, string> fileNameToContents, string outputDirectoryPath = null)
        {
            if (!string.IsNullOrWhiteSpace(outputDirectoryPath) && !Directory.Exists(outputDirectoryPath))
                Directory.CreateDirectory(outputDirectoryPath);

            foreach (var file in fileNameToContents)
            {
                var filePath = file.Key;

                if (!string.IsNullOrWhiteSpace(outputDirectoryPath))
                    filePath = Path.Combine(outputDirectoryPath, filePath);

                File.WriteAllText(filePath, file.Value);
            }
        }
    }
}