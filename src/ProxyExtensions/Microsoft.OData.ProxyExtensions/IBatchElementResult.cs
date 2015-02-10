using System;

namespace Microsoft.OData.ProxyExtensions
{
    public interface IBatchElementResult
    {
        IPagedCollection SuccessResult { get; }
        Exception FailureResult { get; }
    }
}