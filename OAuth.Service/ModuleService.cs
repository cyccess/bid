using OAuth.Core.Interfaces;
using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OAuth.Service
{
    public class ModuleService : IModuleService
    {

        private readonly IRepository _reposi;
        private readonly IUnitOfWork _unitOfWork;


        public ModuleService(IRepository reposi, IUnitOfWork unitOfWork)
        {
            _reposi = reposi;
            _unitOfWork = unitOfWork;
        }

        public void Add(Module entity)
        {
            if (_reposi.GetAll<Module>().Any(u => u.ModuleName == entity.ModuleName && u.ProjectId == entity.ProjectId))
            {
                throw new ArgumentException("菜单名已存在");
            }

            //设置菜单编号
            entity.ModuleNo = ModuleNo(entity.ProjectId, entity.ParentNo);
            entity.IsParent = entity.ParentNo == "0";
            entity.IsMenu = true;
            entity.AddDate = DateTime.Now;

            _unitOfWork.RegisterNew(entity);
            _unitOfWork.Commit();
        }

        /// <summary>
        /// 模块编号
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="parentNo"></param>
        /// <returns></returns>
        public string ModuleNo(int projectId, string parentNo)
        {
            var moduleNo = _reposi.GetAll<Module>()
                .Where(m => m.ProjectId == projectId && m.ParentNo == parentNo)
                .OrderByDescending(m => m.ModuleNo)
                .Select(m => m.ModuleNo).FirstOrDefault();

            if (string.IsNullOrEmpty(moduleNo))
            {
                //M001101 表示父菜单
                //编号组成格式：单编号 M001101001（001表示项目编号，101表示父序号，001表示子序号）
                moduleNo = parentNo == "0" ? "M" + projectId.ToString("000") + "101" : parentNo + "001";
            }
            else
            {
                int sequence;
                if (parentNo == "0")
                {
                    sequence = int.Parse(moduleNo.Substring(4, 3));
                    sequence += 1;
                    moduleNo = "M" + projectId.ToString("000") + sequence.ToString("000");
                }
                else
                {
                    sequence = int.Parse(moduleNo.Substring(7));
                    sequence += 1;
                    moduleNo = parentNo + sequence.ToString("000");
                }
            }

            return moduleNo;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Module GetModuleById(int id)
        {
            var entity = _reposi.GetById<Module>(id);
            return entity;
        }

        /// <summary>
        /// 模块列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Module> GetModuleList(int projectId, bool isparent = false, string moduleNo = "")
        {
            var moduleQuery = _reposi.GetAll<Module>().Where(m => m.ProjectId == projectId);

            if (isparent)
            {
                moduleQuery = moduleQuery.Where(m => m.IsParent == true);
            }

            if (moduleNo != "")
            {
                moduleQuery = moduleQuery.Where(m => m.ParentNo == moduleNo);
            }

            return moduleQuery.OrderBy(u => u.ModuleNo).ToList();
        }

        /// <summary>
        /// 更新模块
        /// </summary>
        /// <param name="module"></param>
        public void Update(Module module)
        {
            var entity = _reposi.GetById<Module>(module.Id);
            entity.ModuleName = module.ModuleName;
            entity.ModuleUrl = module.ModuleUrl;
            entity.OrderSort = module.OrderSort;
            entity.ModuleIcon = module.ModuleIcon;
            entity.Status = module.Status;

            _unitOfWork.RegisterDirty(entity);
            _unitOfWork.Commit();
        }


        public void Delate(int id)
        {
            if (id == 0)
            {
                return;
            }
            var entity = _reposi.GetById<Module>(id);
            if (entity != null)
            {
                _unitOfWork.RegisterDeleted(entity);
                _unitOfWork.Commit();
            }
        }

        /// <summary>
        /// 获取某个项目下的所有父级模块
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public IEnumerable<ParentModule> GetParentModuleList(int projectId)
        {
            var list = _reposi.GetAll<Module>()
                .Where(m => m.ProjectId == projectId && m.IsParent == true)
                .OrderBy(m => m.ModuleNo)
                .Select(m => new ParentModule
                {
                    ModuleNo = m.ModuleNo,
                    ModuleName = m.ModuleName
                }).ToList();
            return list;
        }

        /// <summary>
        /// 用户获取菜单列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="appid">项目Id</param>
        /// <returns></returns>
        public IEnumerable<Module> GetModuleList(int userId, string appid)
        {
            try
            {
                //获取用户角色对应的权限
                var permission = _reposi.GetAll<UserRole>().Include(o => o.Role.RoleRights)
                    .Where(u => u.UserId == userId)
                    .SelectMany(o => o.Role.RoleRights.Select(r => r.ModuleId)).Distinct().ToArray();

                //获取项目ID
                var projectId = _reposi.GetAll<Project>().Where(p => p.Uniqueid == new Guid(appid)).Select(p => p.Id).FirstOrDefault();

                //根据权限获取对应的模块列表
                var modules = _reposi.GetAll<Module>().OrderBy(m => m.ModuleNo)
                    .Where(m => permission.Contains(m.Id) && m.Status == MenuStatus.Enable && m.ProjectId == projectId).ToList();

                return modules;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 根据模块moduleId ，获取动作列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public IEnumerable<Permission> GetPermissionListByModuleId(int moduleId)
        {
            var actionList = _reposi.GetAll<Module>().Include(m => m.ModulePermissions)
                .Where(m => m.Id == moduleId)
                .SelectMany(m => m.ModulePermissions.Select(mp => mp.Permission)).ToList();

            return actionList;
        }
    }


    public class ParentModule
    {
        public string ModuleNo { get; set; }
        public string ModuleName { get; set; }
    }

}
