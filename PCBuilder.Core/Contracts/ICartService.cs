using PCBuilder.Core.Models.Cart;

namespace PCBuilder.Core.Contracts
{
    public interface ICartService
    {
        Task<bool> CreateComponent(AddComponentViewModel model);
        Task<AddComponentViewModel> GenerateDefaultModel();
        Task<List<AddComponentViewModel>> GetAllComponents(string name);
        Task<AddComponentViewModel> GetComponent(string id);
        Task<bool> EditComponent(AddComponentViewModel model);
        Task<bool> RemoveComponent(Guid id);
        Task<bool> AddToCart(string userId, string productId);
        Task<CartViewModel> GetCartComponents(string userId);
        Task<bool> RemoveFromCart(string userId, string productId);
        Task<bool> ClearCart(string cartId);

        bool IsComponentInCart(string userId, string componentId);
    }
}
