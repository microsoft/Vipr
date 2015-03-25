namespace Vipr.Core
{
    public class TextFile : OutputFile
    {
        public TextFile(string relativePath, string contents)
        {
            Contents = contents;
            RelativePath = relativePath;
        }

        public TextFile()
        {
        }

        public string Contents { get; private set; }
    }
}
