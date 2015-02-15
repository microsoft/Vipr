using Vipr.Core.CodeModel;

namespace CSharpWriter
{
    public class EnumMember
    {
        public string Name { get; set; }
        public long? Value { get; set; }
        
        public EnumMember(OdcmEnumMember odcmEnumMember)
        {
            Name = odcmEnumMember.Name;
            Value = odcmEnumMember.Value;
        }
    }
}