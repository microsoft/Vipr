using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Vipr.Core.CodeModel;

namespace Vipr.Writer.CSharp
{
    public class ModelField : Field
    {
        public ModelField(string name, OdcmModel odcmModel)
            : base(name, new Type(new Identifier("System", "String")), string.Format("@\"{0}\"", ModelToEscapedString(odcmModel.ServiceMetadata["$metadata"])), true)
        {
            
        }

        private static string ModelToEscapedString(string edmxString)
        {
            var xml = XElement.Parse(edmxString);

            xml.DescendantNodes().OfType<XComment>().Remove();

            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(
                stringBuilder,
                new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    NewLineHandling = NewLineHandling.None,
                    Indent = true,
                }))
            {
                xml.WriteTo(writer);
            }

            return stringBuilder.ToString().Replace("\"", "\"\"");
        }
    }
}