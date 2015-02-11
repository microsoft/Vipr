using System;
using Vipr.Core.CodeModel;

namespace Vipr.Core {
    public interface IViprWriter {
        /// <summary>
        /// Construct a IViprWriter to create source code from a code model.
        /// </summary>
        /// <param name="model">Code model to be translated.</param>
        /// <param name="config">Additional configuration parameters.</param>
        // public IViprWriter(OdcmModel model, IConfigurationProvider config);

        String GenerateProxy(OdcmModel model, IWriterConfigurationProvider config);
    }
}
