using System;
using System.Threading.Tasks;
using OAuth.Service.Interfaces;
using OAuth.Web.Models;
using System.Web.Mvc;
using OAuth.Service.ModelDto;
using OAuth.Service.Common;

namespace OAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ProjectInfo _projectInfo;
        private readonly ICacheManager _cache;
        private readonly IAccountService _accountService;

        public HomeController(IUserService userService, ProjectInfo projectInfo, ICacheManager cache, IAccountService accountService)
        {
            _userService = userService;
            _projectInfo = projectInfo;
            _cache = cache;
            _accountService = accountService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password, UserType userType = UserType.Supplier)
        {
            var userInfo = _accountService.Login(userName, password, userType);

            if (userInfo == null || userInfo.Id <= 0) return Json(new { code = 5, message = "用户名或密码错误" });

            if (userInfo.IsEnabled==false)
            {
                return Json(new { code = 7, message = "此帐户已被禁用，请联系系统管理员" });
            }

            var url = userType == UserType.Supplier ? "/Supplier/QuoteItemList" : "/Subject";
            return Json(new { code = 200, url });
        }

        public ActionResult SignOut()
        {
            _accountService.SignOut();
            return RedirectToAction("Login", "Home");
        }


        public ActionResult SupplierActive(string supplierId)
        {
            ViewBag.SupplierId = supplierId;
            var model = _accountService.SupplierActive(supplierId);
            return View(model);
        }

        [HttpPost]
        public ActionResult SupplierModifyPwd(string supplierId, string oldPwd, string newPwd)
        {
            ViewBag.SupplierId = supplierId;
            var model = _accountService.SupplierModifyPwd(supplierId, oldPwd, newPwd);
            return Json(model);
        }


        [ChildActionOnly]//防止直接调用
        public ActionResult ViewMenu()
        {
            return PartialView();
        }


        public ActionResult TestSqlQuery()
        {
            var model = _userService.Get(1);
            return View(model);
        }

        public ActionResult TestEncrypt(string str = "")
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = EncryptHelper.Encrypt(str);
            }
            ViewBag.Str = str;
            return View();
        }

        public ActionResult TestDecrypt(string str = "")
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = EncryptHelper.Decrypt(str);
            }
            ViewBag.Str = str;
            return View();
        }
    }
}