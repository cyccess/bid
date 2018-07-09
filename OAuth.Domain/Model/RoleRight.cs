using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    /// <summary>
    /// 用户拥有的权限
    /// </summary>
    public class RoleRight : AggregateRoot
    {
        public int RoleId { get; set; }

        public int ModuleId { get; set; }

        //public virtual Menu Menu { get; set; }

    }
}
