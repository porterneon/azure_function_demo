using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AzureFunctionDemo.Dal
{
    public interface IEntityContext
    {
        Task<int> SaveAsync();

        EntityEntry UpdateEntity(object entity);

        Task<EntityEntry> AddEntityAsync(object entity);

        EntityEntry AddEntity(object entity);
    }
}