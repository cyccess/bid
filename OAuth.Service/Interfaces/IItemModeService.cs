using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.Interfaces
{
    public interface IItemModeService
    {
        IEnumerable<ItemMode> GetItemModes(int ItemID);
    }
}
