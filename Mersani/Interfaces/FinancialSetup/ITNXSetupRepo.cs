using Mersani.models.FinancialSetup;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mersani.Interfaces.FinancialSetup
{
    public interface ITNXSetupRepo
    {
        Task<DataSet> GetTXNSetupDataList(TXNSetup voucherType, string authParms);
        Task<DataSet> BulkInsertUpdateTXNSetup(List<TXNSetup> voucherTypes, string authParms);
        Task<DataSet> DeleteTXNSetupData(TXNSetup voucherType, string authParms);
    }
}