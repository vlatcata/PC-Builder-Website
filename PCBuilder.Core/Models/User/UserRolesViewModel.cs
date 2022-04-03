using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBuilder.Core.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string[] RoleIds { get; set; }
    }
}
