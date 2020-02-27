using AzureFunctionDemo.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunctionDemo.Dal
{
    public interface IOperationDbContext : IEntityContext
    {
        DbSet<UserProfile> UserProfiles { get; set; }
    }
}