using Mersani.models.Finance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace Mersani.Interfaces.Finance
{
    public interface IVoucherBudgetRepo
    {
        Task<DataSet> GetVoucherBudget(int id, string authParms);
      
        Task<DataSet> GetALLVoucherBudget(int id, string authParms);
        Task<DataSet> GetVoucherBudgetTrans(int VoucherBudgetMaster, string authParms);
        Task<DataSet> PostVoucherBudget(VoucherBudget entities, string authParms);
       ////////////////////////////
        Task<DataSet> GetPostedVoucherBudget(int id, string authParms);
        Task<DataSet> GetUnPostedVoucherBudget(int id, char postedType, string authParms);
        Task<DataSet> PostedVoucherBudgetHdr(List<VoucherBudgetHdr> entities, string authParms);
        Task<DataSet> deleteVoucherBudget(VoucherBudget entities, string authParms);
        Task<DataSet> GetLastCode(string authParms);
    }
}
