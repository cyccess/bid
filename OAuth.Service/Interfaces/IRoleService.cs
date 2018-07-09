using OAuth.Domain.Model;
using OAuth.Service.ModelDto;
using System.Collections.Generic;

namespace OAuth.Service.Interfaces
{
    public interface IRoleService
    {
        void AddRole(Role entity);

        void Delete(int id);

        void Rename(Role entity);

        void Copy(Role entity);

        IEnumerable<Role> RoleList(int projectId);

        Role GetRoleById(int roleId);

        RoleDto GetRoleRightById(int projectId, int roleId);

        void AddRoleRight(int roleId, int[] permissionArr, bool isChecked);

        void SetSingleRoleRight(int roleId, int[] permissionArr, bool isChecked);

        void SetPart(int userId, int partId);
    }
}
