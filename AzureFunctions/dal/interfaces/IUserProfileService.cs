using System.Threading.Tasks;
using AzureFunctionDemo.Dal.Models;

namespace AzureFunctionDemo.Dal.Interfaces
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetAsync(string id);

        Task<int> UpdateAsync(UserProfile entity, UserProfile model);

        Task<int> InsertAsync(UserProfile model);
    }
}