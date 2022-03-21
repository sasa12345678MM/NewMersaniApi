using Mersani.models.FinancialSetup;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface ISupplierClassRepo
    {
        Task<DataSet> GetSupplierClassDataList(SupplierClass supplierClass, string authParms);
        Task<DataSet> BulkInsertUpdateSupplierData(List<SupplierClass> supplierClasses, string authParms);
        Task<DataSet> DeleteSupplierData(SupplierClass supplierClass, string authParms);
    }
}
