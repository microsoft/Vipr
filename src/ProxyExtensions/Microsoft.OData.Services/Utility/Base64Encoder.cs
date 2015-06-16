using System;
using System.Text;

namespace Microsoft.OData.Services.Utility
{
    public static class Base64Encoder
    {
        public static String Base64Encode(this String plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static String Base64Decode(this String encodedText)
        {
            var plainTextBytes = Convert.FromBase64String(encodedText);
            return Encoding.UTF8.GetString(plainTextBytes, 0, plainTextBytes.Length);
        }
    }
}
