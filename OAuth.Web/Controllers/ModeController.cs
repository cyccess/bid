using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OAuth.Web.Controllers
{
    public class ModeController : BaseController
    {
        private readonly IModeService _modeService;

        public ModeController(IModeService modeService)
        {
            this._modeService = modeService;
        }
        // GET: Mode
        public ActionResult Index(int pageIndex = 1)
        {
            var list = _modeService.GetParentModes(pageIndex);
            return View(list);
        }

        public ActionResult Edit(Mode entity)
        {
            if (entity.Id == -1)
            {
                entity.IsEnabled = true;
                _modeService.Add(entity);
            }
            else {
                _modeService.Update(entity);
            }
            return Json(new { code = 200, message = "操作成功！" });
        }

        public ActionResult Detail(int id = -1, int pageIndex = 1)
        {
            var list = _modeService.GetChildrenModes(id, pageIndex);
            ViewBag.Id = id;
            return View(list);
        }

        public ActionResult Get(int id = -1)
        {
            Mode entity = null;
            if (id == -1)
            {
                entity = new Mode();
                entity.Id = -1;
            }
            else {
                entity = _modeService.GetModeById(id);
            }
            return Json(entity);
        }

        public ActionResult Delete(int id = -1)
        {
            if (id==-1)
            {
                return Json(new { code = 200, message = "暂无可操作数据！" });
            }
            _modeService.Delete(id);
            return Json(new { code = 200, message = "操作执行成功！" });
        }
    }
}