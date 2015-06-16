using System.Threading.Tasks;

namespace Microsoft.OData.Services.Interfaces
{
    public interface IReadable<TEntity>
    {
        Task<TEntity> Read();

        Task<string> ReadRaw();
    }
}
