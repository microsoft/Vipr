using Vipr.Writer.CSharp.Settings;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Entity_Collection_Bound_VoidMethod_with_OmitMethodNamespaceInUriPaths :
        Given_an_OdcmClass_Entity_Collection_Bound_VoidMethod
    {
        public Given_an_OdcmClass_Entity_Collection_Bound_VoidMethod_with_OmitMethodNamespaceInUriPaths()
            : base()
        {
            ServerMethodNameGenerator = () => Method.Name;

            SetConfiguration(new CSharpWriterSettings {OmitMethodNamespaceInUriPaths = true});
        }
    }
}