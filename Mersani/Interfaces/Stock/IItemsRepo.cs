using System.Data;
using Mersani.models.Stock;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mersani.Interfaces.Stock
{
    public interface IItemsRepo
    {
        Task<DataSet> GetItems(Items entity, string authParms);
        Task<DataSet> GetItemsWithCriteria(Items criteria, string authParms);
        Task<DataSet> PostItemMasterDetails(Items entity, string authParms);
        Task<DataSet> PostItem(List<Items> entities, string authParms);
        Task<DataSet> DeleteItemMasterDetails(Items entity, string authParms);
        Task<DataSet> GetLastItemCode(string authParms);


        #region Assempld
        Task<DataSet> GetInvItemAssempld(InvItemAssempldItems entity, string authParms);
        public Task<DataSet> PostInvItemAssempld(List<InvItemAssempldItems> entities, string authParms);
        public Task<DataSet> DeleteInvItemAssempld(InvItemAssempldItems entity, string authParms);
        #endregion


        #region units
        public Task<DataSet> GetItemUnits(ItemUnits entity, string authParms);
        public Task<DataSet> PostItemUnits(List<ItemUnits> entities, string authParms);
        public Task<DataSet> DeleteItemUnits(ItemUnits entity, string authParms);
        #endregion


        #region alternatives
        public Task<DataSet> GetInvItemAlternative(invItemAlternative entity, string authParms);
        public Task<DataSet> PostItemAlternative(List<invItemAlternative> entities, string authParms);
        public Task<DataSet> DeleteItemAlternative(invItemAlternative entity, string authParms);
        #endregion


        #region related
        public Task<DataSet> GetInvItemRelated(invItemRelated entity, string authParms);
        public Task<DataSet> PostItemRelated(List<invItemRelated> entities, string authParms);
        public Task<DataSet> DeleteItemRelated(invItemRelated entity, string authParms);
        #endregion


        #region images
        public Task<DataSet> GetInvItemImages(InvItemImages entity, string authParms);
        public Task<DataSet> PostInvItemImages(List<InvItemImages> entities, string authParms);
        public Task<DataSet> DeleteInvItemImages(InvItemImages entity, string authParms);
        #endregion


        #region prices
        public Task<DataSet> GetInvItemMasterPrices(InvItemMasterPrices entity, string authParms);
        public Task<DataSet> PostInvItemMasterPrices(List<InvItemMasterPrices> entities, string authParms);
        public Task<DataSet> DeleteInvItemMasterPrices(InvItemMasterPrices entity, string authParms);
        #endregion


        #region batches
        public Task<DataSet> GetInvItemBatches(invitemMasterBatches entity, string authParms);
        public Task<DataSet> PostItemMasterBatches(List<invitemMasterBatches> entities, string authParms);
        public Task<DataSet> DeleteItemMasterBatches(invitemMasterBatches entity, string authParms);
        #endregion


    }
}
