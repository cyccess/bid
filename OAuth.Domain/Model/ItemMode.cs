using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class ItemMode : AggregateRoot
    {
        public int ItemID { get; set; }
        public int ModeID { get; set; }

        public virtual Mode Mode { get; set; }
    }
}
