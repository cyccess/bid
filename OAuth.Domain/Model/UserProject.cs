using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class UserProject : AggregateRoot
    {
        public int UserId { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
