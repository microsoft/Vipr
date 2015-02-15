using System.Collections.Generic;
using Vipr.Core.CodeModel;

namespace Vipr.Core
{
    public interface IOdcmWriter
    {
        IDictionary<string, string> GenerateProxy(OdcmModel model, IConfigurationProvider configurationProvider = null);
    }
}