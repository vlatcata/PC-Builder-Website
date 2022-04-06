using System.ComponentModel.DataAnnotations.Schema;

namespace PCBuilder.Infrastructure.Data
{
    public class ComputerComponent
    {
        [ForeignKey(nameof(Computer))]
        public Guid ComputerId { get; set; }
        public Computer Computer { get; set; }

        [ForeignKey(nameof(Component))]
        public Guid ComponentId { get; set; }
        public Component Component { get; set; }
    }
}
