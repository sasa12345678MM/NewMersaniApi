using Mersani.models.PointOfSale;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.PointOfSale
{
    public interface IPharmacyShiftRepo
    {
        Task<DataSet> GetPharmacyShiftMaster(PharmacyShiftMaster entity, string authParms);
        Task<DataSet> GetPharmacyShiftDetails(PharmacyShiftMaster entity, string authParms);
        Task<DataSet> PostPharmacyShiftMasterDetails(PharmacyShiftData entity, string authParms);
        Task<DataSet> DeletePharmacyShiftMasterDetails(PharmacyShiftDetail entity, int type, string authParms);

        Task<DataSet> CheckUserShiftForPharmacy(PharmacyShiftActivation data, string authParms);
        Task<DataSet> GetPharmacyLastShiftCashLeft(int pharmacyId, string authParms);

        Task<DataSet> PostPharmacyShift(PharmacyShiftActivation shift, string authParms);
        Task<DataSet> GetCashierShiftTotals(int shiftId, string authParms);
    }
}
