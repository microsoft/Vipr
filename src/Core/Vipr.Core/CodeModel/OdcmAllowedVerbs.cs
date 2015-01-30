using System;

namespace Vipr.Core.CodeModel
{
    [Flags]
    public enum OdcmAllowedVerbs
    {
        Delete = 1,
        Get = 2,
        Patch = 4,
        Post = 8,
        Put = 16,
        Any = Delete | Get | Patch | Post | Put
    }
}
