namespace ODataReader.v4UnitTests
{
    public class OData
    {
        public static string Core(string name)
        {
            return "Org.OData.Core.V1." + name;
        }

        public static string Capabilities(string name)
        {
            return Capabilities() + "." + name;
        }

        public static string Capabilities()
        {
            return "Org.OData.Capabilities.V1";
        }
    }
}
