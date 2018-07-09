using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OAuth.Service.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjectList();

        IEnumerable<Project> GetProjectsAndParts();


        Project GetProjectById(Guid uniqueid);


        void Add(Project entity);


        void Prefs(int pid, int isStar);

    }
}
