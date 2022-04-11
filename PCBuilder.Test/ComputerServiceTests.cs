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
using System.Text;
using System.Threading.Tasks;

namespace PCBuilder.Test
{
    public class ComputerServiceTests
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
                .AddSingleton<IComputerService, ComputerService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDBAsync(repo);
        }

        [Test]
        public async Task BuildComputerMustReturnFalseWithIncorrectInformation()
        {
            var service = serviceProvider.GetService<IComputerService>();

            var model = new CartViewModel()
            {
                Components = new List<AddComponentViewModel>()
            };

            var result = await service.BuildComputer(model);

            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task GetComputerMustReturnIncorrectValue()
        {
            var service = serviceProvider.GetService<IComputerService>();

            Assert.CatchAsync<InvalidOperationException>(async () => await service.GetComputer("64a74302-4510-40df-9e43-7a14c180c000"));
        }

        [Test]
        public async Task GetUserComputersMustReturnCorrectValue()
        {
            var service = serviceProvider.GetService<IComputerService>();

            var result = await service.GetUserComputers("asd123asd123");

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task GetUserComputersMustReturnIncorrectValue()
        {
            var service = serviceProvider.GetService<IComputerService>();

            var result = await service.GetUserComputers("asd123asd000");

            Assert.AreEqual(0, result.Count);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDBAsync(IApplicationDbRepository repo)
        {
            var computer = new Computer()
            {
                Id = new Guid("64a74302-4510-40df-9e43-7a14c180c5e3"),
                UserId = "asd123asd123",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("0bd6ac7d-4bc0-4fd3-8422-a2810ea2334d"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some CPU",
                        Price = 320,
                        Category = new Category()
                        {
                            Name = "CPU"
                        }
                    },
                    new Component()
                    {
                        Id = new Guid("d2d13c4d-0bc5-4065-a218-a7632a11cfd6"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some GPU",
                        Price = 320,
                        Category = new Category()
                        {
                            Name = "GPU"
                        }
                    },
                    new Component()
                    {
                        Id = new Guid("a80521db-e248-4748-b90b-fa945b05f42d"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some SSD",
                        Price = 320,
                        Category = new Category()
                        {
                            Name = "SSD"
                        }
                    },
                    new Component()
                    {
                        Id = new Guid("5ffdfa0b-4c1e-4a6d-b457-8b928bd2ccdd"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some RAM",
                        Price = 320,
                        Category = new Category()
                        {
                            Name = "RAM"
                        }
                    },
                    new Component()
                    {
                        Id = new Guid("6a463778-6856-4018-863f-4ea185aae826"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some Power Supply",
                        Price = 320,
                        Category = new Category()
                        {
                            Name = "Power Supply"
                        }
                    },
                    new Component()
                    {
                        Id = new Guid("a13e12cd-eb82-4ac6-b526-3276f2a95e07"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some Motherboard",
                        Price = 320,
                        Category = new Category()
                        {
                            Name = "Motherboard"
                        }
                    },
                    new Component()
                    {
                        Id = new Guid("401bfc3c-be7d-4a75-ab47-bfce56344f68"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some Case",
                        Price = 320,
                        Category = new Category()
                        {
                            Name = "Case"
                        }
                    }
                }
            };

            await repo.AddAsync(computer);
            await repo.SaveChangesAsync();

            var CPU = new Category()
            {
                Id = new Guid("3c6841f1-8d5b-4c67-a733-f91fdb2e676e"),
                Name = "CPU",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("b04f538e-8c51-4fea-8eb7-fae64bc70f27"),
                        ImageUrl = "https://www.xda-developers.com/files/2021/12/Fractal-Design-Meshify-2-Compact-black-color.jpg",
                        Manufacturer = "Intel",
                        Model = "Some CPU",
                        Price = 320
                    }
                }
            };

            await repo.AddAsync(CPU);
            await repo.SaveChangesAsync();

            var GPU = new Category()
            {
                Id = new Guid("9b7734da-9d5d-4fa7-a521-4021062819f8"),
                Name = "GPU",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("21a70da4-03d0-466e-b425-62013da0c274"),
                        ImageUrl = "",
                        Manufacturer = "Nvidia",
                        Model = "RTX",
                        Price = 420
                    }
                }
            };

            await repo.AddAsync(GPU);
            await repo.SaveChangesAsync();

            var SSD = new Category()
            {
                Id = new Guid("3741cf7c-1d50-43a4-9dc8-1931bbb0cb3c"),
                Name = "SSD",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("57a1f4f8-c3c6-4cd0-b703-0b4c9af39a6e"),
                        ImageUrl = "",
                        Manufacturer = "Samsung",
                        Model = "Some SSD",
                        Price = 55
                    }
                }
            };

            await repo.AddAsync(SSD);
            await repo.SaveChangesAsync();

            var pcCase = new Category()
            {
                Id = new Guid("2b2b0ca2-f674-4f06-a2de-9d7f30f4574c"),
                Name = "Case",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("5849732a-1382-4abf-b85c-0d69011f9b43"),
                        ImageUrl = "",
                        Manufacturer = "ColorMaster",
                        Model = "Some Case",
                        Price = 55
                    }
                }
            };

            await repo.AddAsync(pcCase);
            await repo.SaveChangesAsync();

            var powerSupply = new Category()
            {
                Id = new Guid("03f88b77-a951-4fa4-a784-229feeb5821d"),
                Name = "Power Supply",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("6294ce6d-213e-4a73-bf63-87feda706026"),
                        ImageUrl = "",
                        Manufacturer = "ColorMaster",
                        Model = "Some Power Supply",
                        Price = 60
                    }
                }
            };

            await repo.AddAsync(powerSupply);
            await repo.SaveChangesAsync();

            var motherboard = new Category()
            {
                Id = new Guid("a5570f56-b6be-41fa-b844-1879f9a3b1e8"),
                Name = "Motherboard",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("86cdc858-37af-449f-b948-f6cae381e850"),
                        ImageUrl = "",
                        Manufacturer = "AORUS",
                        Model = "Some Motherboard",
                        Price = 150
                    }
                }
            };

            await repo.AddAsync(motherboard);
            await repo.SaveChangesAsync();

            var RAM = new Category()
            {
                Id = new Guid("4b017775-43ab-4895-ac57-e96fbc37cac2"),
                Name = "RAM",
                Components = new List<Component>()
                {
                    new Component()
                    {
                        Id = new Guid("436c06b3-971a-4f9e-bf21-02671af33b88"),
                        ImageUrl = "",
                        Manufacturer = "Intel",
                        Model = "Some RAM",
                        Price = 150
                    }
                }
            };

            await repo.AddAsync(RAM);
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
                Components = new List<Component>()
            };

            await repo.AddAsync(cart);
            await repo.SaveChangesAsync();
        }
    }
}
