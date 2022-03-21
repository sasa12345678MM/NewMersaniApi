using System.Data;
using System.Threading.Tasks;
using Mersani.models.FinancialSetup;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface OwnerSupplierRepo
    {
        Task<DataSet> GetOwnerSupplierDataList(OwnerSupplier OwnerSupplier, string authParms);
        Task<DataSet> BulkInsertUpdateOwnerSupplierData(OwnerSupplier OwnerSupplier, string authParms);
        Task<DataSet> DeleteOwnerSupplierData(OwnerSupplier OwnerSupplier, string authParms);
        Task<DataSet> GetLastCode(string authParms);

    }
}
