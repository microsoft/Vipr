using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core {
    public interface IWriterConfigurationProvider {

        OutputType WriterOutputType { get; set; }

    }
}
