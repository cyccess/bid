using OAuth.Domain.Model;
using System.Collections.Generic;

namespace OAuth.Service.Interfaces
{
    public interface IModuleService
    {
        void Add(Module entity);

        Module GetModuleById(int id);

        IEnumerable<Module> GetModuleList(int projectId, bool isparent = false, string moduleNo = "");

        void Update(Module entity);

        void Delate(int id);

        IEnumerable<ParentModule> GetParentModuleList(int projectId);

        IEnumerable<Module> GetModuleList(int userId, string appid);

        IEnumerable<Permission> GetPermissionListByModuleId(int moduleId);
    }
}
