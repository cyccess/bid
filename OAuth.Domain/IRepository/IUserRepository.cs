using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuth.Domain.Model;

namespace OAuth.Domain.IRepository
{
    public interface IUserRepository
    {
        User Get(int id);

    }
}
