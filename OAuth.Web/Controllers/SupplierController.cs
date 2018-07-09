using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using OAuth.Service.ModelDto;
using Webdiyer.WebControls.Mvc;
using OAuth.Service.Common;

namespace OAuth.Web.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly ISupplierService _supplierService;
        private readonly IItemService _itemService;
        private readonly IItemMaterialService _itemMaterialService;
        private readonly IModeService _modeService;

        public SupplierController(ISupplierService supplierService, IItemService itemService, IItemMaterialService itemMaterialService, IModeService modeService)
        {
            this._supplierService = supplierService;
            _itemService = itemService;
            _itemMaterialService = itemMaterialService;
            _modeService = modeService;
        }

        // GET: Supplier
        public ActionResult Index(string SupplierName, int id = 1, int ModeId = 0)
        {
            var modes = _modeService.ModeList().Where(m => m.ParentID > 0);
            var list = _supplierService.GetSuppliers(id, ModeId, SupplierName);
            var modeName = modes.Where(m => m.Id == ModeId).Select(m => m.ModeName).SingleOrDefault();
            ViewBag.Modes = modes;
            ViewBag.ModeName = modeName ?? "供应商类别";
            return View(list);
        }

        public ActionResult List(string SupplierName, int id = 1, int ModeId = 0)
        {
            var modes = _modeService.ModeList().Where(m => m.ParentID > 0);
            var list = _supplierService.GetSuppliers(id, ModeId, SupplierName);
            var modeName = modes.Where(m => m.Id == ModeId).Select(m => m.ModeName).SingleOrDefault();
            ViewBag.Modes = modes;
            ViewBag.ModeName = modeName ?? "供应商类别";
            return View(list);
        }

        public ActionResult Detail(int id = 0)
        {
            var model = _supplierService.GetSupplierById(id);
            return View(model);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(SupplierDto model, string supplierModes)
        {
            model.SupplierModeIds = supplierModes.Split(',').Select(int.Parse).ToArray();
            _supplierService.Add(model);
            return Json(new { code = 200, message = "操作完成！", url = "/supplier" });
        }

        [HttpPost]
        public ActionResult CheckEmail(string param)
        {
            return Json(_supplierService.CheckEmail(param) ?
                new { info = "邮箱已存在", status = "n" } :
                new { info = "通过", status = "y" });
        }

        public ActionResult Edit(int id)
        {
            var model = _supplierService.GetSupplierById(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SupplierDto model, string supplierModes)
        {
            model.SupplierModeIds = supplierModes.Split(',').Select(int.Parse).ToArray();
            _supplierService.Update(model);
            return Json(new { code = 200, message = "操作完成！", url = "/supplier" });
        }

        public ActionResult Delete(int id)
        {
            _supplierService.Delate(id);
            return Json(new { message = "操作完成！", code = 200 });
        }

        public ActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(string oldPwd, string newPwd)
        {
            var result = _supplierService.UpdatePassword(UserId, oldPwd, newPwd);
            return Json(result);
        }

        [HttpPost]

        public ActionResult SendActiveEmail(int supplierId)
        {
            _supplierService.EmailNotice(supplierId);
            return Json(new { code = 200, message = "邮件通知发送成功！！" });
        }


        public ActionResult ForgetPassword(int supplierId)
        {
            _supplierService.ForgetPassword(supplierId);
            return Json(new { code = 200, message = "账号信息邮件发送成功！！" });
        }

        //工程竞价列表
        public ActionResult QuoteItemList(int id = 1)
        {
            var list = _itemService.GetQuoteItems(id);
            return View(list);
        }

        //型材竞价
        [HttpPost]
        public ActionResult MaterialQuote(ItemQuoteDto model)
        {
            model.SupplierId = UserId;
            _supplierService.MaterialQuote(model);
            return Json(new { message = "竞价成功！", code = 200 });
        }

        public ActionResult QuoteMaterialList(int tempmode, int id, int page = 1)
        {
            ViewBag.CheckItemExpire = _itemService.CheckItemExpire(id);

            var list = _itemMaterialService.GetQuoteItemMaterials(id, page);
            return View("Material_" + tempmode, list);
        }

        //获取型材报价
        [HttpPost]
        public ActionResult GetItemQuote(int materialId)
        {
            var entity = _itemMaterialService.GetItemQuote(materialId);
            return Json(entity);
        }

        //获取型材报价和中标信息
        [HttpPost]
        public ActionResult GetItemSureQuote(int materialId)
        {
            var entity = _itemMaterialService.GetItemQuote(materialId);
            var sureQuote = _itemMaterialService.GetMaterialSureQuote(materialId);

            return Json(new
            {
                quote = entity,
                quoteDate = entity.InputTime.ToShortDateString(),
                sure = sureQuote,
                sureDate = sureQuote.InputTime.ToShortDateString()
            });
        }
    }
}