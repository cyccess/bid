using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    /// <summary>
    /// 用户拥有的角色
    /// </summary>
    public class UserRole : AggregateRoot
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public byte Status { get; set; }

        
        public virtual Role Role { get; set; }
    }
}
