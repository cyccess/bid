using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using OAuth.Core.Interfaces;
using System.Data.Entity;

namespace OAuth.Service
{
    public class ItemModeService : IItemModeService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public ItemModeService(IRepository repo, IUnitOfWork work)
        {
            this._repo = repo;
            this._unitOfWork = work;
        }

        public IEnumerable<ItemMode> GetItemModes(int itemID)
        {
            return _repo.GetAll<ItemMode>().Include(u => u.Mode).Where(p => p.ItemID == itemID);
        }
    }
}
