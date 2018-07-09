using OAuth.Service;
using OAuth.Service.Common;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OAuth.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            EndRequest += MvcApplication_EndRequest;
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Autofac初始化
            AutofacConfig.ConfigureContainer();

            //Automapper
            RegisterAutomapper.Initialize();
        }

        private void MvcApplication_EndRequest(object sender, EventArgs e)
        {
            //请求结束销毁用户会话
            SessionService.Destroy();
        }
    }
}
