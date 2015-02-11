using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core {
    public interface IReaderConfigurationProvider : IConfigurationProvider {
        InputType ReaderInputType { get; set; }
    }
}
