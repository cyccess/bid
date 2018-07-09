using OAuth.Domain.Model;
using OAuth.Service.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service.Interfaces
{
    public interface IItemMaterialService
    {
        ItemMaterial Get(int id);

        ItemMaterialDto Add(ItemMaterialDto item);

        void Add(IEnumerable<ItemMaterial> list);

        void Delete(int[] array);


        ItemMaterialDto Update(ItemMaterialDto model);

        IPagedList<ItemMaterial> GetItemMaterials(int itemId, int pageIndex);


        IPagedList<ItemMaterial> GetQuoteItemMaterials(int materialId, int pageIndex);

        ItemQuoteDto GetItemQuote(int materialId);

        ItemQuoteDto GetMaterialSureQuote(int materialId);
    }
}
