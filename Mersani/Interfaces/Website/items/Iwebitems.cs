using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mersani.Interfaces.Website.items
{
    public interface Iwebitems
    {
        Task<DataSet> GetItemsWithPriceAndDiscount(int itemId, int Curr, string authParms);
        Task<DataSet> GetItemOffers(int Curr, string authParms);

        Task<DataSet> GetItemsWithName(string name, int Curr, string authParms);

        Task<DataSet> GetItemsImages(int itemId, string authParms);

        Task<DataSet> GetItemsByGroup(int GroupId, int curr, string authParms);
        Task<DataSet> GetItemsByMenufacturer(int facId, int curr, string authParms);

        Task<DataSet> GetRelatedItems(int GroupId, int curr, string authParms);

    }
}
