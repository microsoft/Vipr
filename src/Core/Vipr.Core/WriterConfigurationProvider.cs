using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vipr.Core {
    public class WriterConfigurationProvider : IWriterConfigurationProvider {

        public WriterConfigurationProvider() {
            m_options = new Dictionary<string, string>();
            m_extras = new HashSet<string>();
        }

        public OutputType WriterOutputType { get; set; }

        public IViprReader Reader { get; set; }

        public IViprWriter Writer { get; set; }

        private Dictionary<string, string> m_options;
        private HashSet<string> m_extras;


        public IDictionary<string, string> Options {
            get { return m_options; }
        }

        public ISet<string> ValueOnlyOptions {
            get { return m_extras; }
            set { m_extras = (HashSet<string>) value; }
        }
    }
}
