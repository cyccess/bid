using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class ModulePermission : AggregateRoot
    {
        public int ModuleId { get; set; }

        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
