using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class Role : AggregateRoot
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public byte Status { get; set; }

        public DateTime AddDate { get; set; }

        public int ProjectId { get; set; }


        public virtual ICollection<RoleRight> RoleRights { get; set; }
    }
}
