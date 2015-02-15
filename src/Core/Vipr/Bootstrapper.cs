// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using DocoptNet;
using Newtonsoft.Json;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace Vipr
{
    public class Bootstrapper
    {
        const string Usage = @"Vipr CLI Tool
Usage:
    vipr.exe compile example to <sourcePath>
    vipr.exe compile fromdisk <metadataPath> to <sourcePath> [--modelexport=<modelExportPath>]
    vipr.exe compile fromweb <metadataUri> to <sourcePath> [--modelexport=<modelExportPath>]
Options:
    --modelexport=<modelExportPath>     Export the OcdmModel generated from the given Edmx model as a json file.
";

        private string _ocdmModelExportPath = "";
        private IOdcmReader _odcmReader;
        private IOdcmWriter _odcmWriter;

        public void Start(string[] args)
        {
            var docopt = new Docopt();

            IDictionary<string, ValueObject> res = new Dictionary<string, ValueObject>();

            res = docopt.Apply(Usage, args, help: true, exit: true);

            _ocdmModelExportPath = res["--modelexport"] == null ? String.Empty : res["--modelexport"].ToString();

            string outputPath = res["<sourcePath>"].ToString();

            string edmxContents = "";

            var fromFile = res["fromdisk"].IsTrue;
            var fromWeb = res["fromweb"].IsTrue;
            var fromExample = !fromWeb && !fromFile;

            if (fromWeb)
            {
                var uri = res["<metadataUri>"].ToString();

                Console.WriteLine("Downloading service description from {0}.", uri);

                edmxContents = LoadEdmxFromWeb(uri);
            }

            if (fromFile)
            {
                var path = res["<metadataPath>"].ToString();

                Console.WriteLine("Loading service description from {0}.", path);

                edmxContents = LoadEdmxFromFile(path);
            }

            if (fromExample)
            {
                Console.WriteLine("Downloading sample service description.");

                edmxContents = LoadEdmxFromWeb("http://services.odata.org/V4/TripPinServiceRW/$metadata");
            }

            Console.WriteLine("Generating Client Library to {0}", outputPath);

            ODataToFile(outputPath, edmxContents);

            Console.WriteLine("Done.");
        }

        private IOdcmReader OdcmReader
        {
            get
            {
                if (_odcmReader == null)
                {
                    _odcmReader = GetOdcmReader();

                    ConfigurationProvider.SetConfigurationOn(_odcmReader);
                }

                return _odcmReader;
            }
        }

        private IOdcmWriter OdcmWriter
        {
            get
            {
                if (_odcmWriter == null)
                {
                    _odcmWriter = GetOdcmWriter();

                    ConfigurationProvider.SetConfigurationOn(_odcmWriter);
                }

                return _odcmWriter;
            }
        }

        protected virtual IOdcmReader GetOdcmReader()
        {
            return new ODataReader.v4.OdcmReader();
        }

        protected virtual IOdcmWriter GetOdcmWriter()
        {
            return new CSharpWriter.CSharpWriter();
        }

        private void ODataToFile(string outputDirectoryPath, string edmxString)
        {
            foreach (var file in EdmxToClientSource(edmxString))
            {
                var filePath = file.Key;

                if (!string.IsNullOrWhiteSpace(outputDirectoryPath))
                    filePath = Path.Combine(outputDirectoryPath, filePath);

                File.WriteAllText(filePath, file.Value);
            }
        }

        private IDictionary<string, string> EdmxToClientSource(string edmxString)
        {
            var model = OdcmReader.GenerateOdcmModel(new Dictionary<string, string> { { "$metadata", edmxString } });

            ExportOcdmModel(model);

            return OdcmWriter.GenerateProxy(model);
        }

        private void ExportOcdmModel(OdcmModel model)
        {
            if (string.IsNullOrEmpty(_ocdmModelExportPath)) return;

            var jss = new JsonSerializerSettings {PreserveReferencesHandling = PreserveReferencesHandling.Objects};

            File.WriteAllText(_ocdmModelExportPath, JsonConvert.SerializeObject(model, jss));
        }

        private static string LoadEdmxFromFile(string filepath)
        {
            return File.ReadAllText(filepath);
        }

        private static string LoadEdmxFromWeb(string uri)
        {
            var wc = new WebClient {Encoding = Encoding.UTF8};

            var result = wc.DownloadString(new Uri(uri));

            return result;
        }
    }
}