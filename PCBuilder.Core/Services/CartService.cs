using Microsoft.EntityFrameworkCore;
using PcBuilder.Infrastructure.Data.Repositories;
using PCBuilder.Core.Contracts;
using PCBuilder.Core.Models.Cart;
using PCBuilder.Infrastructure.Data;
using System.Text;

namespace PCBuilder.Core.Services
{
    public class CartService : ICartService
    {
        private readonly IApplicationDbRepository repo;

        public CartService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task<bool> CreateComponent(AddComponentViewModel model)
        {
            bool result = false;

            var component = new Component()
            {
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                Manufacturer = model.Manufacturer,
                Model = model.Model
            };

            var category = await repo.All<Category>()
            .Where(c => c.Name == model.Category)
            .FirstOrDefaultAsync();

            if (category == null)
            {
                category = new Category()
                {
                    Name = model.Category
                };
            }

            component.Category = category;

            var specification = model.Specifications.Select(s => new Specification()
            {
                Component = component,
                Id = s.Id,
                Title = s.Title,
                Description = s.Description
            })
                .ToList();

            component.Specifications = specification;

            try
            {
                await repo.AddAsync(component);
                await repo.SaveChangesAsync();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public async Task<AddComponentViewModel> GetComponent(string id)
        {
            var component = await repo.All<Component>()
                .Where(c => c.Id.ToString() == id)
                .Include(c => c.Category)
                .Select(c => new AddComponentViewModel()
                {
                    Id = c.Id,
                    Category = c.Category.Name,
                    ImageUrl = c.ImageUrl,
                    Manufacturer = c.Manufacturer,
                    Model = c.Model,
                    Price = c.Price,
                    Specifications = c.Specifications.Select(s => new SpecificationsViewModel()
                    {
                        Id = s.Id,
                        Description = s.Description,
                        Title = s.Title
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return component;
        }

        public async Task<AddComponentViewModel> GenerateDefaultModel()
        {
            var specifications = new List<SpecificationsViewModel>();

            specifications.Add(new SpecificationsViewModel()
            {
                Id = Guid.NewGuid(),
                Title = "",
                Description = ""
            });
            specifications.Add(new SpecificationsViewModel()
            {
                Id = Guid.NewGuid(),
                Title = "",
                Description = ""
            });
            specifications.Add(new SpecificationsViewModel()
            {
                Id = Guid.NewGuid(),
                Title = "",
                Description = ""
            });

            var component = new AddComponentViewModel()
            {
                Category = "",
                ImageUrl = "",
                Manufacturer = "",
                Model = "",
                Price = 0,
                Specifications = specifications
            };

            return component;
        }

        public async Task<List<AddComponentViewModel>> GetAllComponents(string name)
        {
            var components = await repo.All<Component>()
                .Where(c => c.Category.Name == name)
                .Select(c => new AddComponentViewModel()
                {
                    Id = c.Id,
                    Category = c.Category.Name,
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
                .ToListAsync();

            return components;
        }

        public async Task<bool> EditComponent(AddComponentViewModel model)
        {
            var result = false;

            var specifications = model.Specifications
                .Select(s => new Specification()
                {
                    Id = s.Id,
                    Title = s.Title,
                    Description = s.Description
                })
                .ToList();

            var component = await repo.GetByIdAsync<Component>(model.Id);

            if (component != null)
            {
                component.Model = model.Model;
                component.ImageUrl = model.ImageUrl;
                component.Manufacturer = model.Manufacturer;
                component.Price = model.Price;
                component.Specifications = specifications;

                await repo.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<(bool, string)> RemoveComponent(Guid id)
        {
            var result = false;

            var component = await repo.All<Component>()
                .Where(c => c.Id == id)
                .Include(c => c.Specifications)
                .Include(c => c.Category)
                .FirstOrDefaultAsync();

            if (component != null)
            {
                await repo.DeleteAsync<Component>(id);
                await repo.SaveChangesAsync();

                result = true;
            }

            string category = component.Category.Name;

            return (result, category);
        }

        private Cart GetCart(string userId)
        {
            var cart = repo.All<Cart>()
                .Where(c => c.UserId == userId)
                .Include(c => c.Components)
                .ThenInclude(c => c.Category)
                .FirstOrDefault();

            return cart;
        }

        public bool IsComponentInCart(string userId, string componentId)
        {
            CreateUserCart(userId);

            var cart = repo.All<Cart>()
                .Where(c => c.UserId == userId)
                .Include(c => c.Components)
                .FirstOrDefault();

            var component = repo.All<Component>()
                .Where(c => c.Id.ToString() == componentId)
                .Include(c => c.Category)
                .FirstOrDefault();

            var isInCart = cart.Components.Select(c => c.CategoryId == component.CategoryId).FirstOrDefault();

            return isInCart;
        }

        public async Task<(bool, string)> AddToCart(string userId, string componentId)
        {
            var cartExists = true;

            var cart = GetCart(userId);

            if (cart == null)
            {
                cart = new Cart()
                {
                    UserId = userId
                };

                cartExists = false;
            }

            var component = await repo.All<Component>()
                .Where(c => c.Id.ToString() == componentId)
                .Include(c => c.Category)
                .Include(c => c.Specifications)
                .FirstOrDefaultAsync();

            var componentCategory = component.Category.Name;

            if (cart.Components.Any(c => c.Category == component.Category))
            {
                return (false, null);
            }

            var cartComponent = new CartComponent()
            {
                CartId = cart.Id,
                ComponentId = component.Id
            };

            cart.CartComponents.Add(cartComponent);

            //if (component == null || cart.Components.Any(c => c.Category.Name == component.Category.Name))
            //{
            //    return false;
            //}

            cart.Components.Add(component);
            cart.TotalPrice = cart.Components.Sum(c => c.Price);

            if (!cartExists)
            {
                await repo.AddAsync(cart);
            }

            await repo.SaveChangesAsync();

            return (true, componentCategory);
        }

        public async Task<CartViewModel> GetCartComponents(string userId)
        {
            CreateUserCart(userId);

            var cart = await repo.All<Cart>()
                .Where(c => c.UserId == userId)
                .Include(c => c.Components)
                .Select(c => new CartViewModel()
                {
                    UserId = userId,
                    CartId = c.Id,
                    Components = c.Components.Select(co => new AddComponentViewModel()
                    {
                        Category = co.Category.ToString(),
                        Id = co.Id,
                        ImageUrl = co.ImageUrl,
                        Manufacturer = co.Manufacturer,
                        Model = co.Model,
                        Price = co.Price,
                        Specifications = co.Specifications.Select(s => new SpecificationsViewModel()
                        {
                            Description = s.Description,
                            Id = s.Id,
                            Title = s.Title
                        })
                        .ToList()
                    })
                    .ToList(),
                    TotalPrice = c.TotalPrice,
                })
                .FirstOrDefaultAsync();

            return cart;
        }

        private Cart CreateUserCart(string userId)
        {
            var cart = repo.All<Cart>()
                .Where(c => c.UserId == userId)
                .FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart()
                {
                    UserId = userId,
                };

                repo.AddAsync(cart);
                repo.SaveChanges();
            }

            return cart;
        }

        public string CheckMissingComponents(string userId)
        {
            var cart = GetCart(userId);

            StringBuilder sb = new StringBuilder();

            List<string> missingCategories = new List<string>();

            List<string> allCategories = repo.All<Category>()
                .Select(c => c.Name)
                .ToList();

            List<string> cartCategories = cart.Components
                .Select(c => c.Category.Name)
                .ToList();

            foreach (var category in allCategories)
            {
                if (!cartCategories.Contains(category))
                {
                    missingCategories.Add(category);
                    sb.Append($"{category}, ");
                }
            }

            if (sb.Length >= 3)
            {
                sb.Length -= 2;
            }

            return sb.ToString().TrimEnd();
        }

        public async Task<(bool, string)> RemoveFromCart(string userId, string componentId)
        {
            var result = false;

            var cart = GetCart(userId);

            if (cart == null)
            {
                result = false;
            }

            var componentToRemoveFromCart = cart.Components.Where(c => c.Id.ToString() == componentId).FirstOrDefault();
            var componentToRemove = repo.All<CartComponent>().Where(c => c.CartId == cart.Id && c.ComponentId.ToString() == componentId).FirstOrDefault();

            var categoryName = componentToRemoveFromCart.Category.Name;

            if (cart.Components.Remove(componentToRemoveFromCart) && cart.CartComponents.Remove(componentToRemove))
            {
                await repo.SaveChangesAsync();
                result = true;
            }

            return (result, categoryName);
        }

        public async Task<bool> ClearCart(string cartId)
        {
            var result = false;

            var cart = await repo.All<Cart>()
                .Where(c => c.Id.ToString() == cartId)
                .Include(c => c.Components)
                .Include(c => c.CartComponents)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                result = false;
            }

            foreach (var component in cart.CartComponents.ToList())
            {
                cart.CartComponents.Remove(component);
            }

            cart.Components.Clear();
            cart.TotalPrice = 0;

            await repo.SaveChangesAsync();
            result = true;

            return result;
        }
    }
}
