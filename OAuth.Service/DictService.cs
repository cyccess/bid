using OAuth.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using OAuth.Core.Interfaces;

namespace OAuth.Service
{
    public class DictService : IDictService
    {
        private readonly IRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public DictService(IRepository repo, IUnitOfWork work)
        {
            this._repo = repo;
            this._unitOfWork = work;
        }

        public Dict Get(string name, string id)
        {
            var entity = _repo.GetAll<Dict>().Where(p => p.DictName == name && p.DictKey == id ).FirstOrDefault();
            return entity;
        }

        public IEnumerable<Dict> GetDicts(string name)
        {
            var list = _repo.GetAll<Dict>().Where(p => p.DictName == name).OrderBy(u => u.Sort);
            return list;
        }


    }
}
