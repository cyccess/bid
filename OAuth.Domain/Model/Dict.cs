using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.Model
{
    public class Dict :AggregateRoot
    {
        public string DictName { get; set; }

        public string DictKey { get; set; }

        public string DictValue { get; set; }

        public int Sort { get; set; }

        public string Memo { get; set; }
    }
}
