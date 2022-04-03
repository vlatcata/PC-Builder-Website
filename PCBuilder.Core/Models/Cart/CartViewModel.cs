namespace PCBuilder.Core.Models.Cart
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            Components = new List<AddComponentViewModel>();
        }

        public Guid CartId { get; set; }

        public string UserId { get; set; }

        public decimal TotalPrice { get; set; }

        public List<AddComponentViewModel> Components { get; set; }
    }
}
