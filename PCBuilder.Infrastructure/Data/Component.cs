using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCBuilder.Infrastructure.Data
{
    public class Component
    {
        public Component()
        {
            Id = Guid.NewGuid();
            Specifications = new List<Specification>();
            ComponentCarts = new List<CartComponent>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; }

        [Required]
        [StringLength(60)]
        public string Model { get; set; }

        [Required]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public ICollection<CartComponent> ComponentCarts { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [Required]
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }

        public List<Specification> Specifications { get; set; }
    }
}
