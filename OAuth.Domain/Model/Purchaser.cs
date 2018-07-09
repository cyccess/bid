using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class Purchaser :AggregateRoot
    {
        public string PurchaserName { get; set; }

        public string Address { get; set; }

        public string LegalPerson { get; set; }

        public string EntrustPerson { get; set; }

        public string Fax { get; set; }

        public string BankNo { get; set; }

        public string BankName { get; set; }
    }
}
