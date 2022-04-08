using PCBuilder.Core.Models;
using PCBuilder.Infrastructure.Data.Identity;

namespace PCBuilder.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();

        Task<UserEditViewModel> GetUserToEdit(string userId);

        Task<bool> UpdateUser(UserEditViewModel model);

        Task<ApplicationUser> GetUserById(string id);
    }
}
