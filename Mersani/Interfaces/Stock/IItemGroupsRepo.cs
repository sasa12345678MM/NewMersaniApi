using System.Data;
using Mersani.models.Stock;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mersani.Interfaces.Stock
{
    public interface IItemGroupsRepo
    {
        Task<DataSet> GetItemsGroups(ItemGroups entity, string authParms);
        Task<DataSet> BulkItemsGroups(List<ItemGroups> entities, string authParms);
        Task<DataSet> DeleteItemGroup(ItemGroups entity, string authParms);
    }
}
