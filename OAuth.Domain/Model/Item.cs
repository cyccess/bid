using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class Item : AggregateRoot
    {
        public string ItemName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ItemNo { get; set; }

        public int TempMode { get; set; }

        public int InputPerson { get; set; }

        public DateTime InputTime { get; set; }

        public string Memo { get; set; }

        public bool IsNotice { get; set; }

        public DateTime SignTime { get; set; }

        public string SignPlace { get; set; }

        public bool IsUpdate { get; set; }

        public virtual ICollection<ItemMode> ItemModes { get; set; }

        public virtual User User { get; set; }
    }
}
