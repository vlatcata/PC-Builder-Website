using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCBuilder.Infrastructure.Data
{
    public class Specification
    {
        public Specification()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [ForeignKey(nameof(ComponentId))]
        public Component Component { get; set; }
        public Guid ComponentId { get; set; }
    }
}
