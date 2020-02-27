using System.Threading.Tasks;
using AzureFunctionDemo.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AzureFunctionDemo.Dal
{
    public class OperationDbContext : DbContext, IOperationDbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }

        public OperationDbContext(DbContextOptions<OperationDbContext> options)
                    : base(options)
        {
        }

        public async Task<int> SaveAsync()
        {
            return await SaveChangesAsync();
        }

        public async Task<EntityEntry> AddEntityAsync(object entity)
        {
            return await base.AddAsync(entity);
        }

        public EntityEntry UpdateEntity(object entity)
        {
            return base.Update(entity);
        }

        public EntityEntry AddEntity(object entity)
        {
            return base.Add(entity);
        }
    }
}