using System;

namespace Microsoft.OData.Services.OData
{
    public class ODataException:Exception
    {
        public ODataException()
            : base()
        {
        }

        public ODataException(string message)
            : base(message)
        {
        }

        public ODataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
