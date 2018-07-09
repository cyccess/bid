using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using OAuth.Core.Interfaces;

namespace OAuth.Service
{
    public class ItemSureService : IItemSureService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public ItemSureService(IRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public void Add(IEnumerable<ItemSure> item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ItemSure Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItemSure> GetItemSureList(int materialId)
        {
            throw new NotImplementedException();
        }

        public void Update(IEnumerable<ItemSure> item)
        {
            item.ToList().ForEach(p => _repo.GetAll<ItemSure>().Where(v => v.MaterialID == p.MaterialID).ToList().ForEach(m => _unitOfWork.RegisterDeleted(m)));
            //item.ToList().ForEach(p => _repo.GetAll<ItemQuote>().Where(v => v.ItemMaterialId == p.MaterialID && v.Id == p.QuoteID).ToList().ForEach(m => _unitOfWork.RegisterDeleted(m)));
            _unitOfWork.Commit();
            item.ToList().ForEach(p => _unitOfWork.RegisterNew(p));
            _unitOfWork.Commit();
        }
    }
}
