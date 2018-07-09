using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;
using OAuth.Service.ModelDto;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service.Interfaces
{
    public interface ISupplierService
    {
        Supplier Get(int id);

        void Add(SupplierDto entity);

        bool CheckEmail(string email);

        SupplierDto GetSupplierById(int id);

        IPagedList<Supplier> GetSuppliers(int pageIndex, int ModeId, string SupplierName);

        void Update(SupplierDto entity);

        void Delate(int id);

        ResultModel UpdatePassword(int supplierId, string oldPwd, string newPwd);

        void SupplierActive(Supplier entity);

        void EmailNotice(int supplierId);

        void ForgetPassword(int supplierId);

        SupplierDto Login(string username, string password);


        void MaterialQuote(ItemQuoteDto model);

        ItemQuote GetItemQuote(int id);
    }
}
