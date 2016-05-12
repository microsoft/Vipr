#if false
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vipr.Core.CodeModel.Vocabularies.Capabilities;

namespace Vipr.Writer.CSharp.Lite
{
    public class OdcmUpdateCapability : OdcmBooleanCapability
    {
        public OdcmUpdateCapability() : base(true, TermNames.Updatable)
        {
        }
    }

    public class OdcmUpdateLinkCapability : OdcmBooleanCapability
    {
        public OdcmUpdateLinkCapability() : base(true, TermNames.NonUpdatableNavigationProperties)
        {
        }
    }

    public class OdcmDeleteCapability : OdcmBooleanCapability
    {
        public OdcmDeleteCapability() : base(true, TermNames.Deletable)
        {
        }
    }

    public class OdcmDeleteLinkCapability : OdcmBooleanCapability
    {
        public OdcmDeleteLinkCapability() : base(true, TermNames.NonDeletableNavigationProperties)
        {
        }
    }

    public class OdcmInsertCapability : OdcmBooleanCapability
    {
        public OdcmInsertCapability() : base(true, TermNames.Insertable)
        {
        }
    }

    public class OdcmInsertLinkCapability : OdcmBooleanCapability
    {
        public OdcmInsertLinkCapability() : base(true, TermNames.NonInsertableNavigationProperties)
        {
        }
    }

    public class OdcmExpandCapability : OdcmBooleanCapability
    {
        public OdcmExpandCapability() : base(true, TermNames.Expandable)
        {
        }
    }

    public class OdcmExpandLinkCapability : OdcmBooleanCapability
    {
        public OdcmExpandLinkCapability() : base(true, TermNames.NonExpandableProperties)
        {
        }
    }
}
#endif