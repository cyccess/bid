using OAuth.Core.Interfaces;
using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository _reposi;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IRepository reposi, IUnitOfWork unitOfWork)
        {
            _reposi = reposi;
            _unitOfWork = unitOfWork;
        }

        public Permission Add(Permission entity, int moduleId)
        {
            entity.IsVisible = true;
            _unitOfWork.RegisterNew(entity);

            var modulePermission = new ModulePermission
            {
                ModuleId = moduleId,
                PermissionId = entity.Id
            };
            _unitOfWork.RegisterNew(modulePermission);
            _unitOfWork.Commit();
            return entity;
        }

        public Permission GetPermissionById(int id)
        {
            var entity = _reposi.GetById<Permission>(id);
            return entity;
        }

        public IPagedList<Permission> GetPermissionList(int pageIndex, int projectId)
        {
            var pageList = _reposi.GetAll<Module>().Include(m => m.ModulePermissions)
                .Where(u => u.ProjectId == projectId)
                .SelectMany(m => m.ModulePermissions.Select(mp => mp.Permission))
                .OrderBy(p => p.Id)
                .ToPagedList(pageIndex, 100);
            return pageList;
        }

        public void Update(Permission entity)
        {
            var oldEntity = _reposi.GetById<Permission>(entity.Id);

            oldEntity.PermissionAction = entity.PermissionAction;
            oldEntity.PermissionController = entity.PermissionController;
            oldEntity.PermissionName = entity.PermissionName;
            oldEntity.IsVisible = entity.IsVisible;
            oldEntity.Description = entity.Description;

            _unitOfWork.Commit();
        }

        public void Delate(int id)
        {
            if (id == 0)
            {
                // ReSharper disable once ObjectCreationAsStatement
                new ArgumentException("传入的Id错误");
            }

            var modulePermission = _reposi.GetAll<ModulePermission>().FirstOrDefault(mp => mp.PermissionId == id);
            var entity = _reposi.GetById<Permission>(id);

            _unitOfWork.RegisterDeleted(modulePermission);
            _unitOfWork.RegisterDeleted(entity);
            _unitOfWork.Commit();
        }
    }
}
