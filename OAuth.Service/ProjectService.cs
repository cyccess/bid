using OAuth.Core.Interfaces;
using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace OAuth.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;


        public ProjectService(IRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Project> GetProjectList()
        {
            var list = _repo.GetAll<Project>().ToList();
            return list;
        }

        public IEnumerable<Project> GetProjectsAndParts()
        {
            var list = _repo.GetAll<Project>().Include(p => p.Roles).ToList(); ;
            return list;
        }


        public void Add(Project entity)
        {
            entity.Uniqueid = Guid.NewGuid();
            entity.AddDate = DateTime.Now;

            _unitOfWork.RegisterNew(entity);
            _unitOfWork.Commit();
        }


        public Project GetProjectById(Guid uniqueid)
        {
            return _repo.GetAll<Project>().FirstOrDefault(p => p.Uniqueid == uniqueid);
        }


        //设置常用项目
        public void Prefs(int pid, int isStar)
        {
            //自动提交
            //using (IRepository r = new EFRepository())
            //{
            //    var entity = r.Entities<Project>().FirstOrDefault(p => p.Id == pid);
            //    entity.IsCommonUsed = entity.IsCommonUsed ? false : true;
            //    entity.AddDate = DateTime.Now;
            //}
        }
    }
}
