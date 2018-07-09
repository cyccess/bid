using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAuth.Web.Controllers
{
    public class PurchaserController : BaseController
    {
        private readonly IPurchaserService _purchaserService;

        public PurchaserController(IPurchaserService purchaserService)
        {
            this._purchaserService = purchaserService;
        }

        // GET: Purchaser
        public ActionResult Index(int page = 1)
        {
            var list = _purchaserService.GetPurchasers(page);
            return View(list);
        }

        public ActionResult List(int page = 1)
        {
            var list = _purchaserService.GetPurchasers(page);
            return View(list);
        }

        public ActionResult Edit(int id = -1)
        {
            Purchaser entity = null;
            if (id == -1)
            {
                entity = new Purchaser();
                entity.Id = -1;
            }
            else
            {
                entity = _purchaserService.Get(id);
            }
            return View(entity);
        }

        [HttpPost]
        public ActionResult Edit(Purchaser entity)
        {
            if (entity.Id == -1)
            {
                _purchaserService.Add(entity);
            }
            else {
                _purchaserService.Upadte(entity);
            }
            return Json(new { code = 200, message = "保存成功！", url = "/Purchaser" });
        }

        public ActionResult Delete(int id = -1)
        {
            _purchaserService.Detele(id);
            return Json(new { code = 200, message = "删除成功！" });
        }

        public ActionResult Get(int id = -1)
        {
            var entity = _purchaserService.Get(id);
            if (entity == null)
            {
                return null;
            }
            return Json(new { id = entity.PurchaserName, ads = entity.Address, lp = entity.LegalPerson, ep = entity.EntrustPerson, fax = entity.Fax, be = entity.BankName, bn = entity.BankNo});
        }


        public ActionResult Detail(int id = 0)
        {
            var entity = _purchaserService.Get(id);
            return View(entity);
        }
    }
}