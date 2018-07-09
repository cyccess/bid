using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using OAuth.Core.Interfaces;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service
{
    public class PurchaserService : IPurchaserService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public PurchaserService(IRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public void Add(Purchaser entity)
        {
            _unitOfWork.RegisterNew(entity);
            _unitOfWork.Commit();
        }

        public void Detele(int id)
        {
            var entity = _repo.GetById<Purchaser>(id);
            _unitOfWork.RegisterDeleted(entity);
            _unitOfWork.Commit();
        }

        public Purchaser Get(int id)
        {
            return _repo.GetAll<Purchaser>().Where(p => p.Id == id).SingleOrDefault();
        }

        public IEnumerable<Purchaser> GetPurchasers()
        {
            return _repo.GetAll<Purchaser>().OrderByDescending(u => u.PurchaserName);
        }

        public IPagedList<Purchaser> GetPurchasers(int pageIndex)
        {
            return _repo.GetAll<Purchaser>().OrderByDescending(u => u.PurchaserName).ToPagedList(pageIndex, 10);
        }

        public void Upadte(Purchaser entity)
        {
            _unitOfWork.RegisterDirty(entity);
            _unitOfWork.Commit();
        }
    }
}
