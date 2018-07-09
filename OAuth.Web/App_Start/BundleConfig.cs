using System.Web;
using System.Web.Optimization;

namespace OAuth.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
               "~/assets/js/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/assets/js/bootstrap.js",
                      "~/assets/js/respond.js",
                      "~/assets/js/docs.min.js",
                      "~/assets/js/ie10-viewport-bug-workaround.js"));

            bundles.Add(new StyleBundle("~/assets/css").Include(
                      "~/assets/css/bootstrap.css",
                      "~/assets/css/dashboard.css",
                      "~/assets/css/iconfont.css",
                      "~/assets/css/jquery.noty.css",
                      "~/assets/css/noty_theme_default.css",
                      "~/assets/css/base_inner.css"));


            // 将 EnableOptimizations 设为 false 以进行调试。有关详细信息，
            // 请访问 http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}
