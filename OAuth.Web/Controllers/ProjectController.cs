using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using OAuth.Web.Models;
using System;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace OAuth.Web.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectService projectService;
        private readonly IRoleService roleService;
        private readonly ProjectInfo ProjectInfo;


        public ProjectController(IProjectService projectService, IRoleService roleService, ProjectInfo ProjectInfo)
        {
            this.projectService = projectService;
            this.roleService = roleService;
            this.ProjectInfo = ProjectInfo;
        }

        //
        // GET: /Project/
        public ActionResult Index()
        {
            dynamic viewModel = new ExpandoObject();
            var projects = projectService.GetProjectList();
            viewModel.ProjectList = projects;
            viewModel.CommonProjectList = projects.OrderBy(p => p.AddDate).Where(p => p.IsCommonUsed == true).ToList();
            return View(viewModel);
        }

        //角色管理列表
        public ActionResult Detail(string id)
        {
            dynamic viewModel = new ExpandoObject();
            var currentProject = ProjectInfo.CurrentProject(id);

            viewModel.RoleList = roleService.RoleList(currentProject.Id);
            viewModel.CurrentProjectId = currentProject.Uniqueid.ToString("N");
            viewModel.Project = currentProject;

            return View(viewModel);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Project model)
        {
            projectService.Add(model);
            return Json(model);
        }

        [HttpPost]
        public ActionResult Prefs(int pid, int is_star = 0)
        {
            projectService.Prefs(pid, is_star);
            return Json(new { code = 200 });
        }

        //首页项目下拉选择部分视图
        public ActionResult ProjectDropdownMenu()
        {
            return PartialView(projectService.GetProjectList());
        }


        [HttpPost]
        public ActionResult GetProjectsAndParts()
        {
            var list = projectService.GetProjectsAndParts();
            return Json(list);
        }

        [HttpPost]
        public ActionResult GetProjects()
        {
            var list = projectService.GetProjectList();
            return Json(list);
        }
    }
}