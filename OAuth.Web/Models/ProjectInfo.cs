using OAuth.Domain.Model;
using OAuth.Service.Interfaces;
using System;

namespace OAuth.Web.Models
{
    /// <summary>
    /// 项目的缓存信息
    /// </summary>
    public class ProjectInfo
    {
        private readonly IProjectService projectService;

        public ProjectInfo(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        /// <summary>
        /// 当前项目信息
        /// </summary>
        /// <param name="appid">项目唯一编码</param>
        /// <returns></returns>
        public Project CurrentProject(string appid)
        {
            return projectService.GetProjectById(new Guid(appid));
        }
    }
}