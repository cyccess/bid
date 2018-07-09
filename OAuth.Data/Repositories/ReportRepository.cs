using OAuth.Core.Interfaces;
using OAuth.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using OAuth.Domain.Model;

namespace OAuth.Data.Repositories
{
    public class ReportRepository : BaseRepository, IReportRepository
    {
        private readonly IDbContext context;
        public ReportRepository(IDbContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }

        public IEnumerable<Contract> GetContract(int id,int spId)
        {
            return context.Set<Contract>().SqlQuery("select ID,Name,TempMode,Texture,Spec,Extent,Norm,Amount,Unit,TotalWeight,Price,TotalPrice,PriceUnit,Memo from dbo.vi_contract where ItemID = {0} and SupplierID={1}", id, spId).ToList();
        }

        public IEnumerable<ItemContrast> GetContrastPrice(int id)
        {
            //var entity = new { id = id };
            return context.Set<ItemContrast>().SqlQuery("exec sp_offer_price @id", new SqlParameter[1] { new SqlParameter("@id", id) }).ToList();
        }
    }
}
