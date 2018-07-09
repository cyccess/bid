using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuth.Web.Filters
{
    /// <summary>
    /// 布局视图数据绑定
    /// </summary>
    public class HeaderFooterFilter : ActionFilterAttribute
    {
        public IProjectService ps { get; set; }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var p = ps;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult v = filterContext.Result as ViewResult;
            if (v != null) // v will null when v is not a ViewResult
            {

                v.ViewBag.ProjectList = ps.GetProjectList();
            }
        }

      
    }
}