using Vipr.Writer.CSharp.Settings;

namespace CSharpWriterUnitTests
{
    public class Given_an_OdcmClass_Service_Bound_Function_Instance_with_OmitMethodNamespaceInUriPaths :
        Given_an_OdcmClass_Service_Bound_Function_Instance
    {
        public Given_an_OdcmClass_Service_Bound_Function_Instance_with_OmitMethodNamespaceInUriPaths()
            : base()
        {
            ServerMethodNameGenerator = () => Method.Name;

            SetConfiguration(new CSharpWriterSettings { OmitMethodNamespaceInUriPaths = true });

            base.Init();
        }
    }
}