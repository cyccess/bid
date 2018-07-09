using OAuth.Domain.Model;
using OAuth.Service.Common;
using OAuth.Service.Interfaces;
using OAuth.Service.ModelDto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Web.Controllers
{
    public class SubjectController : BaseController
    {
        private readonly IItemService _itemService;
        private readonly IModeService _modeService;
        private readonly IDictService _dictService;
        private readonly IItemModeService _itemModeService;
        private readonly IItemMaterialService _itemMaterialService;
        private readonly IItemSureService _itemSureService;
        private readonly ISupplierService _supplierService;
        private readonly IPurchaserService _purchaserService;

        public SubjectController(IItemService itemService, IModeService modeService, IDictService dictService, IItemModeService itemModeService, IItemMaterialService itemMaterialService, IItemSureService itemSureService, ISupplierService supplierService, IPurchaserService purchaserService)
        {
            this._itemService = itemService;
            this._modeService = modeService;
            this._dictService = dictService;
            this._itemModeService = itemModeService;
            this._itemMaterialService = itemMaterialService;
            this._itemSureService = itemSureService;
            this._supplierService = supplierService;
            this._purchaserService = purchaserService;
        }

        // GET: Subject
        public ActionResult Index(string ItemName, string Contractor, string SignPlace, int id = 1)
        {
            var pageList = _itemService.GetItems(ItemName, Contractor, SignPlace, id);
            return View(pageList);
        }

        public ActionResult Step1(int id = -1)
        {
            Item item = null;
            ViewBag.TempMode = _dictService.GetDicts("temp");
            if (id != -1)
            {
                item = _itemService.Get(id);
                IEnumerable<ItemMode> list = _itemModeService.GetItemModes(id);
                ViewBag.ModeID = string.Join(",", list.Select(p => p.ModeID));
                ViewBag.ModeName = string.Join(",", list.Select(p => p.Mode.ModeName));
                return View(item);
            }
            item = new Item()
            {
                Id = -1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                SignTime = DateTime.Now
            };
            ViewBag.ModeID = "";
            ViewBag.ModeName = "";
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Step1(Item item, string ModeID)
        {
            IList<ItemMode> lt = new List<ItemMode>();
            string[] st = ModeID.Split(',');
            st.ToList().ForEach(p => lt.Add(new ItemMode() { ModeID = Convert.ToInt32(p) }));

            if (item.Id == -1)
            {
                //判断
                if ((Convert.ToDateTime("2016-07-25") - DateTime.Now).Days <= 0)
                {
                    return Json(new { code = 500, message = "对不起，软件使用期限即将到期，请联系管理员注册！", id = item.Id });
                }
                item.InputPerson = UserId;
                item.InputTime = DateTime.Now;
                item.IsUpdate = false;
                _itemService.Add(item, lt.ToList());
            }
            else
            {
                item = _itemService.Update(item, lt.ToList());
            }
            return Json(new { code = 200, message = "数据保存成功！", id = item.Id });
        }

        public ActionResult RemoveItem(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { code = 200, message = "未选择任何删除记录！" });
            }
            _itemService.Delete(Array.ConvertAll<string, int>(id.Split(','), p => int.Parse(p)));
            return this.Json(new { code = 200, message = "操作执行成功！" });
        }

        public ActionResult GetMaterial(int id = -1)
        {
            if (id == -1)
            {
                ItemMaterial model = new ItemMaterial() { Id = -1 };
                return Json(model);
            }
            var entity = _itemMaterialService.Get(id);
            return Json(entity);
        }

        public ActionResult RemoveMaterial(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { code = 200, message = "未选择任何删除记录！" });
            }
            _itemMaterialService.Delete(Array.ConvertAll<string, int>(id.Split(','), p => int.Parse(p)));
            return this.Json(new { code = 200, message = "操作执行成功！" });
        }

        [HttpPost]
        public ActionResult EditMaterial(ItemMaterialDto entity)
        {
            var item = _itemService.Get(entity.ItemID);
            if (entity.Id == -1)
            {
                if (item.IsNotice)
                {
                    item.IsNotice = false;
                    item.IsUpdate = true;
                    _itemService.Update(item);
                }
                entity.IsEnabled = true;
                _itemMaterialService.Add(entity);
                return Json(new { code = 200, message = "添加成功！" });
            }
            _itemMaterialService.Update(entity);
            item.IsNotice = false;
            item.IsUpdate = true;
            _itemService.Update(item);
            return Json(new { code = 200, message = "修改成功！" });
        }

        public ActionResult Upload(HttpPostedFileBase Filedata, string id)
        {
            // 如果没有上传文件
            if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }

            string suffix = Path.GetExtension(Filedata.FileName);
            if (suffix != ".xlsx" && suffix != ".xls")
            {
                return this.Json(new { code = 500, message = "模板文件格式错误" });
            }
            Random random = new Random();
            string name = string.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), random.Next(10000, 99999), suffix);
            string virtualPath = string.Format("~/UploadFile/excel/{0}", name);
            string path = this.Server.MapPath(virtualPath);
            Filedata.SaveAs(path);

            var model = _itemService.Get(Convert.ToInt32(id));
            var entity = _dictService.Get("temp", Convert.ToString(model.TempMode));
            ImportService import = new ImportService();
            string message = null;
            IEnumerable<ItemMaterial> list = import.ImportTemplet(path, model, entity.DictValue, out message);
            if (list != null)
            {
                _itemMaterialService.Add(list);
                if (model.IsNotice)
                {
                    model.IsNotice = false;
                    model.IsUpdate = true;
                    _itemService.Update(model);
                }
                return this.Json(new { code = 200, message = "模板文件上传成功" });
            }
            else
            {
                return this.Json(new { code = 200, message = message });
            }
        }

        public ActionResult Step2(int id = -1, int page = -1)
        {
            if (id == -1)
            {
                return Redirect("/subject/index");
            }
            var entity = _itemService.Get(id);
            ViewBag.entity = entity;
            var list = _itemMaterialService.GetItemMaterials(id, page);
            return View(string.Format("step2_{0}", entity.TempMode), list);
        }

        public ActionResult Match(int id = -1)
        {
            if (id == -1)
            {
                return Redirect("/subject/index");
            }
            var entity = _itemService.Get(id);
            ViewBag.Item = entity;
            IEnumerable<ItemContrast> lt = _itemService.GetContrastPrice(id);
            if (entity.TempMode == 2)
            {
                return View("step3_2", lt);
            }
            else
            {
                return View("step3_1", lt);
            }
        }

        public ActionResult Contract(int id = -1, int it = -1)
        {
            if (id == -1)
            {
                return Redirect("/subject/index");
            }
            var entity = _supplierService.GetItemQuote(it);
            var model = _supplierService.Get(entity.SupplierId);
            var im = _itemService.Get(id);
            string contractNo = $"{im.ItemNo}-{model.Id.ToString().PadLeft(5, '0')}";
            ViewBag.Supplier = model;
            im.ItemNo = contractNo;
            ViewBag.Item = im;
            IEnumerable<Contract> lt = _itemService.GetContract(id, entity.SupplierId);
            ViewBag.list = _purchaserService.GetPurchasers();
            if (im.TempMode==2) {
                return View("step4_2", lt);
            } else {
                return View("step4_1", lt);
            }
        }

        [ValidateInput(false)]
        public ActionResult DownloadContract(int id = -1, int it = -1, string norm = null, string weight = null, string mode = null, string freight = null, string client = null, string place = null, string date = null, string data = null, List<string[]> lt = null)
        {
            PdfService pdfService = new PdfService();
            //var entity = _supplierService.GetItemQuote(it);
            var model = _supplierService.Get(it);
            var item = _itemService.Get(id);
            var entity = _purchaserService.Get(Convert.ToInt32(client));
            IEnumerable<Contract> list = _itemService.GetContract(id, it);
            string name = null;
            if (item.TempMode == 2) {
                name = pdfService.ContractToPDF2(model, item, list, norm, weight, mode, freight, entity, place, date, data, lt);
            }
            else {
                name = pdfService.ContractToPDF1(model, item, list, norm, weight, mode, freight, entity, place, date, data, lt);
            }
            return Json(new { code = 200, message = "保存成功！", name = name });
        }

        public ActionResult ReportToPDF(int id = -1)
        {
            PdfService pdfService = new PdfService();
            var item = _itemService.Get(id);
            string name = null;
            switch (item.TempMode)
            {
                case 1:
                    name = pdfService.ReportToPDF1(item, _itemService.GetContrastPrice(id));
                    break;
                case 2:
                    name = pdfService.ReportToPDF2(item, _itemService.GetContrastPrice(id));
                    break;
                case 3:
                    name = pdfService.ReportToPDF3(item, _itemService.GetContrastPrice(id));
                    break;
                case 4:
                    name = pdfService.ReportToPDF4(item, _itemService.GetContrastPrice(id));
                    break;
                case 5:
                    name = pdfService.ReportToPDF5(item, _itemService.GetContrastPrice(id));
                    break;
                default:
                    break;
            }
            return Json(new { code = 200, message = "文件生成成功！", name = name });
        }

        public ActionResult ItemSubmit(List<string[]> rw)
        {
            if (rw == null)
            {
                return Json(new { code = 500, message = "未进行任何选价操作，无法保存！" });
            }
            List<ItemSure> list = new List<ItemSure>();
            rw.AsEnumerable().ToList().ForEach(p =>
            {
                list.Add(new ItemSure()
                {
                    MaterialID = Convert.ToInt32(p[0]),
                    QuoteID = Convert.ToInt32(p[1]),
                    Memo = p[2],
                    InputPerson = UserId,
                    InputTime = DateTime.Now
                });
            });
            _itemSureService.Update(list.AsEnumerable());
            return Json(new { code = 500, message = "保存成功！" });
        }

        public ActionResult GetModeJson(int uid = 0)
        {
            IEnumerable<Mode> list = _modeService.ModeList();
            return Json(new { data = list.Where(p => p.ParentID == -1 && p.IsEnabled == true).Select(t => new { name = t.ModeName, data = list.Where(m => m.ParentID == t.Id && m.IsEnabled == true).Select(v => new { id = v.Id, name = v.ModeName }) }) });
        }

        public ActionResult EmailNotice(int id = -1)
        {
            EmailService service = new EmailService();
            var item = _itemService.Get(id);
            if (item.IsUpdate)
            {
                var list = _itemService.GetItemSuppliers(id);
                string st = "尊敬的：<b>{0}</b> 试供货商 您好！<br/>  您有一份关于青岛武晓集团采购(<b>{1}</b>) 物料计划进行了修改，请在<b>{2}</b>前给予报价！谢谢！<br /><div style='width:100%;float:right;'>青岛武晓集团<br/><b>报价网址：http://www.wuxiao.cn 供货商管理系统点击进入 <b/></div><div style='width:100%'>{3}</div>";
                list.ToList().ForEach(p => service.SendEmail(p.Email, "物料信息询价通知", string.Format(st, p.SupplierName, item.ItemName, item.EndDate.ToString("yyyy年MM月dd日 HH:mm:ss"), item.StartDate.ToString("yyyy年MM月dd日 HH:mm:ss"))));
                item.IsNotice = true;
                _itemService.Update(item);
            }
            else
            {
                var list = _itemService.GetItemSuppliers(id);
                string st = "尊敬的：<b>{0}</b> 试供货商 您好！<br/>  您有一份关于青岛武晓集团采购(<b>{1}</b>) 物料计划，请在<b>{2}</b>前给予报价！谢谢！<br /><div style='width:100%;float:right;'>青岛武晓集团<br/><b>报价网址：http://www.wuxiao.cn 供货商管理系统点击进入 <b/></div><div style='width:100%'>{3}</div>";
                list.ToList().ForEach(p => service.SendEmail(p.Email, "物料信息询价通知", string.Format(st, p.SupplierName, item.ItemName, item.EndDate.ToString("yyyy年MM月dd日 HH:mm:ss"), item.StartDate.ToString("yyyy年MM月dd日 HH:mm:ss"))));
                item.IsNotice = true;
                _itemService.Update(item);
            }
            return this.Json(new { code = 200, message = "邮件通知发送成功！！" });
        }

        /// <summary>
        /// PDF文件下载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult DownFile(string fileName)
        {
            string path = $"{AppDomain.CurrentDomain.BaseDirectory}/UploadFile/pdf/{fileName}";
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
    }
}