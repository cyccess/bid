using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using Webdiyer.WebControls.Mvc;
using OAuth.Core.Interfaces;
using System.Data.Entity;
using OAuth.Service.ModelDto;
using AutoMapper;

namespace OAuth.Service
{
    public class ItemMaterialService : IItemMaterialService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public ItemMaterialService(IRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public void Add(IEnumerable<ItemMaterial> list)
        {
            list.ToList().ForEach(p => _unitOfWork.RegisterNew(p));
            //使用UnitOfWork方式
            _unitOfWork.Commit();
        }

        public ItemMaterialDto Add(ItemMaterialDto model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("material name is not allowed to be empty");
            }

            var entity = Mapper.Map<ItemMaterialDto, ItemMaterial>(model);
            //使用UnitOfWork方式
            _unitOfWork.RegisterNew(entity);
            _unitOfWork.Commit();
            return model;
        }

        public void Delete(int[] dt)
        {
            foreach (int it in dt)
            {
                var entity = _repo.GetAll<ItemMaterial>().Where(p => p.Id == it).SingleOrDefault();
                if (entity == null)
                {
                    continue;
                }
                var list = _repo.GetAll<ItemQuote>().Where(p => p.ItemMaterialId == it).ToList();
                if (list != null && list.Count != 0)
                {
                    continue;
                }
                list.ForEach(u => _unitOfWork.RegisterDeleted(u));
                _repo.GetAll<ItemSure>().Where(p => p.MaterialID == it).ToList().ForEach(u => _unitOfWork.RegisterDeleted(u));
                _unitOfWork.RegisterDeleted(entity);
            }
            _unitOfWork.Commit();
        }

        public ItemMaterial Get(int id)
        {
            var entity = _repo.GetAll<ItemMaterial>().AsNoTracking().Single(u => u.Id == id);
            return entity;
        }

        public IPagedList<ItemMaterial> GetItemMaterials(int id, int pageIndex)
        {
            var pageList = _repo.GetAll<ItemMaterial>()
                .Where(u => u.ItemID == id)
                .OrderBy(u => u.Name)
                .ToPagedList(pageIndex, 10);
            return pageList;
        }

        public ItemMaterialDto Update(ItemMaterialDto model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("material name is not allowed to be empty");
            }
            int count = _repo.GetAll<ItemQuote>().Where(p => p.ItemMaterialId == model.Id).Count();
            var entity = _repo.GetById<ItemMaterial>(model.Id);
            if (count == 0)
            {
                entity = Mapper.Map(model, entity);
                _unitOfWork.RegisterDirty(entity);
            }
            else
            {
                if (model.Sum < entity.Sum)
                {
                    entity.IsEnabled = false;
                    _unitOfWork.RegisterDirty(entity);
                    model.Id = -1;
                    _unitOfWork.RegisterNew(model);
                }
                else
                {
                    entity = Mapper.Map(model, entity);
                    _unitOfWork.RegisterDirty(entity);
                }
            }
            _unitOfWork.Commit();
            return model;
        }

        public IPagedList<ItemMaterial> GetQuoteItemMaterials(int itemId, int pageIndex)
        {
            var entities = _repo.GetAll<ItemMaterial>()
                .Where(u => u.ItemID == itemId && u.IsEnabled == true)
                .OrderBy(u => u.Name)
                .ToPagedList(pageIndex, 10);

            //获取物料ID数组
            var itemMaterialIdArr = entities.Select(im => im.Id).ToArray();
            //获取物料对应的中标
            var itemSureList = _repo.GetAll<ItemSure>().Where(s => itemMaterialIdArr.Contains(s.MaterialID)).ToList();

            entities.ForEach(im => im.ItemSure = itemSureList.FirstOrDefault(b => b.MaterialID == im.Id));

            return entities;

            //return entities.Select(s => { s.ItemSure = itemSureList.FirstOrDefault(b => b.MaterialID == s.Id); return s; }).ToPagedList(pageIndex,10);

            //var t = (from qim in entities
            //         join s in itemSureList
            //         on qim.Id equals s.MaterialID into list
            //         select new ItemMaterial
            //         {
            //             ItemSure = list.FirstOrDefault()
            //         })
            //         .ToPagedList(pageIndex, 10);

        }

        public ItemQuoteDto GetItemQuote(int materialId)
        {
            var entity = _repo.GetAll<ItemQuote>()
                .SingleOrDefault(iq => iq.ItemMaterialId == materialId && iq.SupplierId == SessionService.SessionInfo.Id);

            return AutoMapper.Mapper.Map<ItemQuote, ItemQuoteDto>(entity);
        }

        public ItemQuoteDto GetMaterialSureQuote(int materialId)
        {
            var itemSure = _repo.GetAll<ItemSure>()
                .SingleOrDefault(iq => iq.MaterialID == materialId);

            if (itemSure != null)
            {
                var entity = _repo.GetAll<ItemQuote>()
                   .SingleOrDefault(iq => iq.ItemMaterialId == itemSure.MaterialID && iq.Id == itemSure.QuoteID);
                entity.Memo = itemSure.Memo;
                return AutoMapper.Mapper.Map<ItemQuote, ItemQuoteDto>(entity);
            }

            return new ItemQuoteDto();
        }
    }
}
