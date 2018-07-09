using OAuth.Domain.Model;
using OAuth.Service.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service.Interfaces
{
    public interface IUserService
    {
        User Get(int id);

        User Add(User user);


        UserDto GetUesrById(int id);


        IPagedList<User> GetUsers(int pageIndex);


        void Update(UserDto user);

        void Delate(int id);

        UserDto Login(string username, string password);

        IEnumerable<Project> GetUserProjectList(int userId);


        User GetUesr(int id);


        void ResetPassword(int uid, string password);

        bool CheckOldPassword(int uid, string oldPwd);


        void SetUserProject(int userId, int projectId);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="oldPwd">旧密码</param>
        ResultModel ChangePassword(int uid, string newPwd, string oldPwd);
    }
}
