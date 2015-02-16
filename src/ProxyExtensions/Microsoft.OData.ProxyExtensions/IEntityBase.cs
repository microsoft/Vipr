namespace Microsoft.OData.ProxyExtensions
{
    public interface IEntityBase
    {
        /// <param name=""dontSave"">true to delay saving until batch is saved; false to save immediately.</param>
        global::System.Threading.Tasks.Task UpdateAsync(bool dontSave = false);
        /// <param name=""dontSave"">true to delay saving until batch is saved; false to save immediately.</param>
        global::System.Threading.Tasks.Task DeleteAsync(bool dontSave = false);
    }
}