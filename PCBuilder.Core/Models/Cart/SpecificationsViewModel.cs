using System.ComponentModel.DataAnnotations;

namespace PCBuilder.Core.Models.Cart
{
    public class SpecificationsViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} cannot be empty or more than {1} symbols")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} cannot be empty or more than {1} symbols")]
        public string Description { get; set; }
    }
}
