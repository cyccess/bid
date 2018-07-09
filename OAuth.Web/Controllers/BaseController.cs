using OAuth.Web.Filters;
using System.Web.Mvc;
using OAuth.Service;
using OAuth.Service.ModelDto;

namespace OAuth.Web.Controllers
{
    [LoginActionFilter]
    public class BaseController : Controller
    {
        /// <summary>
        /// 登录用户信息
        /// </summary>
        protected UserInfo SessionInfo => SessionService.SessionInfo;

        /// <summary>
        /// 登录用户Id
        /// </summary>
        protected int UserId => SessionInfo.Id;


        //ajax 错误过滤
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !Request.IsAjaxRequest() || !(filterContext.Exception is HttpAntiForgeryException))
            {
                base.OnException(filterContext);
                return;
            }

            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult
            {
                Data = new { Code = 400, message = "非法请求，拒绝访问！" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}