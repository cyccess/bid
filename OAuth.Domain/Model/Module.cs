using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace OAuth.Domain.Model
{
    public class Module : AggregateRoot
    {
        public string ModuleNo { get; set; }

        public string ModuleName { get; set; }
        public string ModuleUrl { get; set; }
        public string ParentNo { get; set; }
        public bool IsParent { get; set; }
        public MenuStatus Status { get; set; }
        public int OrderSort { get; set; }

        public string ModuleIcon { get; set; }

        public bool IsMenu { get; set; }

        public string ModuleController { get; set; }

        [ScriptIgnore]
        public DateTime AddDate { get; set; }

        public int ProjectId { get; set; }

        public virtual ICollection<ModulePermission> ModulePermissions { get; set; }
    }

    public enum MenuStatus : byte
    {
        Disabled,
        Enable
    }
}
