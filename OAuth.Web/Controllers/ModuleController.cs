using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using OAuth.Web.Models;
using System.Dynamic;
using System.Web.Mvc;

namespace OAuth.Web.Controllers
{
    public class ModuleController : BaseController
    {
        private IModuleService moduleService;
        private IProjectService projectService;
        private ProjectInfo ProjectInfo;

        public ModuleController(IModuleService moduleService, IProjectService projectService, ProjectInfo ProjectInfo)
        {
            this.moduleService = moduleService;
            this.projectService = projectService;
            this.ProjectInfo = ProjectInfo;
        }

        public ActionResult Index(string id, bool isparent = false, string moduleNo = "")
        {
            dynamic viewModel = new ExpandoObject();

            var currentProject = ProjectInfo.CurrentProject(id);
            var pageList = moduleService.GetModuleList(currentProject.Id, isparent, moduleNo);

            viewModel.PageList = pageList;
            viewModel.CurrentProjectId = currentProject.Uniqueid.ToString("N");
            viewModel.CurrentProject = currentProject;
            viewModel.IsParent = isparent;
            viewModel.ParentModule = moduleService.GetParentModuleList(currentProject.Id);

            return View(viewModel);
        }

        public ActionResult Add(string appid)
        {
            var project = ProjectInfo.CurrentProject(appid);
            var moduleList = moduleService.GetParentModuleList(project.Id);
            ViewBag.AppId = appid;
            return View(moduleList);
        }

        [HttpPost]
        public ActionResult Add(Module model, string appid)
        {
            var project = ProjectInfo.CurrentProject(appid);
            model.ProjectId = project.Id;
            moduleService.Add(model);
            return Json(new { status = 1, message = "添加成功！" });
        }

        public ActionResult Edit()
        {
            //var entity = moduleService.GetModuleById(id);
            // ViewBag.ParentModuleList = moduleService.GetParentModuleList(entity.ProjectId);
            //ViewBag.ProjectList = projectService.GetProjectList();
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Module entity)
        {
            moduleService.Update(entity);
            return Json(new { code = 200, message = "修改成功！" });
        }

        [HttpPost]
        public ActionResult Delete(int moduleId = 0)
        {
            moduleService.Delate(moduleId);
            return Json(new { code = 200, message = "删除成功" });
        }

        //获取模块
        [HttpPost]
        public ActionResult GetModule(int projectId, int moduleId)
        {
            var module = moduleService.GetModuleById(moduleId);
            var actions = moduleService.GetPermissionListByModuleId(moduleId);
            var data = new { module = module, actions = actions };

            return Json(data);
        }

        //获取动作列表
        [HttpPost]
        public ActionResult GetActionList(int moduleId)
        {
            var model = moduleService.GetPermissionListByModuleId(moduleId);
            return Json(model);
        }
    }
}