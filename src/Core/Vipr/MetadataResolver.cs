using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Vipr
{
    internal class MetadataResolver
    {
        internal static Logger Logger => LogManager.GetLogger("MetadataResolver");

        public static string GetMetadata(string metadataPath)
        {
            string edmxContents = "";

            if (Uri.IsWellFormedUriString(metadataPath, UriKind.Absolute))
            {
                Logger.Info("Downloading metadata from {0}.", metadataPath);

                edmxContents = LoadEdmxFromWeb(metadataPath);
            }
            else
            {
                Logger.Info("Loading metadata from {0}.", metadataPath);

                edmxContents = LoadEdmxFromFile(metadataPath);
            }
            return edmxContents;
        }

        private static string LoadEdmxFromFile(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        private static string LoadEdmxFromWeb(string uri)
        {
            var wc = new WebClient { Encoding = Encoding.UTF8 };

            var result = wc.DownloadString(new Uri(uri));

            return result;
        }
    }
}