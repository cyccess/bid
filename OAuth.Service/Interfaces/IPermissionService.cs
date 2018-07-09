using OAuth.Domain.Model;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service.Interfaces
{
    public interface IPermissionService
    {
        Permission Add(Permission entity, int moduleId);

        Permission GetPermissionById(int id);

        IPagedList<Permission> GetPermissionList(int pageIndex, int projectId);

        void Update(Permission entity);

        void Delate(int id);
    }
}
