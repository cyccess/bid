using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace OAuth.Domain.Model
{
    public class Project : AggregateRoot
    {
        public Guid Uniqueid { get; set; }

        public string Name { get; set; }

        public string LinkUrl { get; set; }


        public string Description { get; set; }


        public bool IsCommonUsed { get; set; }


        [ScriptIgnore]
        public DateTime AddDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
