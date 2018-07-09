using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuth.Domain.IRepository
{
    public interface IReportRepository
    {
        IEnumerable<ItemContrast> GetContrastPrice(int id);

        IEnumerable<Contract> GetContract(int id, int spId);
    }
}