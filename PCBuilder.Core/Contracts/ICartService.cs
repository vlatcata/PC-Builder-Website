using PCBuilder.Core.Models.Cart;

namespace PCBuilder.Core.Contracts
{
    public interface ICartService
    {
        Task<bool> CreateComponent(AddComponentViewModel model);
        Task<AddComponentViewModel> GenerateDefaultModel();
        Task<List<AddComponentViewModel>> GetAllComponents(string name);
        Task<AddComponentViewModel> GetComponent(Guid id);
        Task<bool> EditComponent(AddComponentViewModel model);
        Task<(bool, string)> RemoveComponent(Guid id);
        Task<(bool, string)> AddToCart(string userId, Guid productId);
        Task<CartViewModel> GetCartComponents(string userId);
        Task<(bool, string)> RemoveFromCart(string userId, Guid productId);
        Task<bool> ClearCart(Guid cartId);
        bool IsComponentInCart(string userId, Guid componentId);
        string CheckMissingComponents(string userId);
    }
}
