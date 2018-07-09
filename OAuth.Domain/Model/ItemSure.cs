using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class ItemSure : AggregateRoot
    {
        public int MaterialID { get; set; }
        public int QuoteID { get; set; }
        public int InputPerson { get; set; }
        public DateTime InputTime { get; set; }
        public string Memo { get; set; }

    }
}
