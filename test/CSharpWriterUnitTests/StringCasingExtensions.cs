namespace CSharpWriterUnitTests
{
    public static class StringCasingExtensions
    {
        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }
    }
}