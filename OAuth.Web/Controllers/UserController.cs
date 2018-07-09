using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using OAuth.Service.ModelDto;
using OAuth.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        //
        // GET: /User/
        public ActionResult Index(int id = 1)
        {
            var pageList = userService.GetUsers(id);
            return View(pageList);
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(User user)
        {
            userService.Add(user);
            return Json(new { code = 200, message = "用户添加成功！" });
        }

        public ActionResult Edit(int id = 0)
        {
            var entity = userService.GetUesrById(id);
            return View(entity);
        }

        [HttpPost]
        public ActionResult Edit(UserDto entity)
        {
            userService.Update(entity);
            return Json(new { code = 200, message = "修改成功！" });
        }

        [HttpPost]
        public ActionResult Delete(int uid = 0)
        {
            userService.Delate(uid);
            return Json(new { code = 200, message = "删除成功！" });
        }


        [HttpPost]
        public ActionResult GetUser(int uid = 0)
        {
            var entity = userService.GetUesr(uid);
            return Json(entity);
        }

        [HttpPost]
        public ActionResult ResetPassword(int uid, string password)
        {
            userService.ResetPassword(uid, password);
            return Json(new { code = 200, message = "密码重置成功！" });
        }


        public ActionResult UpdatePassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UpdatePassword(string oldPwd, string newPwd)
        {
            var model = userService.ChangePassword(UserId, newPwd, oldPwd);
            return Json(model);
        }


        [HttpPost]
        public ActionResult CheckOldPassword(string param)
        {
            var result = userService.CheckOldPassword(UserId, param);
            return Json(new { status = result ? "y" : "n", info = result ? "通过" : "旧密码验证错误！" });
        }


        [HttpPost]
        public ActionResult SetUserProject(int uid, int pid)
        {
            userService.SetUserProject(uid, pid);
            return Json(new { code = 200, message = "设置成功" });
        }
    }
}