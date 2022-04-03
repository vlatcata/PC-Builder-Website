using PCBuilder.Core.Models.Cart;

namespace PCBuilder.Core.Models.Home
{
    public class ComputerViewModel
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public decimal Price { get; set; }

        public List<AddComponentViewModel> Components { get; set; }
    }
}
