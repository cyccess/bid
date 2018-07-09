using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.ModelDto
{
    public class RoleDto
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public IEnumerable<Module> Modules { get; set; }

        public IEnumerable<RoleRightDto> RoleRightDto { get; set; }
    }


    public class RoleRightDto
    {
        public int Id { get; set; }

        public int RoleId { get; set; }

        public int ModuleId { get; set; }
    }
}
