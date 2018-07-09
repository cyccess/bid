using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuth.Web.Controllers
{
    public class ItemMaterialController : BaseController
    {
        // GET: ItemMaterial
        public ActionResult Index()
        {
            return View();
        }
    }
}