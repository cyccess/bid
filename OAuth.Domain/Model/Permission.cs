using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class Permission : AggregateRoot
    {
        public string PermissionAction { get; set; }

        public string PermissionName { get; set; }

        public string PermissionController { get; set; }

        public string Icon { get; set; }

        public string Description { get; set; }

        public bool IsButton { get; set; }

        public bool IsVisible { get; set; }

       
    }
}
