using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuth.Web.Controllers
{
    public class OAuthAPIController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IModuleService modulService;
        private readonly IUserService userService;

        public OAuthAPIController(IRoleService roleService, IModuleService modulService, IUserService userService)
        {
            this.roleService = roleService;
            this.modulService = modulService;
            this.userService = userService;
        }

        //检查权限
        public ActionResult CheckPermission(string controller, string action, string userId)
        {
            return Json("");
        }

        //获取菜单列表
        public ActionResult GetModuleList(int userId, string appid)
        {
            var list = modulService.GetModuleList(userId, appid);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //登录
        //oAuthAPI/Login/?username=admin&password=123
        public ActionResult Login(string username, string password)
        {
            var model = userService.Login(username, password);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //获取用户项目
        //oAuthAPI/GetUserProjectList/?userid=1
        public ActionResult GetUserProjectList(int userId)
        {
            var model = userService.GetUserProjectList(userId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        //修改用户密码
        //oAuthAPI/ChangePassword/?userid=1&newPwd=123&oldPwd=321
        public ActionResult ChangePassword(int userId, string newPwd, string oldPwd)
        {
            var result = userService.ChangePassword(userId, newPwd, oldPwd);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}