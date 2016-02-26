// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using DocoptNet;
using Newtonsoft.Json;
using Vipr.Core;
using Vipr.Core.CodeModel;

namespace Vipr
{
    internal class Bootstrapper
    {
        const string Usage = @"Vipr CLI Tool
Usage:
    vipr.exe <inputFile> [--reader=<readerName>] [--writer=<writerName>] [--outputPath=<outputPath>] [--modelExport=<modelExportPath>]

Options:
    --reader=<readerName>               Use the model reader defined in assembly readerName. Default is ODataReader.v4.
    --writer=<writerName>               Use the client library writer defined in assembly writerName. Default is CSharpWriter.
    --outputPath=<outputPath>           Use outputPath as the root directory for the generated proxy. Default is the current directory.
    --modelExport=<modelExportPath>     Export the OcdmModel generated from the given Edmx model as a json file.
";

        private IOdcmReader _odcmReader;
        private IOdcmWriter _odcmWriter;
        private string _ocdmModelExportPath = String.Empty;
        private string _readerName = "Vipr.Reader.OData.v4";
        private string _writerName = "Vipr.Writer.CSharp";
        private string _metadataPath = "http://services.odata.org/V4/TripPinServiceRW/$metadata";
        private string _outputPath = @".\output";

        public void Start(string[] args)
        {
            Console.WriteLine();

            GetCommandLineConfiguration(args);

            var edmxContents = MetadataResolver.GetMetadata(_metadataPath);

            Console.WriteLine("Generating Client Library to {0}", Path.GetFullPath(_outputPath));

            MetadataToClientSource(edmxContents, _outputPath);

            Console.WriteLine("Done.");
        }

        private void GetCommandLineConfiguration(string[] args)
        {
            var docopt = new Docopt();

            IDictionary<string, ValueObject> res = docopt.Apply(Usage, args, help: true, exit: true);

            _ocdmModelExportPath = res["--modelExport"] == null ? _ocdmModelExportPath : res["--modelExport"].ToString();

            _readerName = res["--reader"] == null ? _readerName : res["--reader"].ToString();

            _writerName = res["--writer"] == null ? _writerName : res["--writer"].ToString();

            if (res["--outputPath"] == null)
            {
                // do nothing, rely on default
            }
            else if (res["--outputPath"].ToString() == String.Empty)
            {
                _outputPath = @".\";  // current working directory
            }
            else
            {
                _outputPath = res["--outputPath"].ToString();
            }

            _metadataPath = res["<inputFile>"] == null ? _metadataPath : res["<inputFile>"].ToString();
        }

        public IOdcmReader OdcmReader
        {
            get
            {
                if (_odcmReader != null) return _odcmReader;

                _odcmReader = GetOdcmReader();

                ConfigurationProvider.SetConfigurationOn(_odcmReader);

                return _odcmReader;
            }
        }

        public IOdcmWriter OdcmWriter
        {
            get
            {
                if (_odcmWriter != null) return _odcmWriter;

                _odcmWriter = GetOdcmWriter();

                ConfigurationProvider.SetConfigurationOn(_odcmWriter);

                return _odcmWriter;
            }
        }

        protected virtual IOdcmReader GetOdcmReader()
        {
            return TypeResolver.GetInstance<IOdcmReader>(_readerName);
        }

        protected virtual IOdcmWriter GetOdcmWriter()
        {
            return TypeResolver.GetInstance<IOdcmWriter>(_writerName);
        }

        private void MetadataToClientSource(string edmxString, string outputDirectoryPath)
        {
            FileWriter.Write(MetadataToClientSource(edmxString), outputDirectoryPath);
        }

        private IEnumerable<TextFile> MetadataToClientSource(string edmxString)
        {
            var model = OdcmReader.GenerateOdcmModel(new List<TextFile> { new TextFile("$metadata", edmxString) });

            ExportOcdmModel(model);

            return OdcmWriter.GenerateProxy(model);
        }

        private void ExportOcdmModel(OdcmModel model)
        {
            if (string.IsNullOrEmpty(_ocdmModelExportPath)) return;

            var jss = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects };

            File.WriteAllText(_ocdmModelExportPath, JsonConvert.SerializeObject(model, jss));
        }
    }
}
