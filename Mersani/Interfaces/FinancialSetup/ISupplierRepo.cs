using System.Data;
using System.Threading.Tasks;
using Mersani.models.FinancialSetup;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface ISupplierRepo
    {
        Task<DataSet> GetSupplierDataList(Supplier supplier, string authParms);
        Task<DataSet> BulkInsertUpdateSupplierData(Supplier supplier, string authParms);
        Task<DataSet> DeleteSupplierData(Supplier supplier, string authParms);
        Task<DataSet> GetLastCode(string authParms);
    }
}
