using PCBuilder.Core.Models.Cart;
using PCBuilder.Core.Models.Home;

namespace PCBuilder.Core.Contracts
{
    public interface IComputerService
    {
        Task<bool> BuildComputer(CartViewModel model);
        Task<List<ComputerViewModel>> GetUserComputers(string userId);
        Task<ComputerViewModel> GetComputer(string computerId);
    }
}
