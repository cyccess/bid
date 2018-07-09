using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.Interfaces
{
    public interface IDictService
    {
        IEnumerable<Dict> GetDicts(string name);

        Dict Get(string name,string id);
    }
}
