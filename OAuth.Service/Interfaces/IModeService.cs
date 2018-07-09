using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service.Interfaces
{
    public interface IModeService
    {
        void Add(Mode entity);

        void Delete(int id);

        void Update(Mode entity);

        IEnumerable<Mode> ModeList();

        Mode GetModeById(int modeId);

        IPagedList<Mode> GetParentModes(int pageIndex);

        IPagedList<Mode> GetChildrenModes(int parentID,int pageIndex);
    }
}
