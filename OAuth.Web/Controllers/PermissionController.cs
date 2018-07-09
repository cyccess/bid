using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using OAuth.Web.Models;
using System;
using System.Dynamic;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Web.Controllers
{
    public class PermissionController : BaseController
    {
        private readonly IPermissionService permissionService;
        private readonly IProjectService projectService;
        private readonly ProjectInfo ProjectInfo;


        public PermissionController(IPermissionService permissionService, IProjectService projectService, ProjectInfo projectInfo)
        {
            this.permissionService = permissionService;
            this.projectService = projectService;
            this.ProjectInfo = projectInfo;
        }

        //
        // GET: /Permission/
        public ActionResult Index(string id, int pageIndex = 1)
        {
            dynamic viewModel = new ExpandoObject();

            var currentProject = ProjectInfo.CurrentProject(id);
            var pageData = permissionService.GetPermissionList(pageIndex, currentProject.Id);

            viewModel.PageList = pageData;
            viewModel.CurrentProject = currentProject;

            return View(viewModel);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Permission entity, int moduleId)
        {
            var data = permissionService.Add(entity, moduleId);
            return Json(new { code = 200, message = "添加成功！", action = data });
        }

        public ActionResult Edit(int id = 0)
        {
            var entity = permissionService.GetPermissionById(id);
            return View(entity);
        }

        [HttpPost]
        public ActionResult Edit(Permission entity)
        {
            permissionService.Update(entity);
            return Json(new { status = 1, message = "修改成功！" });
        }

        [HttpPost]
        public ActionResult Delete(int id = 0)
        {
            permissionService.Delate(id);
            return Json(new { code = 200, message = "删除成功！" });
        }
    }
}