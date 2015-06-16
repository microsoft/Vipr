using System.Text;

namespace Microsoft.OData.Services.OData
{
    public class MultipartElement
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public MultipartElement(string name, string content)
        {
            Name = name;
            ContentType = "text/html";
            Content = Encoding.UTF8.GetBytes(content);
        }

        public MultipartElement(string name, string contentType, byte[] content)
        {
            Name = name;
            ContentType = contentType;
            Content = content;
        }
    }
}
