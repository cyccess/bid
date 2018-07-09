using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using OAuth.Service.Common;
using OAuth.Service.ModelDto;
using System.Web;

namespace OAuth.Service
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly ISupplierService _supplierService;
        private readonly IModuleService _modulService;


        public AccountService(IUserService userService, ISupplierService supplierService, IModuleService modulService)
        {
            _userService = userService;
            _supplierService = supplierService;
            _modulService = modulService;
        }

        public UserInfo Login(string username, string password, UserType userType)
        {
            UserInfo userinfo;

            if (userType == UserType.Supplier)
            {
                userinfo = _supplierService.Login(username, password);
            }
            else
            {
                userinfo = _userService.Login(username, password);
            }

            var session = SessionService.SessionInfo;

            session.Id = userinfo.Id;
            session.UserName = userinfo.UserName;
            session.UserType = userType;
            session.Module = GetModule(userinfo.Id);

            SessionService.Save();
            return userinfo;
        }

        /// <summary>
        /// 获取权限菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private IEnumerable<Module> GetModule(int userId)
        {
            string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            var list = _modulService.GetModuleList(userId, appid);
            return list;
        }


        public void SignOut()
        {
            SessionService.Clear();
        }

        public ResultModel SupplierActive(string supplierId)
        {
            int id = 0;
            string str = EncryptHelper.Decrypt(supplierId);
            int.TryParse(str, out id);

            var entity = _supplierService.Get(id);

            if (entity == null)
                return new ResultModel(10003, "账户不存在，请联系系统管理员");

            if (entity.IsActivated)
                return new ResultModel(10004, "此账户已激活");

            var nowTime = DateTime.Now;
            if (nowTime - entity.NoticeTime > new TimeSpan(24, 0, 0))
                return new ResultModel(10005, "激活链接失效，请联系系统管理员");

            //激活账户
            _supplierService.SupplierActive(entity);
            return new ResultModel(10000, "恭喜您，账户已激活。为了您的账号信息安全，建议您及时修改登录密码。");
        }

        public ResultModel SupplierModifyPwd(string supplierId, string oldPwd, string newPwd)
        {
            supplierId = HttpUtility.UrlDecode(supplierId);
            int id = 0;
            string str = EncryptHelper.Decrypt(supplierId);
            int.TryParse(str, out id);

            return _supplierService.UpdatePassword(id, oldPwd, newPwd);
        }
    }
}
