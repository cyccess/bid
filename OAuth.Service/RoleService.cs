using OAuth.Core.Interfaces;
using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using OAuth.Service.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAuth.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IRepository repo, IUnitOfWork work)
        {
            this._repo = repo;
            this._unitOfWork = work;
        }

        public void AddRole(Role entity)
        {
            if (_repo.GetAll<Role>().Any(u => u.Name == entity.Name && u.ProjectId == entity.ProjectId))
            {
                throw new ArgumentException("角色已存在");
            }

            entity.AddDate = DateTime.Now;
            entity.Status = 1;
            _unitOfWork.RegisterNew(entity);
            _unitOfWork.Commit();
        }


        public void Delete(int roleId)
        {
            var entity = _repo.GetById<Role>(roleId);
            if (entity != null)
            {
                _unitOfWork.RegisterDeleted(entity);
            }

            var rrList = _repo.GetAll<RoleRight>().Where(r => r.RoleId == roleId).ToList();

            foreach (var item in rrList)
            {
                _unitOfWork.RegisterDeleted(item);
            }

            _unitOfWork.Commit();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<Role> RoleList(int projectId)
        {
            return _repo.GetAll<Role>().Where(r => r.ProjectId == projectId);
        }


        /// <summary>
        /// 获取角色模块
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public RoleDto GetRoleRightById(int projectId, int roleId)
        {
            //获取该角色已有的模块
            var roleDto = _repo.GetAll<Role>().Where(r => r.Id == roleId)
                .Select(r => new RoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleRightDto = r.RoleRights
                    .Select(m => new RoleRightDto
                    {
                        Id = m.Id,
                        RoleId = m.RoleId,
                        ModuleId = m.ModuleId
                    })
                }).FirstOrDefault();
            //获取指定项目下的所有模块信息
            roleDto.Modules = _repo.GetAll<Module>().Where(m => m.ProjectId == projectId).OrderBy(m => m.ModuleNo).ToList();
            return roleDto;
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionArr"></param>
        /// <param name="isChecked"></param>
        public void AddRoleRight(int roleId, int[] permissionArr, bool isChecked)
        {
            var rrList = _repo.GetAll<RoleRight>().Where(rr => rr.RoleId == roleId).ToList();
            foreach (var entity in rrList)
            {
                _unitOfWork.RegisterDeleted(entity);
            }

            if (isChecked)
            {
                foreach (var item in permissionArr)
                {
                    _unitOfWork.RegisterNew(new RoleRight()
                    {
                        RoleId = roleId,
                        ModuleId = item
                    });
                }
            }
            _unitOfWork.Commit();
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionArr"></param>
        /// <param name="isChecked"></param>
        public void SetSingleRoleRight(int roleId, int[] permissionArr, bool isChecked)
        {
            var rrList = _repo.GetAll<RoleRight>().Where(rr => rr.RoleId == roleId && permissionArr.Contains(rr.ModuleId)).ToList();

            //删除该角色包含的指定权限项
            foreach (var entity in rrList)
            {
                _unitOfWork.RegisterDeleted(entity);
            }

            if (isChecked)
            {
                foreach (var item in permissionArr)
                {
                    _unitOfWork.RegisterNew(new RoleRight()
                    {
                        RoleId = roleId,
                        ModuleId = item
                    });
                }
            }
            _unitOfWork.Commit();
        }

        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="partId"></param>
        public void SetPart(int userId, int partId)
        {
            var userPart = _repo.GetAll<UserRole>().FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == partId);
            //如果已拥有该角色，将其删除
            if (userPart != null)
            {
                _unitOfWork.RegisterDeleted(userPart);
            }
            else
            {
                userPart = new UserRole
                {
                    UserId = userId,
                    RoleId = partId,
                    Status = 1
                };
                _unitOfWork.RegisterNew(userPart);
            }
            _unitOfWork.Commit();
        }

        public Role GetRoleById(int roleId)
        {
            return _repo.GetById<Role>(roleId);
        }

        public void Rename(Role role)
        {
            var entity = GetRoleById(role.Id);
            if (entity != null)
            {
                entity.Name = role.Name;
                entity.AddDate = DateTime.Now;

                _unitOfWork.RegisterDirty(entity);
                _unitOfWork.Commit();
            }
        }

        public void Copy(Role role)
        {
            if (_repo.GetAll<Role>().Count(u => u.Name == role.Name && u.ProjectId == role.ProjectId) > 0)
            {
                throw new ArgumentException("角色已存在");
            }

            role.AddDate = DateTime.Now;
            role.Status = 1;

            _unitOfWork.RegisterNew(role);

            //获取被复制角色的权限
            var list = _repo.GetAll<RoleRight>().Where(b => b.RoleId == role.Id).ToList(); ;

            foreach (var item in list)
            {
                item.RoleId = role.Id;
                _unitOfWork.RegisterNew(item);
            }
            _unitOfWork.Commit();
        }
    }
}
