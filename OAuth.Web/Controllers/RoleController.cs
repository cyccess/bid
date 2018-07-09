using OAuth.Domain.Model;
using OAuth.Service;
using OAuth.Service.Interfaces;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace OAuth.Web.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService roleService;
        private readonly IProjectService projectService;
        private readonly IModuleService moduleService;

        public RoleController(IRoleService roleService, IModuleService moduleService, IProjectService projectService)
        {
            this.roleService = roleService;
            this.moduleService = moduleService;
            this.projectService = projectService;
        }

        //
        // GET: /Role/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRoleById(int roleId)
        {
            var entity = roleService.GetRoleById(roleId);
            return Json(entity);
        }

        public ActionResult Add()
        {
            ViewBag.ProjectList = projectService.GetProjectList();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Role model)
        {
            roleService.AddRole(model);
            return Json(new { code = 200, message = "添加成功！", model = model });
        }

        //角色权限设置视图
        public ActionResult GetRoleRight(int projectId = 0, int roleId = 0)
        {
            var entity = roleService.GetRoleRightById(projectId, roleId);
            return PartialView(entity);
        }

        //设置角色权限
        [HttpPost]
        public ActionResult SetRoleRight(int roleId, int[] permissionArr, bool isChecked)
        {
            roleService.AddRoleRight(roleId, permissionArr, isChecked);
            return Json(new { status = 1, message = isChecked ? "角色权限设置成功！" : "角色权限取消成功！" });

        }

        //设置角色权限
        [HttpPost]
        public ActionResult SetSingleRoleRight(int roleId, int[] permissionArr, bool isChecked)
        {
            roleService.SetSingleRoleRight(roleId, permissionArr, isChecked);
            return Json(new { status = 1, message = isChecked ? "角色权限设置成功！" : "角色权限取消成功！" });
        }

        //删除角色
        [HttpPost]
        public ActionResult Delete(int roleId)
        {
            roleService.Delete(roleId);
            return Json(new { code = 200, message = "删除成功" });
        }

        //重命名
        [HttpPost]
        public ActionResult Rename(Role role)
        {
            roleService.Rename(role);
            return Json(new { code = 200, message = "操作成功" });
        }

        //复制角色
        [HttpPost]
        public ActionResult Copy(Role role)
        {
            roleService.Copy(role);
            return Json(new { code = 200, message = "操作成功", model = role });
        }

        //设置用户角色
        [HttpPost]
        public ActionResult SetPart(int userId, int partId)
        {
            roleService.SetPart(userId, partId);
            return Json(new { code = 200, message = "设置成功" });
        }
    }
}