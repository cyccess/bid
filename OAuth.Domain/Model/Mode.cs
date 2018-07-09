using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class Mode : AggregateRoot
    {
   

        public string ModeKey { get; set; }

        public string ModeName { get; set; }

        public int ParentID { get; set; }

        public bool IsEnabled { get; set; }
    }
}
