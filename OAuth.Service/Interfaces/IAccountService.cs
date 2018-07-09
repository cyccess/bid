using OAuth.Service.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Service.Interfaces
{
    public interface IAccountService
    {
        UserInfo Login(string username, string password, UserType userType);

        void SignOut();

        ResultModel SupplierActive(string supplierId);

        ResultModel SupplierModifyPwd(string supplierId, string oldPwd, string newPwd);
    }
}
