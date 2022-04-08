using Microsoft.EntityFrameworkCore;
using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models.Cart;
using PCBuilder.Core.Models.Home;
using PCBuilder.Infrastructure.Data;

namespace PCBuilder.Core.Services
{
    public class ComputerService : IComputerService
    {
        private readonly IApplicationDbRepository repo;

        public ComputerService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<bool> BuildComputer(CartViewModel model)
        {
            var result = false;

            var components = await repo.All<CartComponent>()
                .Where(c => c.CartId == model.CartId)
                .Select(c => c.Component)
                .ToListAsync();

            var computer = new Computer()
            {
                UserId = model.UserId,
                Components = components,
                Price = components.Sum(c => c.Price)
            };

            if (computer.Components.Count < 7)
            {
                return false;
            }

            foreach (var component in components)
            {
                var computerComponent = new ComputerComponent()
                {
                    ComputerId = computer.Id,
                    ComponentId = component.Id
                };

                computer.ComputerComponents.Add(computerComponent);
            }

            try
            {
                await repo.AddAsync(computer);
                await repo.SaveChangesAsync();

                result = true;
            }
            catch (InvalidOperationException ex)
            {
                result = false;
            }

            return result;
        }

        private Component GetComponentFromCart(string userId, string categoryName)
        {
            var cart = GetCart(userId);

            var components = cart.Components;

            var component = components.FirstOrDefault(c => c.Category.Name == categoryName);

            return component;
        }

        private Cart GetCart(string userId)
        {
            var cart = repo.All<Cart>()
                .Where(c => c.UserId == userId)
                .Include(c => c.Components)
                .ThenInclude(c => c.Specifications)
                .FirstOrDefault();

            return cart;
        }

        public async Task<ComputerViewModel> GetComputer(string computerId)
        {
            var computer = await repo.All<Computer>()
                .Where(c => c.Id.ToString() == computerId)
                .Include(c => c.Components)
                .Include(c => c.ComputerComponents)
                .Select(c => new ComputerViewModel()
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Price = c.Price,
                })
                .FirstOrDefaultAsync();

            var components = repo.All<ComputerComponent>()
                .Where(c => c.ComputerId == computer.Id)
                .Select(c => c.Component)
                .Select(c => new AddComponentViewModel()
                {
                    Category = c.Category.Name,
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    Manufacturer = c.Manufacturer,
                    Model = c.Model,
                    Price = c.Price,
                    Specifications = c.Specifications.Select(s => new SpecificationsViewModel()
                    {
                        Description = s.Description,
                        Id = s.Id,
                        Title = s.Title
                    })
                        .ToList()
                })
                .ToList();

            computer.Components = components;

            return computer;
        }

        public async Task<List<ComputerViewModel>> GetUserComputers(string userId)
        {
            var computers = await repo.All<Computer>()
                .Where(c => c.UserId == userId)
                .Include(c => c.Components)
                .Include(c => c.ComputerComponents)
                .Select(c => new ComputerViewModel()
                {
                    Id = c.Id,
                    UserId = userId,
                    Price = c.Price,
                })
                .ToListAsync();

            foreach (var computer in computers)
            {
                var components = repo.All<ComputerComponent>()
                .Where(c => c.ComputerId == computer.Id)
                .Select(c => c.Component)
                .Select(c => new AddComponentViewModel()
                {
                    Category = c.Category.Name,
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    Manufacturer = c.Manufacturer,
                    Model = c.Model,
                    Price = c.Price,
                    Specifications = c.Specifications.Select(s => new SpecificationsViewModel()
                    {
                        Description = s.Description,
                        Id = s.Id,
                        Title = s.Title
                    })
                        .ToList()
                })
                .ToList();

                computer.Components = components;
            }

            return computers;
        }
    }
}
