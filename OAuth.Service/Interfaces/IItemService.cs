using OAuth.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace OAuth.Service.Interfaces
{
    public interface IItemService
    {
        Item Get(int id);

        Item Add(Item item,IEnumerable<ItemMode> list);

        Item Update(Item item);

        Item Update(Item item, IEnumerable<ItemMode> list);

        void Delete(int[] dt);

        int GetItemCount();

        //Item EmailNotice(Item item);

        IEnumerable<Supplier> GetItemSuppliers(int itemId);

        IPagedList<Item> GetItems(string ItemName, string Contractor, string SignPlace, int pageIndex);

        IPagedList<Item> GetQuoteItems(int pageIndex);

        IEnumerable<ItemContrast> GetContrastPrice(int id);

        IEnumerable<Contract> GetContract(int id,int spId);

        bool CheckItemExpire(int itemId);
    }
}
