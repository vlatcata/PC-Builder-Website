using System.ComponentModel.DataAnnotations;

namespace PCBuilder.Core.Models.Cart
{
    public class AddComponentViewModel
    {
        public AddComponentViewModel()
        {
            Specifications = new List<SpecificationsViewModel>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "{0} cannot be empty or more than {1} symbols")]
        public string Category { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "{0} cannot be empty or more than {1} symbols")]
        public string Model { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "{0} cannot be empty or more than {1} symbols")]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "{0} cannot be empty or more than {1} symbols")]
        public string ImageUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        public List<SpecificationsViewModel> Specifications { get; set; }
    }
}
