using PCBuilder.Core.Models;
using PCBuilder.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
