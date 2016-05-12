using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core.CodeModel.Vocabularies.Capabilities
{
    public interface INameMapper
    {
        string ToInternal(string name);
        string ToExternal(string name);
    }
}
