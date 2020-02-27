using System.Threading.Tasks;
using AzureFunctionDemo.Dal.Interfaces;
using AzureFunctionDemo.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunctionDemo.Dal.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IOperationDbContext _context;

        public UserProfileService(IOperationDbContext context)
        {
            _context = context;
        }

        public Task<UserProfile> GetAsync(string id)
        {
            return _context.UserProfiles.FirstOrDefaultAsync(i => i.GlobalId == id);
        }

        public async Task<int> InsertAsync(UserProfile model)
        {
            await _context.AddEntityAsync(model);
            return await _context.SaveAsync();
        }

        public async Task<int> UpdateAsync(UserProfile entity, UserProfile model)
        {
            entity.Domain = model.Domain;
            entity.Email = model.Email;
            entity.ManagerEmail = model.ManagerEmail;
            entity.Name = model.Name;
            entity.GlobalId = model.GlobalId;
            entity.Language = model.Language;
            entity.LastUpdateDate = model.LastUpdateDate;
            entity.PhoneNumber = model.PhoneNumber;

            _context.UpdateEntity(entity);
            return await _context.SaveAsync();
        }
    }
}