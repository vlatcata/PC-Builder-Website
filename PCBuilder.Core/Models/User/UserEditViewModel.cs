using System.ComponentModel.DataAnnotations;

namespace PCBuilder.Core.Models
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
