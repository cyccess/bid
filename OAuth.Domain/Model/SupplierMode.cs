using System;
using System.Collections.Generic;

namespace OAuth.Domain.Model
{ 
    public  class SupplierMode :AggregateRoot
    {
        public int SupplierId { get; set; }

        public int ModeId { get; set; }


        public virtual Mode Mode { get; set; }
    }
}
