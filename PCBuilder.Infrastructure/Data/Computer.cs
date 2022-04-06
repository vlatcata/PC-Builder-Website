using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCBuilder.Infrastructure.Data
{
    public class Computer
    {
        public Computer()
        {
            Id = Guid.NewGuid();
            Components = new List<Component>();
            ComputerComponents = new List<ComputerComponent>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public ICollection<ComputerComponent> ComputerComponents { get; set; }

        [Required]
        public List<Component> Components { get; set; }
    }
}
