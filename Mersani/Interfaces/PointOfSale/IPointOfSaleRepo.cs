using Mersani.models.PointOfSale;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.PointOfSale
{
    public interface IPointOfSaleRepo
    {
        Task<DataSet> GetPointOfSaleMaster(PointOfSaleMASTER entity, string authParms);

        Task<DataSet> GetPointOfSaleDetails(PointOfSaleDetails entity, string authParms);
        Task<DataSet> GetPointOfSaleLastCode(string authParms);
        Task<DataSet> GetItemDataByBarCode(string barcode, int stockId, string authParms);
        Task<DataSet> GetItemWithinNearbyPharamcies(int itemId, string authParms);
        Task<DataSet> PostPointOfSaleMASTERDetails(PointOfSaleData entities, string authParms);
        Task<DataSet> DeletePointOfSaleMASTERDetails(PointOfSaleDetails entity, int type, string authParms);

        Task<DataSet> BulkPointOfSale(List<PointOfSaleMASTER> entities, string authParms);

        Task<DataSet> GetItemsByInventory(int stockId, string authParms);
        Task<DataSet> GetDefaultAccounts(string authParms);
        Task<DataSet> GetDefaultPrinters(string authParms);

        Task<DataSet> GetSalesOrdersNotifactions(int pharmacyId, string authParms);
        List<dynamic> GetReminders(int pharmacyId, string authParms);

        Task<DataSet> getPosShiftsExpenses(IPosShiftsExpenses entity, string authParms);
        Task<DataSet> SavePosShiftsExpenses(List<IPosShiftsExpenses> entities, string authParms);

        Task<DataSet> GetPOSInvPromotions(int invId, string authParms);
        Task<DataSet> CheckItemDiscount(int itemId, decimal Qty, string authParms);
        Task<DataSet> CheckInvoiceDiscount(decimal total, string authParms);
        Task<DataSet> CheckLinkConnection();

        Task<DataSet> SyncDataFromLocalToLive(string authParms);


    }
}
