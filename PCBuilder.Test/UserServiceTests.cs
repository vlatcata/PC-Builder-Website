using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models;
using PCBuilder.Core.Services;
using PCBuilder.Infrastructure.Data;
using PCBuilder.Infrastructure.Data.Identity;
using PCBuilder.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCBuilder.Test
{
    public class UserServiceTests
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDBAsync(repo);
        }

        [Test]
        public async Task GetUserByIdMustPassWithCorrectId()
        {
            var service = serviceProvider.GetService<IUserService>();

            var result = await service.GetUserById("asd123asd123");

            Assert.AreEqual("asd123asd123", result.Id);
        }

        [Test]
        public async Task GetUserByIdMustThrowWithIncorrectId()
        {
            var service = serviceProvider.GetService<IUserService>();

            var result = await service.GetUserById("asd123asd000");

            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task GetUsersMustReturnCorrectAnswer()
        {
            var service = serviceProvider.GetService<IUserService>();

            var user = new ApplicationUser()
            {
                Id = "asd123asd123",
                Email = "asd@abv.bg",
                UserName = "testUser",
                PasswordHash = null
            };

            var result = await service.GetUsers();

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task GetUserToEditMustPassWithValidInformation()
        {
            var service = serviceProvider.GetService<IUserService>();

            var result = await service.GetUserToEdit("asd123asd123");

            Assert.AreEqual("asd123asd123", result.Id);
        }

        [Test]
        public async Task GetUserToEditMustNotPassWithInvalidInformation()
        {
            var service = serviceProvider.GetService<IUserService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.GetUserToEdit("asd123asd000"));
        }

        [Test]
        public async Task UpdateUserMustUpdateCorrect()
        {
            var service = serviceProvider.GetService<IUserService>();

            var editedUser = new UserEditViewModel()
            {
                Id = "asd123asd123",
                FirstName = "Ivan",
                LastName = "Petrov",
                Email = "ivanpetrov@abv.bg"
            };

            var result = await service.UpdateUser(editedUser);

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task UpdateUserMustNotUpdateWithIncorrectInformation()
        {
            var service = serviceProvider.GetService<IUserService>();

            var editedUser = new UserEditViewModel()
            {
                Id = "asd123asd000",
                FirstName = "Ivan",
                LastName = "Petrov",
                Email = "ivanpetrov@abv.bg"
            };

            var result = await service.UpdateUser(editedUser);

            Assert.AreEqual(false, result);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDBAsync(IApplicationDbRepository repo)
        {
            var user = new ApplicationUser()
            {
                Id = "asd123asd123",
                Email = "asd@abv.bg",
                UserName = "testUser",
                PasswordHash = null
            };

            await repo.AddAsync(user);
            await repo.SaveChangesAsync();

            var cart = new Cart()
            {
                Id = new Guid("b40666de-fed8-4ce2-a790-fdfaca9e5627"),
                UserId = user.Id,
                Components = new List<Component>()
            };

            await repo.AddAsync(cart);
            await repo.SaveChangesAsync();
        }
    }
}
