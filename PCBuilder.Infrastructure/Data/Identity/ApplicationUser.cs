using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCBuilder.Infrastructure.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }
    }
}
