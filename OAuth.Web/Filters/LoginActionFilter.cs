using OAuth.Service.Interfaces;
using OAuth.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using OAuth.Service;
using OAuth.Service.ModelDto;

namespace OAuth.Web.Filters
{
    public class LoginActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = SessionService.SessionInfo;

            if (session.Id == 0)
            {
                string path = filterContext.HttpContext.Request.Path;
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}