using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Services;
using PCBuilder.Infrastructure.Data;
using PCBuilder.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
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
        public void Test1()
        {
            Assert.Pass();
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
                Name = "CPU"
            };

            var components = new List<Component>();
            var component = new Component()
            {
                Category = category,
                CategoryId = category.Id,
                Id = new Guid("383b2808-693e-44ba-b0d3-4020c55fa098"),
                ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                Manufacturer = "Nvidia",
                Model = "Some case",
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
        }
    }
}