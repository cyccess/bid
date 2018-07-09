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
    public class ModeService : IModeService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public ModeService(IRepository repo, IUnitOfWork work)
        {
            this._repo = repo;
            this._unitOfWork = work;
        }

        public void Add(Mode entity)
        {
            if (string.IsNullOrEmpty(entity.ModeName))
            {
                throw new ArgumentException("names are not allowed to be empty");
            }
            _unitOfWork.RegisterNew(entity);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var entity = _repo.GetAll<Mode>().Where(p => p.Id == id).SingleOrDefault();
            entity.IsEnabled = entity.IsEnabled == true ? false : true;
            _unitOfWork.RegisterDirty(entity);
            if (entity.ParentID==-1){
                _repo.GetAll<Mode>().Where(p => p.ParentID == entity.Id).ToList().ForEach(u => { u.IsEnabled = u.IsEnabled == true ? false : true; _unitOfWork.RegisterDirty(u); });
            }
            _unitOfWork.Commit();
        }

        public void Update(Mode entity)
        {
            if (string.IsNullOrEmpty(entity.ModeName))
            {
                throw new ArgumentException("names are not allowed to be empty");
            }
            _unitOfWork.RegisterDirty(entity);
            _unitOfWork.Commit();
        }

        public IPagedList<Mode> GetChildrenModes(int parentID, int pageIndex)
        {
            var items = _repo.GetAll<Mode>()
                .Where(p => p.ParentID == parentID)
                .OrderByDescending(u => u.ModeName)
                .ToPagedList(pageIndex, 10);

            return items;
        }

        public Mode GetModeById(int modeId)
        {
            return _repo.GetAll<Mode>().Where(p => p.Id == modeId).SingleOrDefault();
        }

        public IPagedList<Mode> GetParentModes(int pageIndex)
        {
            var items = _repo.GetAll<Mode>()
                .Where(p=>p.ParentID==-1)
                .OrderByDescending(u => u.ModeName)
                .ToPagedList(pageIndex, 10);

            return items;
        }

        public IEnumerable<Mode> ModeList()
        {
            return _repo.GetAll<Mode>().ToList();
        }
    }
}
