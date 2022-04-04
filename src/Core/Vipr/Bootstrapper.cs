// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using DocoptNet;
using Newtonsoft.Json;
using Vipr.Core;
using Vipr.Core.CodeModel;
using NLog;

namespace Vipr
{
    internal partial class Bootstrapper
    {
        [DocoptArguments(HelpConstName = nameof(Help))]
        private sealed partial class Arguments
        {
            const string Help = @"Vipr CLI Tool
Usage:
    vipr.exe <inputFile> [--reader=<readerName>] [--writer=<writerName>] [--outputPath=<outputPath>] [--modelExport=<modelExportPath>]

Options:
    --reader=<readerName>               Use the model reader defined in assembly readerName. Default is ODataReader.v4.
    --writer=<writerName>               Use the client library writer defined in assembly writerName. Default is CSharpWriter.
    --outputPath=<outputPath>           Use outputPath as the root directory for the generated proxy. Default is the current directory.
    --modelExport=<modelExportPath>     Export the OcdmModel generated from the given Edmx model as a json file.
";
        }

        private IOdcmReader _odcmReader;
        private IOdcmWriter _odcmWriter;
        private string _ocdmModelExportPath = String.Empty;
        private string _readerName = "Vipr.Reader.OData.v4";
        private string _writerName = "Vipr.Writer.CSharp";
        private string _metadataPath = "http://services.odata.org/V4/TripPinServiceRW/$metadata";
        private string _outputPath = @".\output";

        internal Logger Logger => LogManager.GetLogger("Bootstrapper");

        public void Start(string[] args)
        {
            GetCommandLineConfiguration(args);

            var edmxContents = MetadataResolver.GetMetadata(_metadataPath);

            Logger.Info("Generating Client Library to {0}", Path.GetFullPath(_outputPath));

            MetadataToClientSource(edmxContents, _outputPath);

            Logger.Info("Done.");
        }

        private void GetCommandLineConfiguration(string[] args)
        {
            switch (Arguments.CreateParser()
                             .EnableHelp()
                             .Parse(args)
                             .Match(res => (object)res,
                                    res => (Console.Out, res.Help, 0),
                                    res => (Console.Error, res.Usage, 1)))
            {
                case Arguments arguments:
                {
                    GetCommandLineConfiguration(arguments);
                    break;
                }
                case (TextWriter writer, string message, int exitCode):
                {
                    writer.WriteLine(message);
                    Environment.Exit(exitCode);
                    break;
                }
            }
        }

        private void GetCommandLineConfiguration(Arguments res)
        {
            _ocdmModelExportPath = res.OptModelexport ?? _ocdmModelExportPath;

            _readerName = res.OptReader ?? _readerName;

            _writerName = res.OptWriter ?? _writerName;

            if (res.OptOutputpath == null)
            {
                // do nothing, rely on default
            }
            else if (res.OptOutputpath == String.Empty)
            {
                _outputPath = @".\";  // current working directory
            }
            else
            {
                _outputPath = res.OptOutputpath;
            }

            _metadataPath = res.ArgInputfile ?? _metadataPath;
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
            FileWriter.WriteAsync(MetadataToClientSource(edmxString), outputDirectoryPath);
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
