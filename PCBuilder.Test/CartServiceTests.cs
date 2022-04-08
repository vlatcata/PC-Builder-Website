using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models.Cart;
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
    public class CartServiceTests
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
                .AddSingleton<ICartService, CartService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDBAsync(repo);
        }

        [Test]
        public void CreatingCorrectComponentMustNotThrow()
        {
            var service = serviceProvider.GetService<ICartService>();

            var component = new AddComponentViewModel()
            {
                Category = "Case",
                ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                Manufacturer = "Nvidia",
                Model = "Some case",
                Price = 320
            };

            var specifications = new List<SpecificationsViewModel>();
            var specification = new SpecificationsViewModel()
            {
                Title = "Case Type",
                Description = "ATX"
            };
            specifications.Add(specification);

            component.Specifications = specifications;

            var result = service.CreateComponent(component).Result;

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task CreatingIncorrectCorrectComponentMustThrow()
        {
            var service = serviceProvider.GetService<ICartService>();

            var component = new AddComponentViewModel()
            {
                Category = "CPU",
                ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                Price = 320
            };

            var result = await service.CreateComponent(component);

            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task CreateComponentWithNoCategoryMustCreatecategory()
        {
            var service = serviceProvider.GetService<ICartService>();

            var component = new AddComponentViewModel()
            {
                ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                Price = 320,
                Category = "Power Supply"
            };

            var result = await service.CreateComponent(component);

            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task GetExistingComponentMustNotThrow()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.GetComponent(new Guid("383b2808-693e-44ba-b0d3-4020c55fa098"));
            var component = new AddComponentViewModel()
            {
                Id = new Guid("383b2808-693e-44ba-b0d3-4020c55fa098")
            };

            Assert.AreEqual(component.Id, result.Id);
        }

        [Test]
        public async Task GetNonExistingComponentMustThrow()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.GetComponent(new Guid("383b2808-693e-44ba-b0d3-4020c55fa000"));

            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task GenerateDefaultModelMustBeSuccessfull()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.GenerateDefaultModel();

            var component = new AddComponentViewModel()
            {
                Specifications = new List<SpecificationsViewModel>()
                {
                    new SpecificationsViewModel(),
                    new SpecificationsViewModel(),
                    new SpecificationsViewModel()

                }
            };

            Assert.AreEqual(component.Specifications.Count, result.Specifications.Count);
        }

        [Test]
        public async Task GetAllComponentsMustReturnWithValidCategoryname()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.GetAllComponents("CPU");

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetAllComponentsMustReturnWithInvalidCategoryname()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.GetAllComponents("Power Supply");

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task EditComponentMustEditSuccessfully()
        {
            var service = serviceProvider.GetService<ICartService>();

            var component = new AddComponentViewModel()
            {
                Id = new Guid("383b2808-693e-44ba-b0d3-4020c55fa098"),
                Category = "CPU",
                ImageUrl = "asdasdasd",
                Manufacturer = "Intel",
                Model = "Some CPU",
                Price = 200,
            };

            var specifications = new List<SpecificationsViewModel>();
            specifications.Add(new SpecificationsViewModel()
            {
                Id = new Guid("a7183c24-55e2-461f-9329-48b9257e3f2f"),
                Title = "Case Type",
                Description = "ATX"
            });

            component.Specifications = specifications;

            var result = await service.EditComponent(component);

            Assert.AreEqual(true, result);
        }


        [Test]
        public async Task EditInvalidComponentMustNotEdit()
        {
            var service = serviceProvider.GetService<ICartService>();

            var component = new AddComponentViewModel()
            {
                Id = new Guid("96e56783-b03b-4551-8061-24468031b000"),
            };

            var result = await service.EditComponent(component);

            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task RemoveExistingComponentMustPass()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.RemoveComponent(new Guid("383b2808-693e-44ba-b0d3-4020c55fa098"));

            Assert.AreEqual((true, "CPU"), result);
        }

        [Test]
        public async Task RemoveNonExistingComponentMustThrow()
        {
            var service = serviceProvider.GetService<ICartService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.RemoveComponent(new Guid("383b2808-693e-44ba-b0d3-4020c55fa000")));
        }

        [Test]
        public async Task IsComponentInCartWithValidUserMustPass()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = service.IsComponentInCart("asd123asd123", new Guid("383b2808-693e-44ba-b0d3-4020c55fa098"));

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task IsComponentInCartWithInvalidUserMustNotPass()
        {
            var service = serviceProvider.GetService<ICartService>();

            Assert.CatchAsync<NullReferenceException>(async () => service.IsComponentInCart("asd123asd123", new Guid("383b2808-693e-44ba-b0d3-4020c55fa000")));
        }

        [Test]
        public async Task AddToCartWithValidDataMustPass()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.AddToCart("asd123asd123", new Guid("e4e604a3-1ad7-4192-8495-3c00f81f9a84"));

            Assert.AreEqual((true, "Case"), result);
        }

        [Test]
        public async Task AddToCartWithInvalidDataMustNotPass()
        {
            var service = serviceProvider.GetService<ICartService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.AddToCart("asd123asd123", new Guid("e4e604a3-1ad7-4192-8495-3c00f81f9000")));
        }

        [Test]
        public async Task AddToCartWithExistingCategoryMustFail()
        {
            var service = serviceProvider.GetService<ICartService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.AddToCart("asd123asd123", new Guid("4ea95f07-a20d-4b04-b8f6-b14b450ee762")));
        }

        [Test]
        public async Task AddToCartWithNoCartMustCreateOne()
        {
            var service = serviceProvider.GetService<ICartService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.AddToCart("asd123asd000", new Guid("b53c126d-c752-42e5-8a0a-1e7e2b18b22f")));
        }

        [Test]
        public async Task IfCategoryInCartExistsTestMustFail()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.AddToCart("asd123asd123", new Guid("383b2808-693e-44ba-b0d3-4020c55fa098"));

            bool first = false;
            string second = null;

            Assert.AreEqual((first, second), result);
        }

        [Test]
        public async Task GetCartComponentsMustPassWithValidUser()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.GetCartComponents("asd123asd123");

            Assert.AreEqual(1, result.Components.Count);
        }

        [Test]
        public async Task GetCartComponentsMustNotPassWithInvalidUser()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.GetCartComponents("asd123asd000");

            Assert.AreEqual(0, result.Components.Count);
        }

        [Test]
        public async Task CheckMissingComponentsMustReturnCorrectString()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = service.CheckMissingComponents("asd123asd123");

            Assert.AreEqual("GPU, Case", result);
        }

        [Test]
        public async Task ClearCartMustSucceedWithValidData()
        {
            var service = serviceProvider.GetService<ICartService>();

            var result = await service.ClearCart(new Guid("b40666de-fed8-4ce2-a790-fdfaca9e5627"));

            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task ClearCartMustNotSucceedWithInvalidData()
        {
            var service = serviceProvider.GetService<ICartService>();

            Assert.CatchAsync<NullReferenceException>(async () => await service.ClearCart(new Guid("b40666de-fed8-4ce2-a790-fdfaca9e5000")));
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDBAsync(IApplicationDbRepository repo)
        {
            var category = new Category()
            {
                Id = new Guid("96e56783-b03b-4551-8061-24468031bf26"),
                Name = "CPU",
            };

            var components = new List<Component>();
            var component = new Component()
            {
                Category = category,
                CategoryId = category.Id,
                Id = new Guid("383b2808-693e-44ba-b0d3-4020c55fa098"),
                ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                Manufacturer = "Intel",
                Model = "Some CPU",
                Price = 320
            };
            components.Add(component);

            var specifications = new List<Specification>();
            var specification = new Specification()
            {
                Id = new Guid("a7183c24-55e2-461f-9329-48b9257e3f2f"),
                Component = component,
                ComponentId = component.Id,
                Title = "Case Type",
                Description = "ATX"
            };
            specifications.Add(specification);

            component.Specifications = specifications;
            category.Components = components;

            await repo.AddAsync(category);
            await repo.SaveChangesAsync();

            var category3 = new Category()
            {
                Id = new Guid("e46d043b-e543-40d0-b09d-6ecfa414fba8"),
                Name = "GPU",
            };

            var components3 = new List<Component>();
            var component3 = new Component()
            {
                Category = category,
                CategoryId = category.Id,
                Id = new Guid("4ea95f07-a20d-4b04-b8f6-b14b450ee762"),
                ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                Manufacturer = "Nvidia",
                Model = "Some GPU",
                Price = 400
            };
            components.Add(component);

            var specifications3 = new List<Specification>();
            var specification3 = new Specification()
            {
                Id = new Guid("b53c126d-c752-42e5-8a0a-1e7e2b18b22f"),
                Component = component,
                ComponentId = component.Id,
                Title = "Case Type",
                Description = "Mini ATX"
            };
            specifications3.Add(specification3);

            component3.Specifications = specifications3;
            category3.Components = components3;

            await repo.AddAsync(category3);
            await repo.SaveChangesAsync();

            var category2 = new Category()
            {
                Id = new Guid("acc46f5e-f42c-43bd-92f8-b830b27599e8"),
                Name = "Case",
            };
            var components2 = new List<Component>();

            var component2 = new Component()
            {
                Id = new Guid("e4e604a3-1ad7-4192-8495-3c00f81f9a84"),
                Category = category2,
                ImageUrl = "asd",
                Model = "Case Model",
                Manufacturer = "Case Manufacturer",
                Price = 50
            };
            components2.Add(component2);
            category2.Components = components2;

            await repo.AddAsync(category2);
            await repo.SaveChangesAsync();

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
                Components = components,
            };

            await repo.AddAsync(cart);
            await repo.SaveChangesAsync();
        }
    }
}