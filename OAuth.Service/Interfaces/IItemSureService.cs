using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.Interfaces
{
    public interface IItemSureService
    {
        ItemSure Get(int id);

        void Add(IEnumerable<ItemSure> item);

        void Update(IEnumerable<ItemSure> item);

        void Delete(int id);

        IEnumerable<ItemSure> GetItemSureList(int materialId);
    }
}
