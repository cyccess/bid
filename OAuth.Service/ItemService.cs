using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using OAuth.Core.Interfaces;
using System.Data.Entity;
using Webdiyer.WebControls.Mvc;
using OAuth.Domain.IRepository;

namespace OAuth.Service
{
    public class ItemService : IItemService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReportRepository _reportRepository;

        public ItemService(IRepository repo, IUnitOfWork unitOfWork, IReportRepository reportRepository)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _reportRepository = reportRepository;
        }

        public Item Add(Item item, IEnumerable<ItemMode> list)
        {
            if (string.IsNullOrEmpty(item.ItemName))
            {
                throw new ArgumentException("project name is not allowed to be empty");
            }
            //使用UnitOfWork方式
            _unitOfWork.RegisterNew(item);
            _unitOfWork.Commit();
            foreach (ItemMode it in list)
            {
                it.ItemID = item.Id;
                _unitOfWork.RegisterNew(it);
            }
            _unitOfWork.Commit();
            return item;
        }

        public Item Update(Item item, IEnumerable<ItemMode> list)
        {
            if (string.IsNullOrEmpty(item.ItemName))
            {
                throw new ArgumentException("project name is not allowed to be empty");
            }
            var entity = _repo.GetAll<Item>().Include(p => p.ItemModes).Single(u => u.Id == item.Id);
            entity.ItemName = item.ItemName;
            //entity.StartDate = item.StartDate;
            //entity.EndDate = item.EndDate;
            entity.ItemNo = item.ItemNo;
            entity.Memo = item.Memo;
            entity.SignTime = item.SignTime;
            entity.SignPlace = item.SignPlace;
            _unitOfWork.RegisterDirty(entity);

            var lt = (from p in entity.ItemModes join l in list on p.ModeID equals l.ModeID into dt from it in dt.DefaultIfEmpty() where it == null select p).ToList();
            lt.ForEach(p => _unitOfWork.RegisterDeleted(p));

            var li = (from p in list join l in entity.ItemModes on p.ModeID equals l.ModeID into dt from it in dt.DefaultIfEmpty() where it == null select p).ToList();
            li.ForEach(p => { p.ItemID = entity.Id; _unitOfWork.RegisterNew(p); });

            _unitOfWork.Commit();
            return entity;
        }

        public Item Get(int id)
        {
            var entity = _repo
                .GetAll<Item>().Single(u => u.Id == id);
            return entity;
        }

        public IPagedList<Item> GetItems(string ItemName, string Contractor, string SignPlace, int pageIndex)
        {
            int userId = SessionService.SessionInfo.Id;
            var entities = _repo.GetAll<Item>()
                .Include(u => u.User)
                .Where(u => userId != 1 ? u.InputPerson == userId : 1 == 1);

            if (!string.IsNullOrEmpty(ItemName))
            {
                entities = entities.Where(item => item.ItemName.Contains(ItemName));
            }
            if (!string.IsNullOrEmpty(Contractor))
            {
                entities = entities.Where(item => item.User.FullName.Contains(Contractor));
            }
            if (!string.IsNullOrEmpty(SignPlace))
            {
                entities = entities.Where(item => item.SignPlace.Contains(SignPlace));
            }

            return entities.OrderByDescending(u => u.Id).ToPagedList(pageIndex, 10); 
        }

        //public Item EmailNotice(Item item)
        //{
        //    if (string.IsNullOrEmpty(item.ItemName))
        //    {
        //        throw new ArgumentException("project name is not allowed to be empty");
        //    }
        //    var entity = _repo.GetAll<Item>().Include(p => p.ItemModes).Single(u => u.Id == item.Id);
        //    entity.IsNotice = true;
        //    _unitOfWork.RegisterDirty(entity);

        //    _unitOfWork.Commit();
        //    return entity;
        //}

        public IEnumerable<Supplier> GetItemSuppliers(int itemId)
        {
            var im = _repo.GetAll<ItemMode>().Where(p => p.ItemID == itemId);
            var sm = _repo.GetAll<SupplierMode>().Where(p => im.Select(v => v.ModeID).ToList().Contains(p.ModeId)).Select(b => b.SupplierId);
            var lt = _repo.GetAll<Supplier>().Where(p => sm.Contains(p.Id));
            return lt;
        }

        public void Delete(int[] dt)
        {
            foreach (int it in dt)
            {
                var list = _repo.GetAll<ItemMaterial>().Where(p => p.ItemID == it).AsEnumerable();
                var lt = _repo.GetAll<ItemQuote>().Where(p => list.Select(u => u.Id).Contains(p.ItemMaterialId)).ToList();
                if (lt.Count() == 0)
                {
                    lt.ForEach(u => _unitOfWork.RegisterDeleted(u));
                    _repo.GetAll<ItemMode>().Where(p => p.ItemID == it).ToList().ForEach(u => _unitOfWork.RegisterDeleted(u));
                    _repo.GetAll<ItemSure>().Where(p => list.Select(u => u.Id).Contains(p.MaterialID)).ToList().ForEach(u => _unitOfWork.RegisterDeleted(u));
                    list.ToList().ForEach(p => _unitOfWork.RegisterDeleted(p));
                    _unitOfWork.RegisterDeleted(_repo.GetById<Item>(it));
                }
            }
            _unitOfWork.Commit();
        }


        /// <summary>
        /// 获取待竞价项目列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IPagedList<Item> GetQuoteItems(int pageIndex)
        {
            int userId = SessionService.SessionInfo.Id;

            //获取当前供应商类型
            var modelIdArr = _repo.GetAll<SupplierMode>().Where(sm => sm.SupplierId == userId).Select(sm => sm.ModeId).ToArray();

            var items = _repo.GetAll<Item>()
                .Include(item => item.ItemModes)
                .Where(item => item.ItemModes.Any(im => modelIdArr.Contains(im.ModeID)))
                .Where(item => item.StartDate < DateTime.Now)
                .OrderByDescending(u => u.Id)
                .ToPagedList(pageIndex, 10);

            return items;
        }

        public IEnumerable<ItemContrast> GetContrastPrice(int id)
        {
            return _reportRepository.GetContrastPrice(id);
        }

        public IEnumerable<Contract> GetContract(int id, int spId)
        {
            return _reportRepository.GetContract(id, spId);
        }

        public Item Update(Item item)
        {
            if (string.IsNullOrEmpty(item.ItemName))
            {
                throw new ArgumentException("project name is not allowed to be empty");
            }
            var entity = _repo.GetAll<Item>().Include(p => p.ItemModes).Single(u => u.Id == item.Id);
            entity.ItemName = item.ItemName;
            entity.StartDate = item.StartDate;
            entity.EndDate = item.EndDate;
            entity.ItemNo = item.ItemNo;
            entity.Memo = item.Memo;
            entity.SignTime = item.SignTime;
            entity.SignPlace = item.SignPlace;
            entity.IsNotice = item.IsNotice;
            _unitOfWork.RegisterDirty(entity);
            _unitOfWork.Commit();
            return entity;
        }

        public bool CheckItemExpire(int itemId)
        {
            var nowDate = DateTime.Now;

            var result = _repo.GetAll<Item>()
                .Any(item => item.StartDate <= nowDate && item.EndDate >= nowDate && item.Id == itemId);

            return result;
        }

        public int GetItemCount()
        {
            return _repo.GetAll<Item>().Count();
        }
    }
}
