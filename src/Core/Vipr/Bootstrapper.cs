using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using CSharpWriter;
using DocoptNet;
using Microsoft.Data.OData;
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
    vipr.exe compile fromedmx <edmxPath> to <sourcePath> [--modelexport=<modelExportPath>]
    vipr.exe compile fromweb <webPath> to <sourcePath> [--modelexport=<modelExportPath>]
Options:
    --modelexport=<modelExportPath>     Export the OcdmModel generated from the given Edmx model as a json file.
";

        private string _ocdmModelExportPath = "";

        public void Start(string[] args)
        {
            var docopt = new Docopt();

            IDictionary<string, ValueObject> res = new Dictionary<string, ValueObject>();

            res = docopt.Apply(Usage, args, help: true, exit: true);

            _ocdmModelExportPath = res["--modelexport"] == null ? String.Empty : res["--modelexport"].ToString();

            string outputPath = res["<sourcePath>"].ToString();

            string edmxContents = "";

            var fromWeb = res["fromweb"].IsTrue;
            var fromFile = res["fromedmx"].IsTrue;
            var fromExample = !fromWeb && !fromFile;

            if (fromWeb)
            {
                edmxContents = LoadEdmxFromWeb(res["<webPath>"].ToString());
            }

            if (fromFile)
            {
                edmxContents = LoadEdmxFromFile(res["<edmxPath>"].ToString());
            }

            if (fromExample)
            {
                edmxContents = LoadEdmxFromWeb("http://services.odata.org/V4/TripPinServiceRW/$metadata");
            }

            ODataToFile(outputPath, edmxContents);
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
            var reader = GetOdcmReader();
            var model = reader.GenerateOdcmModel(new Dictionary<string, string>{{"$metadata",edmxString}});

            ExportOcdmModel(model);

            var writer = GetOdcmWriter();
            return writer.GenerateProxy(model);
        }

        private void ExportOcdmModel(OdcmModel model)
        {
            if (string.IsNullOrEmpty(_ocdmModelExportPath)) return;

            var jss = new JsonSerializerSettings();
            jss.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

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