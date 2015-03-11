namespace Vipr.Core
{
    public class TextFile
    {
        public TextFile(string relativePath, string contents)
        {
            Contents = contents;
            RelativePath = relativePath;
        }

        public string RelativePath { get; private set; }

        public  string Contents { get; private set; }
    }
}
