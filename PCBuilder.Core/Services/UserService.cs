using Microsoft.EntityFrameworkCore;
using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models;
using PCBuilder.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBuilder.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;

        public UserService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await repo.GetByIdAsync<ApplicationUser>(id);
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            return await repo.All<ApplicationUser>()
                .Select(u => new UserListViewModel()
                {
                    Email = u.Email,
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}"
                })
                .ToListAsync();
        }

        public async Task<UserEditViewModel> GetUserToEdit(string userId)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);

            return new UserEditViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public async Task<bool> UpdateUser(UserEditViewModel model)
        {
            bool result = false;

            var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);

            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                await repo.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
