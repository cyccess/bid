using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service.Interfaces
{
    public interface IPurchaserService
    {
        IPagedList<Purchaser> GetPurchasers(int pageIndex);

        IEnumerable<Purchaser> GetPurchasers();

        void Add(Purchaser entity);

        Purchaser Get(int id);

        void Upadte(Purchaser entity);

        void Detele(int id);
    }
}
